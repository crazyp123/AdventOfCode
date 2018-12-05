using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2018
{
    public class Day4
    {
        private List<ShiftLog> _logs;
        private Dictionary<int, List<int>> _guardSleepMinutes;

        public Day4()
        {
            var input = Utils.GetInput(2018, 4).AsListOf<string>();

            _logs = input.Select(ParseLog).OrderBy(log => log.Time).ToList();

            int prevId = 0;
            foreach (var log in _logs)
            {
                if (log.Status == LogType.BeginsShift)
                {
                    prevId = log.GuardId;
                    continue;
                }

                log.GuardId = prevId;
            }

            _guardSleepMinutes = new Dictionary<int, List<int>>();

            var lastAsleep = new DateTime();

            foreach (var log in _logs)
            {
                if (log.Status == LogType.BeginsShift)
                {
                    if (!_guardSleepMinutes.ContainsKey(log.GuardId))
                        _guardSleepMinutes.Add(log.GuardId, new List<int>());
                }
                else if (log.Status == LogType.FallsAsleep)
                {
                    lastAsleep = log.Time;
                }
                else if (log.Status == LogType.WakesUp)
                {
                    var mins = log.Time.Subtract(lastAsleep).TotalMinutes;
                    for (int i = 0; i < mins; i++)
                    {
                        _guardSleepMinutes[log.GuardId].Add(lastAsleep.AddMinutes(i).Minute);
                    }
                }
            }

            Part1();
            Part2();
        }

        void Part1()
        {
            var mostAsleep = _guardSleepMinutes.OrderByDescending(mins => mins.Value.Count).First();

            var moreFrequentMin = mostAsleep.Value.GroupBy(min => min).OrderByDescending(mins => mins.Count()).First().Key;

            Console.WriteLine($"Day 4 (1/2) Answer is: {mostAsleep.Key * moreFrequentMin}");
        }

        void Part2()
        {
            var mostAsleepAtSameMin = _guardSleepMinutes
                .OrderByDescending(mins => mins.Value.Count == 0 ? 0 : mins.Value.GroupBy(min => min).Max(g => g.Count()))
                .FirstOrDefault();

            var moreFrequentMin = mostAsleepAtSameMin.Value.GroupBy(i => i).OrderByDescending(ints => ints.Count()).First().Key;

            Console.WriteLine($"Day 4 (2/2) Answer is: {mostAsleepAtSameMin.Key * moreFrequentMin}");
        }

        ShiftLog ParseLog(string s)
        {
            var date = new Regex(@"(\[(.*?)\])").Match(s).Value;
            var id = new Regex(@"(\#\d+)").Match(s);

            var time = DateTime.Parse(date.Substring(1, date.Length - 2));

            return new ShiftLog
            {
                Time = time,
                GuardId = id.Success ? int.Parse(id.Value.TrimStart('#')) : -1,
                Status = s.Contains("begins") ? LogType.BeginsShift : s.Contains("wakes") ? LogType.WakesUp : LogType.FallsAsleep
            };
        }
    }

    public class ShiftLog
    {
        public DateTime Time;
        public int GuardId;
        public LogType Status;

    }

    public enum LogType
    {   
        WakesUp,
        BeginsShift,
        FallsAsleep
    }
}
