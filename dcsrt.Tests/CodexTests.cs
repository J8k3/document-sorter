using dcsrt;
using NUnit.Framework;

namespace dcsrt.Tests
{
    [TestFixture(TestOf = typeof(Codex))]
    public class CodexTests
    {
        [Test()]
        public void Codex_Constructor_InitializesWords()
        {
            Codex codex = new Codex();
            Assert.That(codex.Words, Is.Not.Null);
        }

        [Test()]
        public void Codex_SetProperties()
        {
            Codex codex = new Codex
            {
                RuleId = "TestRule",
                Destination = "Test\\Path",
                Words = new[] { "word1", "word2", "word3" }
            };
            Assert.That(codex.RuleId, Is.EqualTo("TestRule"));
            Assert.That(codex.Destination, Is.EqualTo("Test\\Path"));
        }
    }
}
