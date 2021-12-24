using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AoC.Utils;

namespace AoC.y2021
{
    public class Day17 : Day
    {
        private int[] _xRange;
        private int[] _yRange;

        public Day17()
        {
            // test input
            var input = "target area: x=20..30, y=-10..-5";

            input = Input;

            var ranges = input.Replace("target area: ", "").Split(',', StringSplitOptions.TrimEntries);
            _xRange = ranges[0].Substring(2).Split("..").Select(s => int.Parse(s)).ToArray();
            _yRange = ranges[1].Substring(2).Split("..").Select(s => int.Parse(s)).ToArray();

        }

        public override object Result1()
        {
            var paths = new List<((int x, int y) velocity, List<(int x, int y)>)>();
            for (int x = 1; x < _xRange[1]; x++)
            {
                for (int y = 1; y < 100; y++)
                {
                    var velocity = (x, y);

                    if (TrySimulateProbe(velocity, out var path))
                    {
                        paths.Add((velocity, path));
                    }
                }
            }

            return paths.Max(t => t.Item2.Max(t2 => t2.y));
        }

        public override object Result2()
        {
            var count = 0;

            for (int x = 1; x <= _xRange[1]; x++)
            {
                for (int y = _yRange[0]; y <= 1000; y++)
                {
                    var velocity = (x, y);

                    if (TrySimulateProbe(velocity, out var path))
                    {
                        count++;
                    }
                }
            }

            return count;
        } // wrong 1876

        private bool TrySimulateProbe((int x, int y) velocity, out List<(int x, int y)> path)
        {
            var position = (x:0, y:0);

            path = new List<(int x, int y)>();

            var inTarget = false;

            var step = 0;

            while (!PassedTarget(position))
            {
                step++;

                position = (position.x + velocity.x, position.y + velocity.y);
                path.Add(position);

                velocity = (velocity.x > 0 ? velocity.x - 1 : 0, velocity.y - 1);

                inTarget = inTarget || InTarget(position);
            }

            return inTarget;
        }



        bool PassedTarget((int x, int y) pos)
        {
            return pos.x > _xRange[1] || pos.y < _yRange[0];
        }

        bool InTarget((int x, int y) pos)
        {
            return _xRange[0] <= pos.x && pos.x <= _xRange[1] && _yRange[0] <= pos.y && pos.y <= _yRange[1];
        }
    }
}