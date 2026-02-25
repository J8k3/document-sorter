using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using NLog;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static SymSpell;

namespace dcsrt
{
    public static class PdfUtil
    {
        public const string PDFSearchPattern = "*.pdf";
        private static SymSpell spellChecker = null;

        public static string ExtractTextFromPDF(FileInfo file, int maxPages = -1)
        {
            StringBuilder documentContentBuilder = new StringBuilder();

            using (MiniProfiler.Current.Step("Reading Pdf"))
            {
                using (PdfReader reader = new PdfReader(file.FullName))
                {
                    using (PdfDocument document = new PdfDocument(reader))
                    {
                        int pageCount = document.GetNumberOfPages();

                        if (maxPages == -1)
                        {
                            maxPages = pageCount;
                        }

                        for (int i = 1; i <= maxPages; i++)
                        {
                            PdfPage page = document.GetPage(i);

                            using (MiniProfiler.Current.Step($"Extracting Page [{i}] Content"))
                            {
                                string pageContent = PdfTextExtractor.GetTextFromPage(page, new SimpleTextExtractionStrategy());
                                                                
                                documentContentBuilder.Append(pageContent);
                            }
                            if (i == pageCount)
                            {
                                break;
                            }
                        }

                        document.Close();
                    }

                    reader.Close();
                }
            }

            return documentContentBuilder.ToString();
        }

        public static Dictionary<string, string> LoadDocuments(string sourceDirectoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly, int pagesToExtractPerDocument = 1)
        {
            Dictionary<string, string> documents = new Dictionary<string, string>();

            Program.Logger.Log(LogLevel.Info, "Searching for files in the source path [{0}].", sourceDirectoryPath);

            IEnumerable<string> sourceFilePaths = FileSystemUtil.GetFilesBelowPath(sourceDirectoryPath, PdfUtil.PDFSearchPattern, searchOption);

            Program.Logger.Log(LogLevel.Info, "Found [{0}] files in the source path [{1}].", sourceFilePaths.Count(), sourceDirectoryPath);

            int current = 0;
            int total = sourceFilePaths.Count();
            foreach (string sourceFilePath in sourceFilePaths)
            {
                current++;
                ProgressTracker.Update(current, total);
                string content = PdfUtil.LoadDocument(sourceFilePath, pagesToExtractPerDocument);
                documents.Add(sourceFilePath, content);
            }

            ProgressTracker.Complete();

            Program.Logger.Log(LogLevel.Info, "Completed loading documents.");

            return documents;
        }

        public static string LoadDocument(string sourceFilePath, int pagesToExtract, bool spellCheck = true)
        {
            string result = string.Empty;

            Program.Logger.Log(LogLevel.Trace, "Loading document [{0}].", sourceFilePath);

            FileInfo sourceFile = new FileInfo(sourceFilePath);

            try
            {
                if (sourceFile.Exists)
                {
                    using (StackExchange.Profiling.Timing step = MiniProfiler.Current.Step($"Reading document [{sourceFile.FullName}] contents."))
                    {
                        string sourceContent = PdfUtil.ExtractTextFromPDF(sourceFile, pagesToExtract);

                        if (sourceContent.Length > 0)
                        {
                            sourceContent = sourceContent.ReplaceLineBreaksWithSpace();
                            sourceContent = sourceContent.RemoveAllNonAlphaNumericCharacters(" ");
                            sourceContent = sourceContent.RemoveDuplicateWhitespace();
                            sourceContent = sourceContent.RemoveStopWords();

                            if (spellCheck)
                            {
                                using (MiniProfiler.Current.Step($"Spell checking document [{sourceFilePath}]."))
                                {
                                    Program.Logger.Log(LogLevel.Debug, "Applying spell check to document [{0}].", sourceFilePath);
                                    
                                    if (spellChecker == null)
                                    {
                                        spellChecker = new SymSpell(initialCapacity: 82765, maxDictionaryEditDistance: 2, prefixLength: 7);
                                        string path = AppDomain.CurrentDomain.BaseDirectory + "frequency_dictionary_en_82_765.txt";
                                        if (!spellChecker.LoadDictionary(path, 0, 1))
                                        {
                                            throw new FileNotFoundException("SymSpell Dictionary File not found: " + Path.GetFullPath(path));
                                        }
                                    }

                                    string[] tokens = sourceContent.Split(new[] { ' ' }, StringSplitOptions.None);
                                    StringBuilder sb = new StringBuilder(sourceContent.Length);
                                    int correctionCount = 0;

                                    for (int i = 0; i < tokens.Length; i++)
                                    {
                                        string word = tokens[i];

                                        if (PdfUtil.ShouldCorrect(word))
                                        {
                                            List<SuggestItem> suggestions = spellChecker.Lookup(word, Verbosity.Top, 2, includeUnknown: true);
                                            if (suggestions != null && suggestions.Count > 0)
                                            {
                                                SuggestItem suggestion = suggestions[0];
                                                string correctedWord = suggestion.term;
                                                int distance = suggestion.distance;
                                                if (PdfUtil.AcceptCorrection(word, correctedWord, distance))
                                                {
                                                    Program.Logger.Log(LogLevel.Trace, "Corrected '{0}' to '{1}' (distance: {2})", word, correctedWord, distance);
                                                    word = correctedWord;
                                                    correctionCount++;
                                                }
                                            }
                                        }

                                        sb.Append(word);

                                        if (i < tokens.Length - 1)
                                        {
                                            sb.Append(" ");
                                        }
                                    }

                                    sourceContent = sb.ToString();
                                    Program.Logger.Log(LogLevel.Debug, "Applied {0} spell corrections to document [{1}].", correctionCount, sourceFilePath);
                                }
                            }

                            result = sourceContent;
                        }
                        else
                        {
                            Program.Logger.Log(LogLevel.Info, $"[{sourceFilePath}] does not contain readable text.");
                        }
                    }
                }
            }
            catch (PdfException ex)
            {
                Program.Logger.Log(LogLevel.Warn, ex, "The document [{0}] is invalid. {1}", sourceFilePath, ex.Message);
            }

            return result;
        }

        private static bool ShouldCorrect(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            if (token.Length < 3)
            {
                return false;
            }

            if (token.Length > 25)
            {
                return false;
            }

            int letters = token.Count(char.IsLetter);
            int digits = token.Count(char.IsDigit);

            if (letters == 0)
            {
                return false;
            }

            if (digits > 0 && digits >= letters)
            {
                return false;
            }

            if (token.All(char.IsUpper) && token.Length <= 4)
            {
                return false;
            }

            return true;
        }

        private static bool AcceptCorrection(string original, string corrected, int distance)
        {
            if (string.IsNullOrWhiteSpace(corrected))
            {
                return false;
            }

            if (string.Equals(original, corrected, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (corrected.Length < original.Length - 1)
            {
                return false;
            }

            if (distance > 2)
            {
                return false;
            }

            if (original.All(char.IsUpper))
            {
                if (!corrected.All(char.IsUpper))
                {
                    return false;
                }

                if (Math.Abs(corrected.Length - original.Length) > 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
