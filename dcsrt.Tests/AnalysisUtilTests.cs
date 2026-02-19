using dcsrt;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace dcsrt.Tests
{
    [TestFixture(TestOf = typeof(AnalysisActivity))]
    public class AnalysisActivityTests
    {
        [Test()]
        public void MatchDocument_StringSearch_ExactMatch()
        {
            string content = "This is a test document with specific keywords";
            IEnumerable<string> words = new[] { "test", "document", "keywords" };
            bool result = AnalysisActivity.MatchDocument(content, words, MatchingStrategy.StringSearch, 0, 70);
            Assert.That(result, Is.True);
        }

        [Test()]
        public void MatchDocument_StringSearch_PartialMatch()
        {
            string content = "This is a test document";
            IEnumerable<string> words = new[] { "test", "missing", "keywords" };
            bool result = AnalysisActivity.MatchDocument(content, words, MatchingStrategy.StringSearch, 0, 50);
            Assert.That(result, Is.False);
        }

        [Test()]
        public void MatchDocument_StringSearch_CaseInsensitive()
        {
            string content = "THIS IS A TEST DOCUMENT";
            IEnumerable<string> words = new[] { "test", "document" };
            bool result = AnalysisActivity.MatchDocument(content, words, MatchingStrategy.StringSearch, 0, 70);
            Assert.That(result, Is.True);
        }

        [Test()]
        public void MatchDocument_EmptyContent_ReturnsFalse()
        {
            IEnumerable<string> words = new[] { "test" };
            bool result = AnalysisActivity.MatchDocument(string.Empty, words, MatchingStrategy.StringSearch, 0, 70);
            Assert.That(result, Is.False);
        }

        [Test()]
        public void MatchDocument_NullWords_ReturnsFalse()
        {
            string content = "test content";
            bool result = AnalysisActivity.MatchDocument(content, null, MatchingStrategy.StringSearch, 0, 70);
            Assert.That(result, Is.False);
        }

        [Test()]
        public void MatchDocument_HighMatchPercentage_RequiresMoreMatches()
        {
            string content = "test document";
            IEnumerable<string> words = new[] { "test", "missing1", "missing2", "missing3" };
            bool result = AnalysisActivity.MatchDocument(content, words, MatchingStrategy.StringSearch, 0, 50);
            Assert.That(result, Is.False);
        }

        [Test()]
        public void MatchDocument_Probabilistic_ThrowsWhenThresholdZero()
        {
            string content = "test document";
            IEnumerable<string> words = new[] { "test" };
            Assert.Throws<System.InvalidOperationException>(() =>
                AnalysisActivity.MatchDocument(content, words, MatchingStrategy.Probabilistic, 0, 70));
        }

        [Test()]
        public void GetWordCountsMatchingOccurrenceParameters()
        {
            string content = "word word word test test other";
            var result = AnalysisActivity.GetWordCountsMatchingOccurrenceParameters(content, 1, 10).ToList();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test()]
        public void GetWordsMatchingOccurrenceParameters()
        {
            string content = "word word word test test other";
            var result = AnalysisActivity.GetWordsMatchingOccurrenceParameters(content, 1, 10).ToList();
            Assert.That(result, Is.Not.Null);
        }
    }
}
