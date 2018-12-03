using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AdventOfCode
{
    public static class Utils
    {
        public static string GetInput(int year, int day)
        {
            // your logged in session cookie (taken from browser)
            string session = @"53616c7465645f5f27c3b4c33978ee985ca1ef0c157c14da6117e9b317a9d96812e79bf7a9ebc16da065e5714e40db0b";

            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Cookie, $"session={session}");
            return webClient.DownloadString($"http://adventofcode.com/{year}/day/{day}/input");
        }

        public static List<T> AsListOf<T>(this string i, char separator = '\n')
        {
            return i.Split(separator).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s=> (T)Convert.ChangeType(s, typeof(T))).ToList();
        }
    }
}
