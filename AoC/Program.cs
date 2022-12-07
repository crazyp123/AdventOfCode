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
            var todayYear = DateTime.Today.Year;
            var todayDay = DateTime.Today.Day;

            AnsiConsole.Write(
                new Panel(new FigletText("ADVENT OF CODE")
                    .Color(Color.Green1)
                    .Centered())
                    .BorderColor(Color.Green1)
            );

            AnsiConsole.Write(new Markup($"year: [invert]{todayYear}[/]").Centered());
            AnsiConsole.Write(new Markup($"day: [green]{todayDay:D2}[/]\n").Centered());


            var choices = new[] { "Run part 1", "Run part 2", "Show today's puzzle", "Other day" }.ToList();
            var cmd = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(choices));

            switch (choices.IndexOf(cmd))
            {
                case 0:
                    await Run(todayYear, todayDay);
                    break;
                case 1:
                    await Run(todayYear, todayDay, 2);
                    break;
                case 2:
                    ShowProblem(todayYear, todayDay, 1);
                    break;
                case 3:
                    var (day, year, part) = PromptDayToRun();
                    var otherDay =
                        AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "Run", "Show puzzle" }));
                    if (otherDay == "Run")
                    {
                        await Run(year, day, part ?? 0);
                    }
                    else
                    {
                        ShowProblem(year, day, part ?? 1);
                    }
                    break;
            }
        }

        static async Task Run(int year, int day, int part = 0)
        {
            var instance = GetDayImplementation(day, year);

            switch (part)
            {
                case 1:
                case 2:
                    instance.Run(part);
                    PromptPostAnswer(day, year, part, instance.Result(part));
                    break;
                default:
                    instance.Part1();
                    var correct = PromptPostAnswer(day, year, 1, instance?.ResultPart1);
                    if (correct)
                    {
                        ShowProblem(year, day, 2);
                    }
                    AnsiConsole.WriteLine();

                    instance.Part2();
                    PromptPostAnswer(day, year, part, instance.Result(2));
                    break;
            }
        }

        private static Day GetDayImplementation(int day, int year)
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

            var instance = (Day)Activator.CreateInstance(dayType);
            return instance;
        }

        static bool PromptPostAnswer(int day, int year, int part, string value, bool submit = false)
        {
            if (string.IsNullOrEmpty(value) || value == "The method or operation is not implemented.") return false;

            AnsiConsole.MarkupLine($"Press [yellow]SPACE[/] to Submit or [yellow]ENTER[/] to Continue");

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

                AnsiConsole.Write(new Text(resultTxt, new Style(Color.Yellow3_1)));
                return isCorrect;
            }

            return false;
        }

        static (int, int, int?) PromptDayToRun()
        {
            var day = DateTime.Today.Day;
            var year = DateTime.Today.Year;

            //var runToday = AnsiConsole.Confirm("Run current day?");
            //if (runToday)
            //{
            //    return (day, year, null);
            //}

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
                    AdventOfCodeService.GetProblemHtml(year, day, part)?.Print();
                });
        }
    }
}