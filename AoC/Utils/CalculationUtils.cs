using System;
using System.Collections.Generic;

namespace AoC.Utils
{
    public static class Calculations
    {
        public static int ManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public static int ManhattanDistance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static IEnumerable<(T, int)> FindAdjacentItems<T>(this IEnumerable<T> list)
        {
            T previous = default(T);
            int count = 0;
            foreach (var item in list)
            {
                if (item.Equals(previous))
                {
                    count++;
                }
                else
                {
                    if (count > 1)
                    {
                        yield return (previous, count);
                    }
                    count = 1;
                }
                previous = item;
            }

            if (count > 1)
            {
                yield return (previous, count);
            }
        }
    }
}