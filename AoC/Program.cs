using System;
using System.Linq;
using System.Threading.Tasks;
using AoC.Utils;
using AoC.y2019;
using AoC.y2020;

namespace AoC
{
    class Program
    {
        static async Task Main()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("----------------------");
            Console.WriteLine("--- ADVENT OF CODE ---");
            Console.WriteLine("----------------------");
            Console.ResetColor();

            Console.WriteLine($"today is: {DateTime.Today.Day:D2}/{DateTime.Today.Year}");

            var type = typeof(Day);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .ToList();

            int? partToRun = null;

            Type dayType = null;
            do
            {
                var (day, year, part) = PromptDayToRun();
                
                dayType = types.FirstOrDefault(t => Utils.Utils.GetClassTypeDay(t) == day && Utils.Utils.GetClassTypeYear(t) == year);
                partToRun = part;

                Console.Write($"day {day:D2}/{year}: ");
                Console.ForegroundColor = dayType is null ? ConsoleColor.Red : ConsoleColor.Green;
                Console.WriteLine(dayType is null ? "not found, try again" : "OK");
                Console.ResetColor();

            } while (dayType is null);

            Console.WriteLine();

            var instance = (IDay)Activator.CreateInstance(dayType);

            Console.ForegroundColor = ConsoleColor.Blue;
            switch (partToRun)
            {
                case 1:
                    instance?.Part1();
                    break;
                case 2:
                    instance?.Part2();
                    break;
                default:
                    instance?.Part1();
                    instance?.Part2();
                    break;
            }
            Console.ResetColor();
        }

        static (int, int, int?) PromptDayToRun()
        {
            var day = DateTime.Today.Day;
            var year = DateTime.Today.Year;

            Console.WriteLine($"enter day and year (default {year}) to run, or press ENTER to run current day:");

            Console.ForegroundColor = ConsoleColor.Yellow;
            var dayToRun = Console.ReadLine();
            Console.ResetColor();

            int? part = null;

            if (!string.IsNullOrWhiteSpace(dayToRun))
            {
                var split = dayToRun.Split(' ');

                if (split[0].StartsWith("p"))
                {
                    part = int.TryParse(split[0].Substring(1), out var p) ? p : part;
                    day = int.TryParse(split[1], out var d) ? d : -1;
                    if (split.Length == 3) year = int.TryParse(split[2], out var y) ? y : -1;
                }
                else
                {
                    day = int.TryParse(split[0], out var d) ? d : -1;
                    if (split.Length == 2) year = int.TryParse(split[1], out var y) ? y : -1;
                }
            }

            if (day == -1 || year == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("wrong input");
                Console.ResetColor();
                return PromptDayToRun();
            }

            return (day, year, part);
        }

    }
}
