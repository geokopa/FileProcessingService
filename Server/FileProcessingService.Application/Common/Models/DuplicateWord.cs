using System;

namespace FileProcessingService.Application.Common.Models
{
    public class DuplicateWord
    {
        public string Word { get; set; }
        public int Count { get; set; }

        public DuplicateWord(string word, int count)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentNullException(nameof(word), $"{nameof(word)} argument must have a value");

            Word = word;
            Count = count;
        }
    }
}
