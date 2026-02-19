using CommandLine;
using StackExchange.Profiling;
using System;

namespace dcsrt
{
    internal class Program
    {
        public const string ScanSnapCreator = "PFU ScanSnap Organizer 5.6.40 #iX500";
        public const string ScanSnapProducer = "PFU PDF Library 1.4.1";

        internal static AppContext Context;

        static Program()
        {
            Program.Context = new AppContext();
        }

        static void Main(string[] args)
        {
            MiniProfiler.StartNew();

            CommandLineArguments arguments = null;

            using (MiniProfiler.Current.Step("Parsing command line arguments."))
            {
                Parser.Default.ParseArguments<AnalysisCommandLineArguments, TrainingCommandLineArguments>(args).WithParsed(o => arguments = (CommandLineArguments)o);
            }

            if (arguments != null)
            {
                using (MiniProfiler.Current.Step("Loading Codex."))
                {
                    CodexManager.Reload(arguments.CodexPath);
                }

                if (arguments.GetType() == typeof(AnalysisCommandLineArguments))
                {
                    AnalysisActivity.Invoke((AnalysisCommandLineArguments)arguments);
                }
                else if (arguments.GetType() == typeof(TrainingCommandLineArguments))
                {
                    TrainingActivity.Invoke((TrainingCommandLineArguments)arguments);
                }
                else
                {
                    throw new NotSupportedException("The command line verb is not supported.");
                }
            }

            MiniProfiler.Current.Stop();

            if (MiniProfiler.Current != null)
            {
                Program.Context.Logger.Log(NLog.LogLevel.Trace, MiniProfiler.Current.RenderPlainText());
            }

            if (arguments != null)
            {
                if (!arguments.SilentlyExit)
                {
                    Console.ReadLine();
                }
            }
        }
    }
}
