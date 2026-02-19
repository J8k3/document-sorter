using dcsrt;
using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;

namespace dcsrt.Tests
{
    [TestFixture(TestOf = typeof(FileAnalysisResponse))]
    public class FileAnalysisResponseTests
    {
        [Test()]
        public void FileAnalysisResponse_Constructor_SetsSourceFilePath()
        {
            string path = "C:\\test\\file.pdf";
            FileAnalysisResponse response = new FileAnalysisResponse(path);
            Assert.That(response.SourceFilePath, Is.EqualTo(path));
            Assert.That(response.CandidateRuleIds, Is.Not.Null);
            Assert.That(response.CandidateRuleIds.Count, Is.EqualTo(0));
        }

        [Test()]
        public void FileAnalysisResponse_IsMatchedToCodex_ReturnsTrueWhenMatched()
        {
            FileAnalysisResponse response = new FileAnalysisResponse("test.pdf")
            {
                MatchedCodex = new Codex { RuleId = "TestRule" }
            };
            Assert.That(response.IsMatchedToCodex, Is.True);
        }

        [Test()]
        public void FileAnalysisResponse_IsMatchedToCodex_ReturnsFalseWhenNotMatched()
        {
            FileAnalysisResponse response = new FileAnalysisResponse("test.pdf");
            Assert.That(response.IsMatchedToCodex, Is.False);
        }

        [Test()]
        public void FileAnalysisResponse_DestinationFilePath_BuildsCorrectPath()
        {
            string testFile = Path.Combine(Path.GetTempPath(), "test_codex_" + Path.GetRandomFileName() + ".json");
            try
            {
                CodexDictionary testCodex = new CodexDictionary
                {
                    RootPath = "C:\\Root",
                    Dictionary = new[]
                    {
                        new Codex { RuleId = "TestRule", Destination = "SubFolder", Words = new[] { "test" } }
                    }
                };
                string json = JsonConvert.SerializeObject(testCodex);
                File.WriteAllText(testFile, json);
                CodexManager.Reload(testFile);
                FileAnalysisResponse response = new FileAnalysisResponse("C:\\source\\test.pdf")
                {
                    IsValid = true,
                    MatchedCodex = new Codex { RuleId = "TestRule", Destination = "SubFolder" }
                };
                string expected = "C:\\Root\\SubFolder\\test.pdf";
                Assert.That(response.DestinationFilePath, Is.EqualTo(expected));
            }
            finally
            {
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }

        [Test()]
        public void FileAnalysisResponse_DestinationFilePath_ReturnsNullWhenNotMatched()
        {
            FileAnalysisResponse response = new FileAnalysisResponse("test.pdf");
            Assert.That(response.DestinationFilePath, Is.Null);
        }
    }
}
