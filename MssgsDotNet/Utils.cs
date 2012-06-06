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

        public static bool ToBoolean(this string str)
        {
            try
            {
                return Convert.ToBoolean(str.Replace("0", "false").Replace("1", "true").Replace("null", "false"));
            }
            catch
            {
                return false;
            }
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            if (!dict.ContainsKey(key)) return default(TValue);
            else return dict[key];
        }
    }
}
