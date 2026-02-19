using dcsrt;
using NUnit.Framework;

namespace dcsrt.Tests
{
    [TestFixture(TestOf = typeof(CodexDictionary))]
    public class CodexDictionaryTests
    {
        [Test()]
        public void CodexDictionary_Constructor_InitializesDictionary()
        {
            CodexDictionary dictionary = new CodexDictionary();
            Assert.That(dictionary.Dictionary, Is.Not.Null);
        }

        [Test()]
        public void CodexDictionary_SetProperties()
        {
            CodexDictionary dictionary = new CodexDictionary
            {
                RootPath = "C:\\Test",
                Dictionary = new[]
                {
                    new Codex { RuleId = "Rule1", Destination = "Path1", Words = new[] { "word1" } },
                    new Codex { RuleId = "Rule2", Destination = "Path2", Words = new[] { "word2" } }
                }
            };
            Assert.That(dictionary.RootPath, Is.EqualTo("C:\\Test"));
        }

        [Test()]
        public void CodexDictionary_Enumerable()
        {
            CodexDictionary dictionary = new CodexDictionary
            {
                Dictionary = new[]
                {
                    new Codex { RuleId = "Rule1" },
                    new Codex { RuleId = "Rule2" },
                    new Codex { RuleId = "Rule3" }
                }
            };
            int count = 0;
            foreach (var codex in dictionary)
            {
                count++;
            }
            Assert.That(count, Is.EqualTo(3));
        }
    }
}
