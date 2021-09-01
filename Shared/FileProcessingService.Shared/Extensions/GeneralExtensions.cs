using System.Collections.Generic;
using System.Linq;

namespace FileProcessingService.Shared.Extensions
{
    public static class GeneralExtensions
    {
        public static string ToJson<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            var entries = dict.Select(d =>
                string.Format("\"{0}\": {1}", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";
        }

        
    }
}
