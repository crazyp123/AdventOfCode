using System;
using System.Linq;
using System.Threading.Tasks;
using AoC.Utils;
using Spectre.Console;

namespace AoC
{

    /// QuickGraph cheatsheet: https://gist.github.com/Jbat1Jumper/95c77d216981e13952cf7f22e653d80d

    class Program
    {
        static async Task Main()
        {
            AnsiConsole.Write(
                new Panel(new FigletText("ADVENT OF CODE")
                    .Color(Color.Green1)
                    .Centered())
                    .BorderColor(Color.Green1)
            );

            AnsiConsole.Write(new Markup($"year: [invert]{DateTime.Today.Year}[/]").Centered());
            AnsiConsole.Write(new Markup($"day: [green]{DateTime.Today.Day:D2}[/]\n").Centered());

            var (day, year, part) = PromptDayToRun();

            var instance = GetDayImplementation(day, year);

            switch (part)
            {
                case 1:
                    instance?.Part1();
                    PromptPostAnswer(day, year, 1, instance?.Result1);
                    break;
                case 2:
                    instance?.Part2();
                    PromptPostAnswer(day, year, 2, instance?.Result2);
                    break;
                default:
                    instance?.Part1();
                    var correct = PromptPostAnswer(day, year, 1, instance?.Result1);
                    if (correct)
                    {
                        ShowProblem(year, day, 2);
                    }
                    Console.WriteLine();

                    instance?.Part2();
                    PromptPostAnswer(day, year, 2, instance?.Result2);
                    break;
            }
        }

        private static IDay GetDayImplementation(int day, int year)
        {
            var dayType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(Day).IsAssignableFrom(p))
                .FirstOrDefault(t =>
                    Utils.Utils.GetClassTypeDay(t) == day && Utils.Utils.GetClassTypeYear(t) == year);

            if (dayType == null)
            {
                AnsiConsole.MarkupLine($"[red]y{year}.Day{day} not found![/]");
                return null;
            }

            var instance = (IDay)Activator.CreateInstance(dayType);
            return instance;
        }

        static bool PromptPostAnswer(int day, int year, int part, string value, bool submit = false)
        {
            if (string.IsNullOrEmpty(value) || value == "The method or operation is not implemented.") return false;

            AnsiConsole.MarkupLine($"\nPress [yellow]SPACE[/] to Submit the answer, [yellow]ENTER[/] to Continue");

            if (submit || Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                var resultTxt = AdventOfCodeService.PostAnswer(year, day, part, value);
                if (resultTxt.Contains("Did you already complete it?"))
                {
                    AnsiConsole.MarkupLine("[green]Already complete![/]");
                    return true;
                }
                var isCorrect = resultTxt.Contains("That's the right answer!");

                AnsiConsole.MarkupLine("\nThe answer is:");
                AnsiConsole.MarkupLine(isCorrect ? "[green]*** CORRECT :) ***[/]" : "[red]*** WRONG :( ***[/]");
                Console.WriteLine();

                AnsiConsole.MarkupLine($"[yellow]{resultTxt}[/]");
                return isCorrect;
            }

            return false;
        }

        static (int, int, int?) PromptDayToRun()
        {
            var day = DateTime.Today.Day;
            var year = DateTime.Today.Year;

            var runToday = AnsiConsole.Confirm("Run current day?");
            if (runToday)
            {
                return (day, year, null);
            }

            year = AnsiConsole.Ask<int>("Year", year);
            day = AnsiConsole.Ask("Day", day);
            var part = AnsiConsole.Ask("Part", 1);

            if (day == -1 || year == -1)
            {
                AnsiConsole.MarkupLine("[red]wrong input[/]");
                return PromptDayToRun();
            }

            return (day, year, part);
        }

        private static void ShowProblem(int year, int day, int part)
        {
            AnsiConsole.Status()
                .Spinner(Spinner.Known.SimpleDotsScrolling)
                .Start($"loading day {day} part {part}", ctx =>
                {
                    var problem = AdventOfCodeService.GetProblem(year, day, part);
                    var content = new Text(string.Join("\n", problem.Skip(1)));
                    var panel = new Panel(content)
                        .BorderColor(Color.SteelBlue)
                        .Header($"Day {day} Part {part}")
                        .HeaderAlignment(Justify.Center)
                        .PadTop(2)
                        .PadLeft(2)
                        .PadRight(2);
                    AnsiConsole.Write(panel);
                });
        }
    }
}