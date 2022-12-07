using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Spectre.Console;

namespace AoC.Utils
{
    public static class Utils
    {

        public static void Run(this Day day, int part)
        {
            if (part == 1) day.Part1();
            else if (part == 2) day.Part2();
        }

        public static string Result(this Day day, int part)
        {
            return part == 1 ? day.ResultPart1 : day.ResultPart2;
        }

        public static void Answer(int day, int part, object result)
        {
            Answer(day, DateTime.Today.Year, part, result);
        }

        public static void Answer(int day, int year, int part, object result)
        {
            var answer = new Markup($"[cyan1 on grey23]{result}[/]").Centered();
            var container = new Panel(answer)
                .Header($"Part {part} Answer")
                .HeaderAlignment(Justify.Center)
                .Expand()
                .HeavyBorder()
                .BorderColor(Color.RoyalBlue1);

            AnsiConsole.Write(container);
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
            if (val == null) return;

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
        string ResultPart1 { get; }
        string ResultPart2 { get; }


        void Part1();
        void Part2();
    }

    public abstract class Day : IDay
    {
        private string _result1;
        private string _result2;
        private string _input;

        public int DayN => Utils.GetClassTypeDay(GetType());

        public int Year => Utils.GetClassTypeYear(GetType());

        public abstract object Result1();

        public abstract object Result2();

        public string Input
        {
            get
            {
                if (_input != null)
                {
                    return _input;
                }

                _input = AdventOfCodeService.GetInput(Year, DayN);
                return _input;
            }
        }

        public string ResultPart1 => _result1;

        public string ResultPart2 => _result2;

        public void Part1()
        {
            _result1 = WrapRunClipboard(Result1)?.ToString();
            Utils.Answer(DayN, Year, 1, _result1);
        }

        public void Part2()
        {
            _result2 = WrapRunClipboard(Result2)?.ToString();
            Utils.Answer(DayN, Year, 2, _result2);
        }

        private object WrapRunClipboard(Func<object> func)
        {
            object result;
            try
            {
                result = func();
                Utils.CopyToClipboard(result.ToString().Trim());
            }
            catch (NotImplementedException e)
            {
                result = e.Message;
            }

            return result;
        }
    }
}