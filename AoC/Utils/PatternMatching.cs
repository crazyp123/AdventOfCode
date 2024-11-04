using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Utils
{
    public static class PatternMatching
    {
        private readonly static Dictionary<string, string> regexMap = new Dictionary<string, string>
        {
            {"n", "([0-9]+)"},  //number
            {"c", "(\\w)"},     //character
            {"s", "(\\w+)"}     //string
        };

        /// <summary>
        /// Returns parsed values based on pattern, using 'n' for numbers and 's' for strings. Eg 'n-n s: s' will parse '1-3 a: abdce'.
        /// 'n': integer
        /// 's': string
        /// 'c': character
        /// </summary>
        public static object[] ParseFromPattern(string input, string pattern)
        {
            //pattern: "n-n c: s"
            //input: "1-3 a: abcde"

            var expression = regexMap.Aggregate(pattern, (s, pair) => s.Replace(pair.Key, pair.Value));

            var match = new Regex(expression, RegexOptions.IgnoreCase).Match(input);
            if (!match.Success)
            {
                throw new Exception($"Failed to match regex! word: {input} regex: {expression} ");
            }

            var typePositions = pattern.Where(c => regexMap.Keys.Contains(c.ToString())).Select(c => c).ToArray();

            var result = new List<object>();
            for (var i = 1; i < match.Groups.Count; i++)
            {
                Group matchGroup = match.Groups[i];

                object value;
                switch (typePositions[i - 1])
                {
                    case 'n':
                        value = int.Parse(matchGroup.Value);
                        break;
                    case 'c':
                        value = matchGroup.Value[0];
                        break;
                    case 's':
                        value = matchGroup.Value;
                        break;
                    default:
                        value = matchGroup.Value;
                        break;
                }
                result.Add(value);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returns parsed values based on pattern, using 'n' for numbers and 's' for strings. Eg 'n-n s: s' will parse '1-3 a: abdce'. 
        /// </summary>
        public static object[] ParsePattern(this string i, string pattern)
        {
            return ParseFromPattern(i, pattern);
        }

        public static List<object[]> AsListOfPatterns(this string i, string pattern, char separator = '\n')
        {
            return i.Split(separator).Where(s => !String.IsNullOrWhiteSpace(s))
                .Select<string, object[]>(s => ParseFromPattern(s, pattern))
                .ToList();
        }

        public static List<(A, B, C, D)> AsListOfPatterns<A, B, C, D>(this string i, string pattern, char separator = '\n')
        {
            return i.Split(separator).Where(s => !String.IsNullOrWhiteSpace(s))
                .Select(s => ParsePattern<A, B, C, D>(s, pattern))
                .ToList();
        }

        public static List<(A, B, C)> AsListOfPatterns<A, B, C>(this string i, string pattern, string separator = "\n")
        {
            return i.Split(separator).Where(s => !String.IsNullOrWhiteSpace(s))
                .Select(s => ParsePattern<A, B, C>(s, pattern))
                .ToList();
        }

        public static List<(A, B)> AsListOfPatterns<A, B>(this string i, string pattern, string separator = "\n")
        {
            return i.Split(separator).Where(s => !String.IsNullOrWhiteSpace(s))
                .Select(s => ParsePattern<A, B>(s, pattern))
                .ToList();
        }

        public static (A, B) ParsePattern<A, B>(this string i, string pattern)
        {
            var o = ParseFromPattern(i, pattern);
            return ((A)o[0], (B)o[1]);
        }

        public static (A, B, C) ParsePattern<A, B, C>(this string i, string pattern)
        {
            var o = ParseFromPattern(i, pattern);
            return ((A)o[0], (B)o[1], (C)o[2]);
        }

        public static (A, B, C, D) ParsePattern<A, B, C, D>(this string i, string pattern)
        {
            var o = ParseFromPattern(i, pattern);
            return ((A)o[0], (B)o[1], (C)o[2], (D)o[3]);
        }

        public static (A, B, C, D, E) ParsePattern<A, B, C, D, E>(this string i, string pattern)
        {
            var o = ParseFromPattern(i, pattern);
            return ((A)o[0], (B)o[1], (C)o[2], (D)o[3], (E)o[4]);
        }

        public static (A, B, C, D, E, F) ParsePattern<A, B, C, D, E, F>(this string i, string pattern)
        {
            var o = ParseFromPattern(i, pattern);
            return ((A)o[0], (B)o[1], (C)o[2], (D)o[3], (E)o[4], (F)o[5]);
        }
    }
}