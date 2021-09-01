using FileProcessingService.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileProcessingService.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string[] AsCleanedArray(this string input)
        {
            string[] elementArrays = input
               .Trim()
               .Replace(" ", string.Empty)
               .ToLowerInvariant()
               .Split(';');

            return elementArrays;
        }

        public static string[] ExtractWords(this string input, char separator)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Array.Empty<string>();

            return input.Sanitize().Split(separator);
        }

        public static string Sanitize(this string input)
        {
            return Regex.Replace(input, @"[^\w\s]", "");
        }

        public static Dictionary<string, int> FindDuplicates(this string input)
        {
            string[] words = input.ExtractWords(SharedConstants.Whitespace);

            Dictionary<string, int> repeatedWordCount = new();
            for (int i = 0; i < words.Length; i++)
            {
                string key = words[i].ToLowerInvariant();
                if (repeatedWordCount.ContainsKey(key))
                {
                    int value = repeatedWordCount[key];
                    repeatedWordCount[key] = value + 1;
                }
                else
                {
                    repeatedWordCount.Add(key, 1);
                }
            }

            foreach (var item in repeatedWordCount)
            {
                if (item.Value < 2)
                    repeatedWordCount.Remove(item.Key);
            }

            return repeatedWordCount;
        }

        public static int GetCountForWord(this Dictionary<string, int> duplicates, string keyword)
        {
            if (duplicates.ContainsKey(keyword))
                return duplicates[keyword];

            return 0;
        }
    }
}
