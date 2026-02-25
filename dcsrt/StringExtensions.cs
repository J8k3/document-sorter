using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace dcsrt
{
    public static class StringExtensions
    {
        private static readonly Lazy<HashSet<string>> StopWords = new Lazy<HashSet<string>>(() =>
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stopwords.txt");
            if (File.Exists(path))
            {
                return new HashSet<string>(File.ReadAllLines(path), StringComparer.OrdinalIgnoreCase);
            }
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        });

        private static readonly Lazy<Annytab.Stemmer.EnglishStemmer> Stemmer = new Lazy<Annytab.Stemmer.EnglishStemmer>(() => new Annytab.Stemmer.EnglishStemmer());

        private static readonly Regex WhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

        public static string StemToLowerInvariant(this string content)
        {
            string result = content;
            if (!string.IsNullOrEmpty(content))
            {
                result = Stemmer.Value.GetSteamWord(result);
            }
            return result;
        }

        public static IEnumerable<string> TokenizeToLowerInvariant(this string content, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            IEnumerable<string> result = null;

            if (!string.IsNullOrEmpty(content))
            {
                result = content.Split(new char[] { ' ' }, options).Select(s => s.ToLowerInvariant());
            }

            return result;
        }

        public static IEnumerable<string> TokenizeDistinctToLowerInvarient(this string content)
        {
            return content.TokenizeToLowerInvariant().Distinct();
        }

        public static string ReplaceLineBreaksWithSpace(this string content)
        {
            string result = content;

            using (MiniProfiler.Current.Step("Removing Line Breaks"))
            {
                if (!string.IsNullOrEmpty(content))
                {
                    string lineSeparator = ((char)0x2028).ToString();
                    string paragraphSeparator = ((char)0x2029).ToString();

                    result = result.Replace("\r\n", " ");
                    result = result.Replace("\n", " ");
                    result = result.Replace("\r", " ");
                    result = result.Replace(lineSeparator, " ");
                    result = result.Replace(paragraphSeparator, " ");
                }
            }

            return result;
        }

        public static string RemoveStopWords(this string content)
        {
            string result = content;

            if (!string.IsNullOrEmpty(content))
            {
                using (MiniProfiler.Current.Step("Removing Stop Words"))
                {
                    string[] words = content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> filtered = new List<string>(words.Length);

                    foreach (string word in words)
                    {
                        if (!StopWords.Value.Contains(word) && !IsNumericOnly(word))
                        {
                            filtered.Add(word);
                        }
                    }

                    result = string.Join(" ", filtered);
                }
            }

            return result;
        }

        private static bool IsNumericOnly(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            if (word.Length >= 4)
            {
                return false;
            }
            foreach (char c in word)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static string CamelCase(this string content)
        {
            string result = content;

            using (MiniProfiler.Current.Step("Camel Casing"))
            {
                if (!string.IsNullOrEmpty(content))
                {
                    result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(content.ToLowerInvariant());
                }
            }

            return result;
        }

        public static string RemoveDuplicateWhitespace(this string content)
        {
            string result = content;

            using (MiniProfiler.Current.Step("Removing Duplicate White Scape"))
            {
                if (!string.IsNullOrEmpty(content))
                {
                    result = WhitespaceRegex.Replace(content, " ");
                }
            }

            return result;
        }

        public static string RemoveAllNonAlphaNumericCharacters(this string content, string replacementCharacter = " ")
        {
            string result = content;
            if (!string.IsNullOrEmpty(content))
            {
                result = content.RemoveMatchingCharacters("[^a-zA-Z0-9 ]", replacementCharacter);
            }
            return result;
        }

        public static string RemoveAllNonAlphaNumericCharactersExceptHyphenCharacters(this string content, string replacementCharacter = " ")
        {
            string result = content;
            if (!string.IsNullOrEmpty(content))
            {
                result = content.RemoveMatchingCharacters("[^a-zA-Z0-9 -]", replacementCharacter).RemoveDuplicateWhitespace().Trim();
            }
            return result;
        }

        public static string RemoveMatchingCharacters(this string content, string regexPattern, string replacementCharacter = " ")
        {
            string result = content;
            using (MiniProfiler.Current.Step("Replacing Matching Characters"))
            {
                if (!string.IsNullOrEmpty(content))
                {
                    result = Regex.Replace(content, regexPattern, replacementCharacter, RegexOptions.IgnoreCase & RegexOptions.CultureInvariant);
                }
            }
            return result;
        }
    }
}
