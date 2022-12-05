﻿using System;
using System.Diagnostics;
using Spectre.Console;

namespace AoC.Utils
{
    public static class Utils
    {
        public static void Answer(int day, int part, object result)
        {
            Console.WriteLine($"Day {day} ({part}/2) Answer is:");
            AnsiConsole.MarkupLine($"[invert]{result}[/]");
        }

        public static void Answer(int day, int year, int part, object result)
        {
            AnsiConsole.MarkupLine($"Day {day} - {year} ({part}/2) Answer is:");
            AnsiConsole.MarkupLine($"[bold yellow]{result}[/]");
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
        string Result1 { get; }
        string Result2 { get; }

        void Part1();
        void Part2();
    }

    public abstract class Day : IDay
    {
        private string _result1;
        private string _result2;

        public int DayN => Utils.GetClassTypeDay(GetType());

        public int Year => Utils.GetClassTypeYear(GetType());

        public abstract object Result1();

        public abstract object Result2();

        public string Input => AdventOfCodeService.GetInput(Year, DayN);

        string IDay.Result1 => _result1;

        string IDay.Result2 => _result2;

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