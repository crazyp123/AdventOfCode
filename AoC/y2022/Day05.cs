using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022;

public class Day05 :Day
{
    private List<(int, int, int)> _instr;
    private List<List<char>> _initSetup;

    public Day05()
    {
        var lines = Input.AsListOf<string>();
        var stacksNum = (lines[0].Length / 4) + 1;

        stacksNum = int.Parse(lines.First(s => s.Trim().StartsWith("1")).Trim().Last().ToString());

        _instr = new List<(int, int, int)>();
        var intstrReached = false;
        _initSetup = new List<List<char>>(stacksNum);

        for (int i = 0; i < stacksNum; i++)
        {
            _initSetup.Add(new List<char>());
        }

        foreach (var line in lines)
        {
            if (line.Trim().StartsWith("1"))
            {
                continue;
            }
            if (string.IsNullOrWhiteSpace(line))
            {
                intstrReached = true;
                continue;
            }
            if (!intstrReached)
            {
                var line2 = line.ToString();
                for (int i = 0; i < stacksNum; i++)
                {
                    var s = line2.Substring(0, 3).Trim();
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        _initSetup[i].Add(s[1]);
                    }

                    var trim = line2.Length < 4 ? line2.Length : 4;
                    line2 = line2[trim..];
                }
            }
            else
            {
                _instr.Add(line.ParsePattern<int, int, int>("move n from n to n"));
            }
        }
    }

    private List<Stack<char>> GetStacks()
    {
        return _initSetup.Select(stack =>
        {
            var x = stack.ToList();
            x.Reverse();
            return new Stack<char>(x);
        }).ToList();
    }

    public override object Result1()
    {
        var stacks = GetStacks();

        foreach (var (n, from, to) in _instr)
        {
            for (int i = 0; i < n; i++)
            {
                stacks[to - 1].Push(stacks[from - 1].Pop());
            }
        }

        return string.Concat(stacks.Select(s => s.Peek()));
    }

    public override object Result2()
    {
        var stacks = GetStacks();

        foreach (var (n, from, to) in _instr)
        {
            var buffer = new Stack<char>();
            for (int i = 0; i < n; i++)
            {
                buffer.Push(stacks[from - 1].Pop());
            }

            for (int i = 0; i < n; i++)
            {
                stacks[to - 1].Push(buffer.Pop());
            }
        }

        return string.Concat(stacks.Select(s => s.Peek()));
    }
}