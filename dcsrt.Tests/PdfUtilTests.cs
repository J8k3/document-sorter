using NUnit.Framework;
using System.IO;
using System.Text;

namespace dcsrt.Tests
{
    [TestFixture(TestOf = typeof(PdfUtil))]
    public class PdfUtilTests
    {
        [TestCase()]
        public void ExtractTextFromPDFTest()
        {
            string path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\typographic_terms.pdf");
            FileInfo file = new FileInfo(path);
            string input = PdfUtil.ExtractTextFromPDF(file);
            string result = input.ToString().Substring(0, 50);
            Assert.That(result, Is.EqualTo("Typographic Terms\nW\nhen older typesetting methods "));
        }
    }
}
