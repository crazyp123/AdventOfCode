using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            var webClient = new HttpClient();
            var url = $"https://adventofcode.com/{year}/day/{day}/input";

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
            };

            request.Headers.Add("cookie", $"session={_session}");
            var reponse = webClient.Send(request);

            if (reponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"input request returned: {reponse.StatusCode} - {reponse.ReasonPhrase}");
            }

            return reponse.Content.ReadAsStringAsync().Result.Trim();
        }

        public static List<T> AsListOf<T>(this string i, string separator = "\n",
            StringSplitOptions options = StringSplitOptions.None)
        {
            return i.Split(separator, options).Select(s => (T)Convert.ChangeType(s, typeof(T))).ToList();
        }

        public static void Answer(int day, int part, object result)
        {
            Console.WriteLine($"Day {day} ({part}/2) Answer is:\n{result}");
        }

        public static void Answer(int day, int year, int part, object result)
        {
            Console.WriteLine($"Day {day} - {year} ({part}/2) Answer is:\n{result}");
        }

        public static int GetClassTypeDay(Type t)
        {
            return int.TryParse(t.Name.Remove(0, 3), out var d) ? d : 0;
        }

        public static int GetClassTypeYear(Type t)
        {
            return int.TryParse(t.Namespace.Remove(0, 5), out var d) ? d : 0;
        }

        public static void CopyToClipboard(string val)
        {
            if (OperatingSystem.IsWindows())
            {
                $"echo | set /p={val}|clip".Bat();
            }
        }

        public static string Bat(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            string result = Run("cmd.exe", $"/c \"{escapedArgs}\"");
            return result;
        }

        private static string Run(string filename, string arguments)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }

    public interface IDay
    {
        void Part1();
        void Part2();
    }

    public abstract class Day : IDay
    {
        public int DayN => Utils.GetClassTypeDay(GetType());

        public int Year => Utils.GetClassTypeYear(GetType());

        public abstract object Result1();

        public abstract object Result2();

        public string Input => Utils.GetInput(Year, DayN);

        public void Part1()
        {
            Utils.Answer(DayN, Year, 1, WrapRunClipboard(Result1));
        }

        public void Part2()
        {
            Utils.Answer(DayN, Year, 2, WrapRunClipboard(Result2));
        }

        private object WrapRunClipboard(Func<object> func)
        {
            object result;
            try
            {
                result = func();
                Utils.CopyToClipboard(result?.ToString().Trim());
            }
            catch (NotImplementedException e)
            {
                result = e.Message;
            }

            return result;
        }
    }
}