using CommandLine;

namespace dcsrt
{
    [Verb("training", false, HelpText = "Generate optimal keyword vocabulary from sample PDF documents. Analyzes document frequency to identify distinctive keywords for classification rules.")]
    public class TrainingCommandLineArguments : CommandLineArguments
    {
    }
}
