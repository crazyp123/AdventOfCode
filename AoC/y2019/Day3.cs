using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;


namespace AoC.y2019
{
    public class Day3 : Day
    {
        private List<string[]> lines;
        public Day3()
        {
            lines = Input.AsListOf<string>().Select(s => s.Split(',')).ToList();
        }

        private List<List<(int, int)>> Positions()
        {
            var positions = new List<List<(int, int)>>();

            foreach (var line in lines)
            {
                (int, int) cursor = (0, 0);
                var line1 = new List<(int, int)>();
                foreach (var command in line)
                {
                    var dir = command.First().ToDirection();
                    var distance = int.Parse(command.Remove(0, 1));

                    for (int i = 0; i < distance; i++)
                    {
                        cursor = cursor.Apply(dir);
                        line1.Add(cursor);
                    }
                }
                positions.Add(line1);
            }
            return positions;
        }

        public override object Result1()
        {
            var positions = Positions();
            var intersections = positions[0].Intersect(positions[1]).ToList();
            return intersections.Select(tuple => Calculations.ManhattanDistance(0, 0, tuple.Item1, tuple.Item2)).Min();
        }

        public override object Result2()
        {
            var positions = Positions();
            var intersections = positions[0].Intersect(positions[1]).ToList();


            List<int> sums = new List<int>();

            foreach (var intersection in intersections)
            {
                var one = positions[0].IndexOf(intersection) + 1;
                var two = positions[1].IndexOf(intersection) + 1;

                sums.Add(one + two);
            }

            return sums.Min();
        }
    }
}