using CommandLine;

namespace dcsrt
{
    public class CommandLineArguments
    {
        [Option('s', "source-path", Required = true, HelpText = "Directory path containing PDF documents to process. All PDFs in this directory will be analyzed or used for training.")]
        public string SourcePath { get; set; }

        [Option('c', "codex-path", Required = false, HelpText = "Path to the Codex JSON file containing document classification rules. If not specified, uses Codex.json from the executable directory.")]
        public string CodexPath { get; set; }

        [Option('d', "dry-run", Required = false, HelpText = "Preview mode: analyze and match documents without moving files. Use this to test rules before applying changes.", Default = false)]
        public bool DryRunMode { get; set; }

        [Option('x', "silently-exit", Required = false, HelpText = "Exit immediately after completion without waiting for user input. Useful for automated scripts and batch processing.", Default = false)]
        public bool SilentlyExit { get; set; }
    }
}
