using FileProcessingService.Application.Common.Models;
using System.Collections.Generic;

namespace FileProcessingService.Application.Common.Extensions
{
    public static class ModelConverters
    {
        public static IEnumerable<DuplicateWord> ToDuplicateWordModel(this Dictionary<string, int> dictionary)
        {
            foreach (var item in dictionary)
            {
                yield return new DuplicateWord(item.Key, item.Value);
            }
        }
    }
}
