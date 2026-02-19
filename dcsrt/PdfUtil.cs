using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using StackExchange.Profiling;
using System.IO;
using System.Text;

namespace dcsrt
{
    public static class PdfUtil
    {
        public const string PDFSearchPattern = "*.pdf";

        public static string ExtractTextFromPDF(FileInfo file, int maxPages = -1)
        {
            StringBuilder documentContentBuilder = new StringBuilder();

            using (MiniProfiler.Current.Step("Reading Pdf"))
            {
                using (var reader = new PdfReader(file.FullName))
                {
                    using (var document = new PdfDocument(reader))
                    {
                        int pageCount = document.GetNumberOfPages();

                        if (maxPages == -1)
                        {
                            maxPages = pageCount;
                        }

                        for (int i = 1; i <= maxPages; i++)
                        {
                            var page = document.GetPage(i);

                            using (MiniProfiler.Current.Step($"Extracting Page [{i}] Content"))
                            {
                                string pageContent = PdfTextExtractor.GetTextFromPage(page, new SimpleTextExtractionStrategy());
                                                                
                                documentContentBuilder.Append(pageContent);
                            }
                            if (i == pageCount)
                            {
                                break;
                            }
                        }

                        document.Close();
                    }

                    reader.Close();
                }
            }

            return documentContentBuilder.ToString();
        }
    }
}
