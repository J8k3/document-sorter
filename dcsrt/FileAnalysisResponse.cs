using System.Collections.Generic;
using System.IO;
using NLog;
using StackExchange.Profiling;

namespace dcsrt
{
    public class FileAnalysisResponse
    {
        private FileInfo m_SourceFile;
        private string m_DestinationFilePath;

        public FileAnalysisResponse(string sourceFilePath)
        {
            this.SourceFilePath = sourceFilePath;
            this.CandidateRuleIds = new List<string>();
        }

        public bool IsValid { get; set; }

        public Codex MatchedCodex { get; set; }

        public bool HasRuleCollision { get; set; }

        public IList<string> CandidateRuleIds { get; private set; }

        public bool IsMatchedToCodex
        {
            get
            {
                return (this.MatchedCodex != null);
            }
        }

        public string SourceFilePath
        {
            get;
            private set;
        }

        public FileInfo SourceFile
        {
            get
            {
                if (this.m_SourceFile == null)
                {
                    this.m_SourceFile = new FileInfo(this.SourceFilePath);
                }
                return this.m_SourceFile;
            }
        }

        public string DestinationFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DestinationFilePath))
                {
                    if (this.IsValid && this.IsMatchedToCodex)
                    {
                        this.m_DestinationFilePath = System.IO.Path.Combine(CodexManager.Codex.RootPath, this.MatchedCodex.Destination, this.SourceFile.Name);
                    }
                }
                return this.m_DestinationFilePath;
            }
        }

        public bool TryMoveFile()
        {
            bool success = true;

            using (MiniProfiler.Current.Step($"[{this.SourceFile.FullName}] {((this.IsValid && this.IsMatchedToCodex) ? "WILL" : "WILL NOT")} be moved to [{this.DestinationFilePath}]."))
            {
                try
                {
                    if (this.IsValid && this.IsMatchedToCodex)
                    {
                        FileSystemUtil.MoveFile(this.SourceFile, this.DestinationFilePath);
                        success = File.Exists(this.DestinationFilePath);
                    }
                }
                catch (System.IO.IOException ex)
                {
                    success = false;
                    Program.Context.Logger
                        .Log(LogLevel.Error,
                             ex,
                             "Failed to move [{0}] to [{1}]. {2} {3}",
                             this.SourceFilePath,
                             this.DestinationFilePath,
                             ex.Message,
                             ex.StackTrace);
                }
            }

            return success;
        }
    }
}
