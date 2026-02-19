using CommandLine;
using System.IO;

namespace dcsrt
{
    [Verb("analysis", true, HelpText = "Analyze and classify PDF documents by matching content against defined rules, then move files to their destination folders.")]
    public class AnalysisCommandLineArguments : CommandLineArguments
    {
        [Option('r', "recursive", Required = false, HelpText = "Search subdirectories recursively. Use AllDirectories to process entire folder tree, or TopDirectoryOnly for current folder only.", Default = SearchOption.AllDirectories)]
        public SearchOption SearchDepth { get; set; }

        [Option('i', "delete-invalid", Required = false, HelpText = "Automatically delete corrupted or unreadable PDF files without prompting. Use with caution.", Default = false)]
        public bool SilentlyDeleteInvalidPDFs { get; set; }

        [Option('t', "matching-threshold", Required = false, HelpText = "Fuzzy matching confidence threshold (0-100). Increase (e.g., 95) for stricter typo tolerance, decrease (e.g., 85) to allow more variations. Only applies when using Probabilistic matching strategy.", Default = 92)]
        public int MatchingThreshold { get; set; }

        [Option('p', "match-percentage", Required = false, HelpText = "Minimum percentage of keywords that must match (0-100). Lower values (e.g., 50) allow fewer keyword matches, higher values (e.g., 90) require more strict matching. Default 70 means 70% of keywords must be found.", Default = 70)]
        public int MatchPercentage { get; set; }

        [Option('m', "matching-strategy", Required = false, HelpText = "Algorithm for matching keywords: StringSearch (fast, exact) or Probabilistic (slower, fuzzy matching with typo tolerance).", Default = MatchingStrategy.StringSearch)]
        public MatchingStrategy MatchingStrategy { get; set; }
    }
}
