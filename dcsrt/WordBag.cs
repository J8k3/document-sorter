using NLog;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace dcsrt
{
    public class WordBag : List<Token>
    {
        private Dictionary<string, int> tokenIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private WordBag()
        { }

        public Dictionary<string, string> RawDocuments { get; private set; }

        public int DocumentCount { get; private set; }

        public static WordBag FromPDFsInDirectory(string sourceDirectoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly, int pagesToExtractPerDocument = 1)
        {
            WordBag wordBag = new WordBag();

            wordBag.RawDocuments = PdfUtil.LoadDocuments(sourceDirectoryPath, searchOption, pagesToExtractPerDocument);

            int current = 0;
            int total = wordBag.RawDocuments.Count;
            foreach (KeyValuePair<string, string> sourceDocument in wordBag.RawDocuments)
            {
                current++;
                ProgressTracker.Update(current, total);
                Program.Logger.Log(LogLevel.Trace, "Loading document [{0}].", sourceDocument.Key);

                using (StackExchange.Profiling.Timing step = MiniProfiler.Current.Step($"Getting document vocabulary [{sourceDocument.Key}]."))
                {
                    if (sourceDocument.Value.Length > 0)
                    {
                        wordBag.DocumentCount++;

                        IEnumerable<string> contentWords = sourceDocument.Value.TokenizeToLowerInvariant();

                        foreach (string contentWord in contentWords)
                        {
                            int tokenIndex = wordBag.IndexOf(contentWord);

                            if (tokenIndex == -1)
                            {
                                wordBag.Add(new Token(ref wordBag, contentWord));
                                tokenIndex = wordBag.Count - 1;
                                wordBag.tokenIndex[contentWord] = tokenIndex;
                            }

                            int documentIndex = wordBag[tokenIndex].IndexOf(sourceDocument.Key);

                            if (documentIndex == -1) { wordBag[tokenIndex].Add(new DocumentOccurrence(ref wordBag, contentWord, sourceDocument.Key)); documentIndex = (wordBag[tokenIndex].Count - 1); }

                            wordBag[tokenIndex][documentIndex].OccurrenceCount++;
                        }
                    }
                }

                IEnumerable<DocumentOccurrence> occurrences = (from token in wordBag
                                                                 from occurence in token
                                                                 where string.Equals(occurence.DocumentPath, sourceDocument.Key, StringComparison.Ordinal)
                                                                 select occurence);

                double maxWordCountInDoc = (from d in occurrences
                                            select d.OccurrenceCount).DefaultIfEmpty(0).Max();

                foreach (DocumentOccurrence d in occurrences)
                {
                    d.MostFrequentWordInDocumentCount = maxWordCountInDoc;
                }
            }

            ProgressTracker.Complete();

            return wordBag;
        }
        
        public IEnumerable<string> GetCandidateVocabulary(double minDocumentFrequency)
        {
            Program.Logger.Log(LogLevel.Info, "Getting candidate vocabulary from WordBag.");

            return this.Where(t => t.DocumentFrequency >= minDocumentFrequency).Select(t => t.Word);
        }

        public int IndexOf(string word)
        {
            if (this.tokenIndex.TryGetValue(word, out int index))
            {
                return index;
            }
            return -1;
        }
    }
}
