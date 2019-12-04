using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Utils;

namespace AoC.y2018
{
    public class Day10
    {
        public List<Log> Logs;

        public Day10()
        {
            var regex = new Regex(@"(\-*\d+)");
            Logs = Utils.Utils.GetInput(2018, 10).AsListOf<string>().Select(s =>
            {
                var matches = regex.Matches(s);
                return new Log((matches[0].Value, matches[1].Value), (matches[2].Value, matches[3].Value));
            }).ToList();
        }

        public class Log
        {
            public (int, int) Position;
            public (int, int) Velocity;

            public Log((string, string) position, (string, string) velocity)
            {
                Position = (int.Parse(position.Item1.Trim()), int.Parse(position.Item2.Trim()));
                Velocity = (int.Parse(velocity.Item1.Trim()), int.Parse(velocity.Item2.Trim()));
            }

            public (int, int) Tick(int seconds)
            {
                Position.Item1 += Velocity.Item1 * seconds;
                Position.Item2 += Velocity.Item2 * seconds;

                return Position;
            }
        }
    }
}