using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AdventOfCode
{
    public static class Utils
    {
        // your logged in session cookie (taken from browser)
        public static readonly string _session;

        static Utils()
        {
            _session = File.ReadAllText("session.txt");
        }

        public static string GetInput(int year, int day)
        {
            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Cookie, $"session={_session}");
            return webClient.DownloadString($"http://adventofcode.com/{year}/day/{day}/input").Trim();
        }

        public static List<T> AsListOf<T>(this string i, char separator = '\n')
        {
            return i.Split(separator).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => (T)Convert.ChangeType(s, typeof(T))).ToList();
        }

        public static void Answer(int day, int part, object result)
        {
            Console.WriteLine($"Day {day} ({part}/2) Answer is: {result} ");
        }
    }
}
