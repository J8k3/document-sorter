using Newtonsoft.Json;
using NLog;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dcsrt
{
    public static class TrainingActivity
    {
        public static void Invoke(TrainingCommandLineArguments arguments)
        {
            using (MiniProfiler.Current.Step("Beginning Training"))
            {
                WordBag wordBag = WordBag.FromPDFsInDirectory(arguments.SourcePath, SearchOption.TopDirectoryOnly, arguments.PagesToAnalyze);

                double bestFrequency = 0;
                double bestMatchRate = 0;
                int bestVocabSize = 0;

                for (double freq = 0.90; freq >= 0.10; freq -= 0.05)
                {
                    IEnumerable<string> vocabulary = wordBag.GetCandidateVocabulary(freq);
                    int vocabSize = vocabulary.Count();

                    if (vocabSize == 0)
                    {
                        continue;
                    }

                    double matchRate = TrainingActivity.TestCandidateVocabulary(wordBag.RawDocuments.Values, vocabulary, arguments.MatchPercentage);

                    Program.Logger.Log(LogLevel.Info, $"Frequency {freq:P0}: {vocabSize} words, {matchRate:F2}% match rate");

                    if (bestMatchRate == 0 || matchRate > bestMatchRate || (matchRate == bestMatchRate && vocabSize < bestVocabSize))
                    {
                        bestFrequency = freq;
                        bestMatchRate = matchRate;
                        bestVocabSize = vocabSize;
                    }
                }

                if (bestFrequency > 0)
                {
                    IEnumerable<string> vocabulary = wordBag.GetCandidateVocabulary(bestFrequency);
                    Program.Logger.Log(LogLevel.Info, $"\nOptimal: {bestFrequency:P0} frequency → {bestVocabSize} words → {bestMatchRate:F2}% match rate");
                    
                    string ruleId = GenerateRuleId(arguments.SourcePath);
                    string destination = GenerateDestination(arguments.SourcePath);
                    
                    Codex rule = new Codex
                    {
                        RuleId = ruleId,
                        Words = vocabulary.ToList(),
                        Destination = destination
                    };
                    
                    Program.Logger.Log(LogLevel.Info, JsonConvert.SerializeObject(rule, Formatting.Indented));
                }
                else
                {
                    Program.Logger.Log(LogLevel.Warn, "No vocabulary found.");
                }

                Program.Logger.Log(LogLevel.Info, "Completed analyzing documents.");
            }
        }

        private static string GenerateRuleId(string sourcePath)
        {
            DirectoryInfo dir = new DirectoryInfo(sourcePath);
            return dir.Name.Replace(" ", "-");
        }

        private static string GenerateDestination(string sourcePath)
        {
            DirectoryInfo dir = new DirectoryInfo(sourcePath);
            return dir.FullName;
        }

        private static double TestCandidateVocabulary(IEnumerable<string> trainingDocuments, IEnumerable<string> vocabulary, int matchPercentage)
        {
            Program.Logger.Log(LogLevel.Info, "Testing WordBags against training data set.");
            Program.Logger.Log(LogLevel.Info, $"Vocabulary size: {vocabulary.Count()} words");
            Program.Logger.Log(LogLevel.Info, $"Training documents: {trainingDocuments.Count()} documents");

            double matches = 0;

            foreach (string trainingDocument in trainingDocuments)
            {
                if (AnalysisActivity.MatchDocument(trainingDocument, vocabulary, MatchingStrategy.StringSearch, 0, matchPercentage))
                {
                    matches++;
                }
            }

            return (matches / trainingDocuments.Count() * 100);
        }
    }
}
