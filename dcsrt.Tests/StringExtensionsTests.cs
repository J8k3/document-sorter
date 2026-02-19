using NUnit.Framework;
using System.Linq;

namespace dcsrt.Tests
{
    [TestFixture(TestOf = typeof(StringExtensions))]
    public class StringExtensionsTests
    {
        [TestCase("this String HAS somE random CASing.")]
        [TestCase("THIS STRING HAS SOME RANDOM CASING.")]
        [TestCase("this string has some random casing.")]
        public void CamelCaseTest(string input)
        {
            string result = input.CamelCase();
            Assert.That(result, Is.EqualTo("This String Has Some Random Casing."));
        }

        [TestCase("this   string                    has lots   of duplicate     white space.")]
        [TestCase("this string has lots of duplicate white space.")]
        public void RemoveDuplicateWhitespace(string input)
        {
            string result = input.RemoveDuplicateWhitespace();
            Assert.That(result, Is.EqualTo("this string has lots of duplicate white space."));
        }

        [TestCase(@"`~1!2@3#4$5%6^7&8*9(0)-_=+qwertyuiop[{]}\|asdfghjkl;:""'zxcvbnm,<.>/?")]
        [TestCase(@""" !""#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~")]
        public void RemoveAllNonAlphaNumericCharactersExceptHyphenCharacters(string input)
        {
            string result = input.RemoveAllNonAlphaNumericCharactersExceptHyphenCharacters();
            Assert.That(result, Is.EqualTo("1 2 3 4 5 6 7 8 9 0 - qwertyuiop asdfghjkl zxcvbnm"));
        }

        [Test()]
        public void RemoveExcludeWordsTest()
        {
            string value = "This is the best string i've ever seen in my very alliteratively and long life.";
            string result = value.RemoveExcludeWordsToLowerInvariant();
            Assert.That(result, Is.EqualTo("string alliteratively life."));
        }

        [Test()]
        public void CamelCase_EmptyString_ReturnsEmpty()
        {
            string actual = string.Empty.CamelCase();
            Assert.That(actual, Is.EqualTo(string.Empty));
        }

        [Test()]
        public void RemoveDuplicateWhitespace_MultipleTypes()
        {
            string input = "hello\t\t  world\n\ntest";
            string expected = "hello world test";
            string actual = input.RemoveDuplicateWhitespace();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test()]
        public void RemoveAllNonAlphaNumericCharacters()
        {
            string input = "hello-world!@#123";
            string expected = "hello world   123";
            string actual = input.RemoveAllNonAlphaNumericCharacters();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test()]
        public void ReplaceLineBreaksWithSpace()
        {
            string input = "line1\r\nline2\nline3\rline4";
            string actual = input.ReplaceLineBreaksWithSpace();
            Assert.That(actual, Does.Not.Contain("\r"));
            Assert.That(actual, Does.Not.Contain("\n"));
            Assert.That(actual, Does.Contain(" "));
        }

        [Test()]
        public void TokenizeToLowerInvariant()
        {
            string input = "Hello World Test";
            var tokens = input.TokenizeToLowerInvariant().ToList();
            Assert.That(tokens.Count, Is.EqualTo(3));
            Assert.That(tokens[0], Is.EqualTo("hello"));
            Assert.That(tokens[1], Is.EqualTo("world"));
            Assert.That(tokens[2], Is.EqualTo("test"));
        }

        [Test()]
        public void TokenizeDistinctToLowerInvarient()
        {
            string input = "Hello world HELLO test World";
            var tokens = input.TokenizeDistinctToLowerInvarient().ToList();
            Assert.That(tokens.Count, Is.EqualTo(3));
            Assert.That(tokens, Does.Contain("hello"));
            Assert.That(tokens, Does.Contain("world"));
            Assert.That(tokens, Does.Contain("test"));
        }

        [Test()]
        public void StemToLowerInvariant()
        {
            string input = "running";
            string actual = input.StemToLowerInvariant();
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Length, Is.LessThanOrEqualTo(input.Length));
        }

        [Test()]
        public void RemoveMatchingCharacters()
        {
            string input = "abc123def456";
            string actual = input.RemoveMatchingCharacters("[0-9]", "X");
            Assert.That(actual, Is.EqualTo("abcXXXdefXXX"));
        }
    }
}
