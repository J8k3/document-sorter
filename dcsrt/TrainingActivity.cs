using iText.Kernel.Exceptions;
using Newtonsoft.Json;
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
    public static class TrainingActivity
    {
        public const int MaxNumberOfSourceTrainingDocuments = 100;

        // The fuzzy matching threshold for a keyword in a given training source document
        public const int DocumentMatchingThreshold = 92;

        public const double MinimumAcceptableMatchRate = 95.0;

        public static void Invoke(TrainingCommandLineArguments arguments)
        {
            using (MiniProfiler.Current.Step("Beginning Training"))
            {
                WordBag wordBag = WordBag.FromPDFsInDirectory(arguments.SourcePath);

                double bestFrequency = 0;
                double bestMatchRate = 0;
                int bestVocabSize = 0;

                for (double freq = 0.90; freq >= 0.10; freq -= 0.05)
                {
                    var vocabulary = wordBag.GetCandidateVocabulary(freq);
                    int vocabSize = vocabulary.Count();

                    if (vocabSize == 0)
                    {
                        continue;
                    }

                    var matchRate = TrainingActivity.TestCandidateVocabulary(wordBag.RawDocuments.Values, vocabulary);

                    Program.Context.Logger.Log(LogLevel.Info, $"Frequency {freq:P0}: {vocabSize} words, {matchRate:F2}% match rate");

                    if (matchRate >= TrainingActivity.MinimumAcceptableMatchRate && (bestMatchRate == 0 || vocabSize < bestVocabSize))
                    {
                        bestFrequency = freq;
                        bestMatchRate = matchRate;
                        bestVocabSize = vocabSize;
                    }
                }

                if (bestFrequency > 0)
                {
                    var vocabulary = wordBag.GetCandidateVocabulary(bestFrequency);
                    Program.Context.Logger.Log(LogLevel.Info, $"\nOptimal: {bestFrequency:P0} frequency → {bestVocabSize} words → {bestMatchRate:F2}% match rate");
                    Program.Context.Logger.Log(LogLevel.Info, JsonConvert.SerializeObject(vocabulary, Formatting.Indented));
                }
                else
                {
                    Program.Context.Logger.Log(LogLevel.Warn, $"Could not find vocabulary with {TrainingActivity.MinimumAcceptableMatchRate}%+ match rate. Using 50% frequency threshold.");
                    var vocabulary = wordBag.GetCandidateVocabulary(0.50);
                    var matchRate = TrainingActivity.TestCandidateVocabulary(wordBag.RawDocuments.Values, vocabulary);
                    Program.Context.Logger.Log(LogLevel.Info, $"Fallback: {vocabulary.Count()} words, {matchRate:F2}% match rate");
                    Program.Context.Logger.Log(LogLevel.Info, JsonConvert.SerializeObject(vocabulary, Formatting.Indented));
                }

                Program.Context.Logger.Log(LogLevel.Info, "Completed analyzing documents.");
            }
        }

        private static double TestCandidateVocabulary(IEnumerable<string> trainingDocuments, IEnumerable<string> vocabulary)
        {
            Program.Context.Logger.Log(LogLevel.Info, "Testing WordBags against training data set.");
            Program.Context.Logger.Log(LogLevel.Info, $"Vocabulary size: {vocabulary.Count()} words");
            Program.Context.Logger.Log(LogLevel.Info, $"Training documents: {trainingDocuments.Count()} documents");
            
            double matches = 0;

            foreach (var trainingDocument in trainingDocuments)
            {
                if (AnalysisActivity.MatchDocument(trainingDocument, vocabulary, MatchingStrategy.StringSearch, 0))
                {
                    matches++;
                }
            }

            return (matches / trainingDocuments.Count() * 100);
        }

        public static Dictionary<string, string> LoadDocuments(string sourceDirectoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly, int pagesToExtractPerDocument = 1)
        {
            Dictionary<string, string> documents = new Dictionary<string, string>();

            Program.Context.Logger.Log(LogLevel.Info, "Searching for files in the source path [{0}].", sourceDirectoryPath);

            IEnumerable<string> sourceFilePaths = FileSystemUtil.GetFilesBelowPath(sourceDirectoryPath, PdfUtil.PDFSearchPattern, searchOption);

            Program.Context.Logger.Log(LogLevel.Info, "Found [{0}] files in the source path [{1}].", sourceFilePaths.Count(), sourceDirectoryPath);

            foreach (string sourceFilePath in sourceFilePaths)
            {
                Console.Write(".");
                string content = TrainingActivity.LoadDocument(sourceFilePath, pagesToExtractPerDocument);
                documents.Add(sourceFilePath, content);
            }


            Program.Context.Logger.Log(LogLevel.Info, "Completed loading documents.");

            return documents;
        }

        public static string LoadDocument(string sourceFilePath, int pagesToExtract, bool spellCheck = true)
        {
            string result = string.Empty;

            Program.Context.Logger.Log(LogLevel.Trace, "Loading document [{0}].", sourceFilePath);

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
                            sourceContent = sourceContent.RemoveAllNonAlphaNumericCharacters(string.Empty);
                            sourceContent = sourceContent.RemoveExcludeWordsToLowerInvariant();

                            if (spellCheck)
                            {
                                SymSpell spell = new SymSpell(initialCapacity: sourceContent.Length, maxDictionaryEditDistance: 2, prefixLength: 7);

                                string path = AppDomain.CurrentDomain.BaseDirectory + "frequency_dictionary_en_82_765.txt";

                                if (!spell.LoadDictionary(path, 0, 1)) 
                                {
                                    throw new FileNotFoundException("SymSpell Dictionary File not found: " + Path.GetFullPath(path));
                                }

                                string[] tokens = sourceContent.Split(new[] { ' ' }, StringSplitOptions.None);
                                StringBuilder sb = new StringBuilder(sourceContent.Length);

                                for (int i = 0; i < tokens.Length; i++)
                                {
                                    string word = tokens[i];

                                    if (TrainingActivity.ShouldCorrect(word))
                                    {
                                        List<SuggestItem> suggestions = spell.Lookup(word.ToLowerInvariant(), Verbosity.Top, 2, includeUnknown: true);
                                        if (suggestions != null && suggestions.Count > 0)
                                        {
                                            SuggestItem suggestion = suggestions[0];
                                            string correctedWord = suggestion.term;
                                            int distance = suggestion.distance;
                                            if (TrainingActivity.AcceptCorrection(word, correctedWord, distance))
                                            {
                                                word = correctedWord;
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
                            }

                            result = sourceContent.ToLowerInvariant();
                        }
                        else
                        {
                            Program.Context.Logger.Log(LogLevel.Info, $"[{sourceFilePath}] does not contain readable text.");
                        }
                    }
                }
            }
            catch (PdfException ex)
            {
                Program.Context.Logger.Log(LogLevel.Warn, ex, "The document [{0}] is invalid. {1}", sourceFilePath, ex.Message);
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

            // Don�t �correct� to something much shorter (USAA -> US)
            // Dont correct to something much shorter (USAA -> US)
            if (corrected.Length < original.Length - 1)
            {
                return false;
            }

            // If it changed a lot, skip (distance already gives this, but double-check)
            // If it changed a lot, skip (distance already gives this, but double-check)
            if (distance > 2)
            {
                return false;
            }

            // If original is all-caps, only accept if corrected is also all-caps and same length-ish
            // If original is all-caps, only accept if corrected is also all-caps and same length-ish
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
