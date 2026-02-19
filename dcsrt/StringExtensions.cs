using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace dcsrt
{
    public static class StringExtensions
    {
        public static string StemToLowerInvariant(this string content)
        {
            string result = content;
            if (!string.IsNullOrEmpty(content))
            {
                Annytab.Stemmer.EnglishStemmer stemmer = new Annytab.Stemmer.EnglishStemmer();

                result = stemmer.GetSteamWord(result);
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

        public static string RemoveExcludeWordsToLowerInvariant(this string content)
        {
            string result = content;

            if (!string.IsNullOrEmpty(content))
            {
                result = result.ToLowerInvariant();

                var stopWords = Properties.Settings.Default.StopWords;

                using (MiniProfiler.Current.Step("Removing Stop Words"))
                {
                    foreach (var stopWord in stopWords)
                    {
                        if (string.IsNullOrWhiteSpace(stopWord))
                        {
                            continue;
                        }

                        string escapedStopWord = Regex.Escape(stopWord.ToLowerInvariant());
                        result = Regex.Replace(result, $@"\b{escapedStopWord}\b", " ", RegexOptions.CultureInvariant);
                    }
                }

                result = result.RemoveDuplicateWhitespace().Trim();
            }

            return result;
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
                    result = Regex.Replace(content, @"\s+", " ");
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
                    result = Regex.Replace(content, regexPattern, replacementCharacter);
                }
            }
            return result;
        }
    }
}
