using System.Collections.Generic;

namespace AoC.Utils;

public static class DictionaryExtensions
{
    public static void AddOrSet<T, X>(this Dictionary<T, X> dict, T key, X value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }
    }
}