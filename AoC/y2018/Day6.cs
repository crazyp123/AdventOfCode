using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2018
{
    public class Day6
    {
        private readonly List<XY> _input;
        private readonly int _width;
        private readonly int _height;

        public Day6()
        {
            _input = AdventOfCodeService.GetInput(2018, 6).AsListOf<string>().Select(s => new XY(s)).ToList();

            _width = _input.Max(p => p.X);
            _height = _input.Max(p => p.Y);

            Part1();
            Part2();
        }

        private void Part1()
        {
            var locs = new List<Location>();

            int id = 1;

            foreach (var point in _input)
            {
                int pos = 0;
                for (int x = 0; x <= _width; x++)
                {
                    for (int y = 0; y <= _height; y++)
                    {
                        var distance = ManhattanDistance(point.X, point.Y, x, y);

                        if (pos >= locs.Count)
                        {
                            locs.Add(new Location(id, distance)
                            {
                                OnEdge = x == 0 || x == _width || y == 0 || y == _height
                            });
                        }
                        else
                        {
                            locs[pos].Populate(id, distance);
                        }
                        pos++;
                    }
                }
                id++;
            }

            var infinite = locs
                .Where(loc => loc.OnEdge)
                .Select(loc => loc.Id)
                .Distinct()
                .ToList();

            var counts = locs
                .Where(loc => !loc.Neutral && !infinite.Contains(loc.Id))
                .GroupBy(loc => loc.Id)
                .ToDictionary(group => group.Key, group => group.Count());

            Utils.Utils.Answer(6, 1, counts.Values.Max());
        }

        void Part2()
        {
            var size = Enumerable.Range(0, _width)
                .SelectMany(x => Enumerable.Range(0, _height)
                    .Select(y => _input.Sum(p => ManhattanDistance(x, y, p.X, p.Y)) < 10000 ? 1 : 0))
                .Sum();

            Utils.Utils.Answer(6, 2, size);
        }

        int ManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }

    public class Location
    {
        public int Id;
        public int Distance;

        public bool Neutral = false;
        public bool OnEdge = false;

        public Location(int id, int distance)
        {
            this.Id = id;
            this.Distance = distance;
        }

        public void Populate(int newId, int newDist)
        {
            if (newDist < Distance)
            {
                Distance = newDist;
                Id = newId;
                Neutral = false;

            }
            else if (newDist == Distance)
            {
                Neutral = true;
                Distance = newDist;
                Id = 0;

            }

        }
    }

    public struct XY
    {
        public int X;
        public int Y;

        public XY(int x, int y)
        {
            X = x;
            Y = y;
        }

        public XY(string xy)
        {
            var points = xy.Split(',');
            X = int.Parse(points[0].Trim());
            Y = int.Parse(points[1].Trim());
        }
    }
}