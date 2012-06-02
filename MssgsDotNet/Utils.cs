using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public static class Utils
    {
        public static bool IsFrikkinEmpty(this string str)
        {
            return String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str);
        }

        public static void AssureHas<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            if (!dict.ContainsKey(key))
                throw new Exception("Dictionary is missing key \"" + key.ToString() + "\"");
        }
    }
}
