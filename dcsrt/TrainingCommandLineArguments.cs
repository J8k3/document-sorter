using CommandLine;

namespace dcsrt
{
    [Verb("training", false, HelpText = "Generate optimal keyword vocabulary from sample PDF documents. Analyzes document frequency to identify distinctive keywords for classification rules.")]
    public class TrainingCommandLineArguments : CommandLineArguments
    {
        [Option('a', "pages-to-analyze", Required = false, HelpText = "Number of pages to extract from each PDF for analysis. Default is 1 (first page only). Use higher values if keywords appear on later pages.", Default = 1)]
        public int PagesToAnalyze { get; set; }

        [Option('p', "match-percentage", Required = false, HelpText = "Minimum percentage of keywords that must match (0-100) when testing vocabulary against training documents. Lower values (e.g., 30-50) allow fewer keyword matches, higher values (e.g., 70-90) require stricter matching. Default 70.", Default = 70)]
        public int MatchPercentage { get; set; }
    }
}
