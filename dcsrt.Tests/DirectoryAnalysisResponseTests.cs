using dcsrt;
using NUnit.Framework;

namespace dcsrt.Tests
{
    [TestFixture(TestOf = typeof(DirectoryAnalysisResponse))]
    public class DirectoryAnalysisResponseTests
    {
        [Test()]
        public void DirectoryAnalysisResponse_Constructor_InitializesLists()
        {
            DirectoryAnalysisResponse response = new DirectoryAnalysisResponse();
            Assert.That(response.ValidDocuments, Is.Not.Null);
            Assert.That(response.InvalidDocuments, Is.Not.Null);
            Assert.That(response.ValidDocuments.Count, Is.EqualTo(0));
            Assert.That(response.InvalidDocuments.Count, Is.EqualTo(0));
        }

        [Test()]
        public void DirectoryAnalysisResponse_AddValidDocument()
        {
            DirectoryAnalysisResponse response = new DirectoryAnalysisResponse();
            FileAnalysisResponse fileResponse = new FileAnalysisResponse("test.pdf") { IsValid = true };
            response.ValidDocuments.Add(fileResponse);
            Assert.That(response.ValidDocuments.Count, Is.EqualTo(1));
        }

        [Test()]
        public void DirectoryAnalysisResponse_AddInvalidDocument()
        {
            DirectoryAnalysisResponse response = new DirectoryAnalysisResponse();
            FileAnalysisResponse fileResponse = new FileAnalysisResponse("test.pdf") { IsValid = false };
            response.InvalidDocuments.Add(fileResponse);
            Assert.That(response.InvalidDocuments.Count, Is.EqualTo(1));
        }
    }
}
