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
        //https://www.codeproject.com/Tips/1064878/Ranking-Tokens-using-TF-IDF-in-Csharp
        private WordBag()
        { }

        public Dictionary<string, string> RawDocuments { get; private set; }

        public int DocumentCount { get; private set; }

        public static WordBag FromPDFsInDirectory(string sourceDirectoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly, int pagesToExtractPerDocument = 1)
        {
            WordBag wordBag = new WordBag();

            wordBag.RawDocuments = TrainingActivity.LoadDocuments(sourceDirectoryPath, searchOption, pagesToExtractPerDocument);

            foreach (var sourceDocument in wordBag.RawDocuments)
            {
                Console.Write(".");
                Program.Context.Logger.Log(LogLevel.Trace, "Loading document [{0}].", sourceDocument.Key);

                using (StackExchange.Profiling.Timing step = MiniProfiler.Current.Step($"Getting document vocabulary [{sourceDocument.Key}]."))
                {
                    if (sourceDocument.Value.Length > 0)
                    {
                        wordBag.DocumentCount++;

                        IEnumerable<string> contentWords = sourceDocument.Value.TokenizeToLowerInvariant();

                        foreach (string contentWord in contentWords)
                        {
                            int tokenIndex = wordBag.IndexOf(contentWord);

                            if (tokenIndex == -1) { wordBag.Add(new Token(ref wordBag, contentWord)); tokenIndex = wordBag.Count() - 1; }

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

                foreach (var d in occurrences)
                {
                    d.MostFrequentWordInDocumentCount = maxWordCountInDoc;
                }
            }

            return wordBag;
        }
        
        public IEnumerable<string> GetCandidateVocabulary(double minDocumentFrequency)
        {
            Program.Context.Logger.Log(LogLevel.Info, "Getting candidate vocabulary from WordBag.");

            return this.Where(t => t.DocumentFrequency >= minDocumentFrequency).Select(t => t.Word);
        }

        public int IndexOf(string word)
        {
            Token item = this.Where(t => string.Equals(t.Word, word, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            return this.IndexOf(item);
        }
    }
}
