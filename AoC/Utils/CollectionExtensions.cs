using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils;

public static class CollectionExtensions
{
    private static readonly Random rng = new();

    /// <summary>
    ///     Applies a function on each element and returns the modified list
    /// </summary>
    public static List<T> Apply<T>(this IEnumerable<T> list, Action<T> apply)
    {
        var enumerable = list.ToList();
        foreach (var x in enumerable) apply(x);
        return enumerable;
    }

    public static Grid<T> ToGrid<T>(this List<List<T>> list)
    {
        return new Grid<T>(list);
    }

    public static IEnumerable<T[]> Permutations<T>(this IEnumerable<T> source)
    {
        var sourceArray = source.ToArray();
        var results = new List<T[]>();
        Permute(sourceArray, 0, sourceArray.Length - 1, results);
        return results;
    }

    private static void Permute<T>(T[] elements, int recursionDepth, int maxDepth, ICollection<T[]> results)
    {
        if (recursionDepth == maxDepth)
        {
            results.Add(elements.ToArray());
            return;
        }

        for (var i = recursionDepth; i <= maxDepth; i++)
        {
            Swap(ref elements[recursionDepth], ref elements[i]);
            Permute(elements, recursionDepth + 1, maxDepth, results);
            Swap(ref elements[recursionDepth], ref elements[i]);
        }
    }

    private static void Swap<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    /// <summary>
    ///     Splits a list in groups of equal parts of size <see cref="groupSize" />
    /// </summary>
    /// <returns></returns>
    public static List<List<T>> SplitInEqualGroups<T>(this List<T> list, int groupSize)
    {
        var groups = new List<List<T>>();
        var next = new List<T>();
        foreach (var r in list)
        {
            next.Add(r);
            if (next.Count == groupSize)
            {
                groups.Add(next);
                next = new List<T>();
            }
        }

        return groups;
    }

    /// <summary>
    ///     Splits a list into multiple using a separator item (excluded).
    /// </summary>
    public static List<List<T>> SplitBy<T>(this List<T> list, Func<T, int, bool> separator)
    {
        var groups = new List<List<T>>();
        var next = new List<T>();
        for (var i = 0; i < list.Count; i++)
        {
            var r = list[i];
            if (!separator(r, i))
            {
                next.Add(r);
                continue;
            }

            ;

            groups.Add(next);
            next = new List<T>();
        }

        return groups;
    }

    /// <summary>
    ///     Splits a list into multiple using a separator item and including separating item.
    /// </summary>
    /// <param name="splitAfterSeparator">set to true if the separator indicates the beginning of the new group</param>
    public static List<List<T>> SplitByAndIncluding<T>(this List<T> list, Func<T, int, bool> separator,
        bool splitAfterSeparator)
    {
        var groups = new List<List<T>>();
        var next = new List<T>();
        for (var i = 0; i < list.Count; i++)
        {
            var r = list[i];
            if (!separator(r, i))
            {
                next.Add(r);
                continue;
            }

            ;

            if (splitAfterSeparator) next.Add(r);

            groups.Add(next);
            next = new List<T>();

            if (!splitAfterSeparator) next.Add(r);
        }

        return groups;
    }

    public static List<T> RemoveAtIndex<T>(this List<T> list, int index)
    {
        var copy = list.ToList();
        copy.RemoveAt(index);
        return list;
    }
}