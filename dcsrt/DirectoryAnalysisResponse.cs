using System;
using System.Collections.Generic;
using System.Linq;

namespace dcsrt
{
    public class DirectoryAnalysisResponse
    {
        public DirectoryAnalysisResponse()
        {
            this.ValidDocuments = new List<FileAnalysisResponse>();
            this.InvalidDocuments = new List<FileAnalysisResponse>();
        }
        public List<FileAnalysisResponse> ValidDocuments { get; set; }
        public List<FileAnalysisResponse> InvalidDocuments { get; set; }
    }
}
