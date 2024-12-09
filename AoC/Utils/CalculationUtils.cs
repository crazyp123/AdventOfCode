using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils;

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
        return value < min ? min : value > max ? max : value;
    }

    public static IEnumerable<(T, int)> FindAdjacentItems<T>(this IEnumerable<T> list)
    {
        var previous = default(T);
        var count = 0;
        foreach (var item in list)
        {
            if (item.Equals(previous))
            {
                count++;
            }
            else
            {
                if (count > 1) yield return (previous, count);
                count = 1;
            }

            previous = item;
        }

        if (count > 1) yield return (previous, count);
    }

    public static IEnumerable<(T value, int count, int index)> FindAdjacentItems<T>(this List<T> items)
    {
        var previous = default(T);
        var count = 1;
        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item.Equals(previous))
            {
                count++;
            }
            else if (i > 0)
            {
                yield return (previous, count, i - count);
                count = 1;
            }

            previous = item;
        }

        yield return (previous, count, items.Count - count);
    }
}