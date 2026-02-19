using FuzzySharp.Extractor;
using FuzzySharp.SimilarityRatio;
using FuzzySharp.SimilarityRatio.Scorer;
using FuzzySharp.SimilarityRatio.Scorer.Composite;
using NLog;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dcsrt
{
    public static partial class AnalysisActivity
    {
        public static void Invoke(AnalysisCommandLineArguments arguments)
        {
            using (MiniProfiler.Current.Step("Beginning Main Analysis"))
            {
                DirectoryAnalysisResponse response = AnalysisActivity.AnalyzeDirectory(arguments.SourcePath, arguments.SearchDepth, arguments.MatchingStrategy, arguments.MatchingThreshold, arguments.DryRunMode, arguments.MatchPercentage);

                if (response.InvalidDocuments.Count > 0)
                {
                    string value = null;

                    if (arguments.SilentlyDeleteInvalidPDFs)
                    {
                        value = "Y";
                    }
                    else
                    {
                        Console.WriteLine("Do you want to delete the invalid PDFs encountered? (Y/n)");
                        foreach (FileAnalysisResponse document in response.InvalidDocuments)
                        {
                            Console.WriteLine(document.SourceFilePath);
                        }
                        value = Console.ReadLine();
                    }

                    if (string.Equals(value, "Y"))
                    {
                        foreach (FileAnalysisResponse document in response.InvalidDocuments)
                        {
                            File.Delete(document.SourceFilePath);
                        }
                    }
                }
            }
        }

        public static DirectoryAnalysisResponse AnalyzeDirectory(string sourceDirectoryPath, SearchOption searchOption, MatchingStrategy matchingStrategy, int matchingThreshold = 0, bool dryrun = false, int matchPercentage = 70, int pagesToAnalyze = 1)
        {
            DirectoryAnalysisResponse response = new DirectoryAnalysisResponse();

            Dictionary<string, string> documents = TrainingActivity.LoadDocuments(sourceDirectoryPath, searchOption, pagesToAnalyze);

            foreach (KeyValuePair<string, string> document in documents)
            {
                FileAnalysisResponse fileResponse = AnalysisActivity.AnalyzeFile(document.Key, document.Value, !dryrun, matchingStrategy, matchingThreshold, matchPercentage);

                if (fileResponse.IsValid)
                {
                    response.ValidDocuments.Add(fileResponse);
                }
                else
                {
                    response.InvalidDocuments.Add(fileResponse);
                }
            }

            Program.Context.Logger.Log(LogLevel.Info, "Completed analyzing Documents.");

            return response;
        }

        public static FileAnalysisResponse AnalyzeFile(string sourceFilePath, string content, bool moveFileOnMatch, MatchingStrategy matchingStrategy, int matchingThreshold = 0, int matchPercentage = 70)
        {
            FileAnalysisResponse response = new FileAnalysisResponse(sourceFilePath);

            Program.Context.Logger.Log(LogLevel.Trace, "Analyzing source file [{0}].", sourceFilePath);

            if (response.SourceFile.Exists)
            {
                using (StackExchange.Profiling.Timing step = MiniProfiler.Current.Step($"Testing source file [{response.SourceFile.FullName}] against codex."))
                {
                    IReadOnlyList<Codex> matchedCodexEntries = AnalysisActivity.FindCodexMatches(content, matchingStrategy, matchingThreshold, matchPercentage);

                    response.IsValid = true;
                    response.HasRuleCollision = matchedCodexEntries.Count > 1;
                    response.MatchedCodex = matchedCodexEntries.FirstOrDefault();

                    foreach (Codex candidate in matchedCodexEntries)
                    {
                        response.CandidateRuleIds.Add(candidate.RuleId);
                    }

                    if (response.HasRuleCollision)
                    {
                        Program.Context.Logger.Log(
                            LogLevel.Warn,
                            "Source file [{0}] matched multiple codex entries [{1}]. Deterministically choosing [{2}] by highest keyword count and RuleId sort.",
                            sourceFilePath,
                            string.Join(", ", response.CandidateRuleIds),
                            response.MatchedCodex?.RuleId ?? "None");
                    }

                    if (response.IsMatchedToCodex)
                    {
                        Program.Context.Logger.Log(
                                 LogLevel.Info,
                                 "Source file [{0}] matched to rule [{1}] in [{2}ms] and will be moved to the destination [{3}].",
                                 sourceFilePath,
                                 response.MatchedCodex.RuleId,
                                 step.DurationMilliseconds,
                                 response.DestinationFilePath);

                        if (moveFileOnMatch)
                        {
                            if (!response.TryMoveFile())
                            {
                                Program.Context.Logger.Log(
                                    LogLevel.Info,
                                    "Source file [{0}] was {1}moved.",
                                    sourceFilePath,
                                    File.Exists(sourceFilePath) ? "not " : string.Empty);
                            }
                        }

                        Program.Context.Logger.Log(LogLevel.Debug, response.MatchedCodex);
                    }
                    else
                    {
                        Program.Context.Logger.Log(LogLevel.Trace, "Unable to find a Codex match for [{0}].", sourceFilePath);
                    }
                }
            }

            return response;
        }

        public static Codex FindCodexMatch(string sourceContent, MatchingStrategy matchingStrategy, int matchingThreshold = 0, int matchPercentage = 70)
        {
            return AnalysisActivity.FindCodexMatches(sourceContent, matchingStrategy, matchingThreshold, matchPercentage).FirstOrDefault();
        }

        public static IReadOnlyList<Codex> FindCodexMatches(string sourceContent, MatchingStrategy matchingStrategy, int matchingThreshold = 0, int matchPercentage = 70)
        {
            List<Codex> matches = new List<Codex>();

            foreach (Codex codex in Program.Context.Codex)
            {
                using (MiniProfiler.Current.Step($"Testing codex [{codex.RuleId}] against document."))
                {
                    bool isMatchedToCodexEntry = AnalysisActivity.MatchDocument(sourceContent, codex.Words, matchingStrategy, matchingThreshold, matchPercentage);

                    if (isMatchedToCodexEntry)
                    {
                        matches.Add(codex);
                    }
                }
            }

            return matches.OrderByDescending(c => c.Words?.Count() ?? 0).ThenBy(c => c.RuleId, StringComparer.OrdinalIgnoreCase).ToList();
        }

        public static IOrderedEnumerable<WordOccurrenceDetails> GetWordCountsMatchingOccurrenceParameters(string content, uint minCount, uint maxCount)
        {
            return content.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .GroupBy(r => r, StringComparer.InvariantCultureIgnoreCase)
                .Where(grp => grp.Count() > minCount && grp.Count() <= maxCount)
                .Select(grp => new WordOccurrenceDetails() { Word = grp.Key, Count = grp.Count() })
                .Where(grp => grp.Word.Length > 3)
                .OrderBy(r => r.Count);
        }

        public static IEnumerable<string> GetWordsMatchingOccurrenceParameters(string content, uint minCount, uint maxCount, uint retryThreshold = 2)
        {
            IOrderedEnumerable<WordOccurrenceDetails> analysis = AnalysisActivity.GetWordCountsMatchingOccurrenceParameters(content, minCount, maxCount);

            IEnumerable<string> result = analysis.Select(r => r.Word);

            if (result.Count() < retryThreshold)
            {
                result = AnalysisActivity.GetWordsMatchingOccurrenceParameters(content, minCount, (maxCount + 1), retryThreshold);
            }

            return result;
        }

        public static bool MatchDocument(string content, IEnumerable<string> words, MatchingStrategy matchingStrategy, int matchingThreshold = 0, int matchPercentage = 70)
        {
            if (string.IsNullOrWhiteSpace(content) || words == null)
            {
                return false;
            }

            string[] contentArray = { };
            int totalWords = words.Count();
            int matchedWords = 0;

            switch (matchingStrategy)
            {
                case MatchingStrategy.Probabilistic:
                    {
                        contentArray = content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    }
                case MatchingStrategy.StringSearch:
                    {
                        content = content.ToUpperInvariant();
                        break;
                    }
                default:
                    {
                        throw new NotSupportedException($"The Matching Strategy [{matchingStrategy}] is not supported.");
                    }
            }

            foreach (string word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                {
                    continue;
                }

                using (MiniProfiler.Current.Step($"Comparing word [{word}] to document content."))
                {
                    bool wordMatch = false;

                    switch (matchingStrategy)
                    {
                        case MatchingStrategy.Probabilistic:
                            {
                                if (matchingThreshold == 0)
                                {
                                    throw new InvalidOperationException("Matching threshold must be greater than zero.");
                                }

                                wordMatch = AnalysisActivity.MatchDocumentWithProbabilisticMatchingStrategy(matchingThreshold, contentArray, word);
                                break;
                            }
                        case MatchingStrategy.StringSearch:
                            {
                                wordMatch = content.Contains(word.ToUpperInvariant());
                                break;
                            }
                        default:
                            {
                                throw new NotSupportedException($"The Matching Strategy [{matchingStrategy}] is not supported.");
                            }
                    }

                    if (wordMatch)
                    {
                        matchedWords++;
                    }
                }
            }

            double actualMatchPercentage = (double)matchedWords / totalWords;
            double requiredPercentage = matchPercentage / 100.0;
            Program.Context.Logger.Log(LogLevel.Debug, $"Document match: {matchedWords}/{totalWords} words ({actualMatchPercentage:P0})");
            return actualMatchPercentage >= requiredPercentage;
        }

        private static bool MatchDocumentWithProbabilisticMatchingStrategy(int matchingThreshold, string[] normalizedContentArray, string word)
        {
            string normalizedWord = word.ToLowerInvariant();
            IRatioScorer scorer = ScorerCache.Get<WeightedRatioScorer>();
            ExtractedResult<string> analysis = FuzzySharp.Process.ExtractOne(normalizedWord, normalizedContentArray, s => s, scorer, 0);
            bool match = (analysis != null) && (analysis.Score >= matchingThreshold);
            return match;
        }
    }
}
