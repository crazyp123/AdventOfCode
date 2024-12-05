using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day05 : Day
{
    private string[] _parts;
    private List<(int, int)> _orderRules;
    private List<List<int>> _pageNumbers;
    private Dictionary<int, List<int>> _orderDict;

    public Day05()
    {
        var test = @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47".Replace("\r", "");
        _parts = Input.Split("\n\n");
        _orderRules = _parts[0].AsListOf<string>().Select(s => s.Split("|"))
            .Select(g => (int.Parse(g[0]), int.Parse(g[1]))).ToList();
        _pageNumbers = _parts[1].AsListOf<string>().Select(s => s.AsListOf<int>(",")).ToList();
        _orderDict = _orderRules.GroupBy(t => t.Item1).ToDictionary(t => t.Key, t => t.Select(t => t.Item2).ToList());
    }

    public override object Result1()
    {
        return MidSum(_pageNumbers.Where(InOrder));
    }

    public override object Result2()
    {
        var toOrder = _pageNumbers.Where(x => !InOrder(x)).ToList();
        var ordered = toOrder.Select(Order).ToList();
        return MidSum(ordered);
    }

    private List<int> Order(List<int> list)
    {
        var buffer = list.ToList();
        var orderded = new List<int>();

        var rules = _orderRules.Where(t => list.Contains(t.Item1)).ToList();

        do
        {
            var next = list.First(page => rules.Where(t => !orderded.Contains(t.Item1)).All(t => t.Item2 != page));
            list.Remove(next);
            orderded.Add(next);
        } while (buffer.Count != orderded.Count);

        return orderded;
    }

    private bool InOrder(List<int> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var num = list[i];
            if (!_orderDict.TryGetValue(num, out var orders)) continue;
            var otherIxs = orders.Select(x => list.IndexOf(x)).Where(ix => ix >= 0).ToList();
            if (otherIxs.Any(y => y < i))
                return false;
        }

        return true;
    }

    private int MidSum(IEnumerable<List<int>> nums)
    {
        return nums.Sum(list => list[(int)Math.Floor(list.Count / 2f)]);
    }
}