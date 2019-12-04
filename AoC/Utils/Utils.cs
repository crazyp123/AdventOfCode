using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AoC.Utils
{
    public static class Utils
    {
        // your logged in session cookie (taken from browser)
        private static readonly string _session;

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
            return i.Split(separator).Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => (T)Convert.ChangeType(s, typeof(T))).ToList();
        }

        public static void Answer(int day, int part, object result)
        {
            Console.WriteLine($"Day {day} ({part}/2) Answer is: {result} ");
        }
    }

    public static class Calculations
    {
        public static int ManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }

    public interface IDay
    {
        void Part1();
        void Part2();
    }

    public abstract class Day : IDay
    {
        public int DayN => int.Parse(GetType().Name.Remove(0, 3));

        public int Year => int.Parse(GetType().Namespace.Remove(0, 5));

        public abstract object Result1();

        public abstract object Result2();

        public string Input => Utils.GetInput(Year, DayN);

        public void Part1()
        {
            Utils.Answer(DayN, 1, Result1());
        }

        public void Part2()
        {
            Utils.Answer(DayN, 2, Result2());
        }

    }
}
