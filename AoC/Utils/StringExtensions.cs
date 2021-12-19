using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils;

public static class StringExtensions
{
    public static string Sort(this string value)
    {
        return new string(value.OrderBy(c => c).ToArray());
    }

    public static string SortDesc(this string value)
    {
        return new string(value.OrderByDescending(c => c).ToArray());
    }

    public static string Shuffle(this string value, List<string> exclusions)
    {
        var list = value.ToList();
        list.Shuffle();
        var shuffled = new string(list.ToArray());
        while (exclusions.Contains(shuffled))
        {
            list.Shuffle();
            shuffled = new string(list.ToArray());
        }
        return shuffled;
    }
}

static class ExtensionsClass
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public static void Shuffle<T>(this IList<T> list, List<List<T>> exclude)
    {
        do Shuffle(list);
        while (exclude.Any(x => x.SequenceEqual(list)));
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

    private static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);

    public static Grid<T> ToGrid<T>(this List<List<T>> list)
    {
        return new Grid<T>(list);
    }
}