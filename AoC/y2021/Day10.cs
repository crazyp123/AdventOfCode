using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2021;

public class Day10 : Day
{
    private Dictionary<char, int> errorScores = new Dictionary<char, int>
    {
        { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 }
    };

    private Dictionary<char, int> repairScores = new Dictionary<char, int>
    {
        { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 }
    };

    private List<char> _openP;
    private List<char> _closeP;
    private List<string> _lines;

    public Day10()
    {
        // test input
        var input = "";

        input = Input;

        _lines = input.AsListOf<string>();

        _openP = new[] { '(', '{', '[', '<' }.ToList();
        _closeP = new[] { ')', '}', ']', '>' }.ToList();

    }

    private int FindError(string input)
    {
        Stack<char> tracker = new Stack<char>();
        foreach (var x in input)
        {
            if (_openP.Contains(x)) tracker.Push(x);
            else
            {
                var open = tracker.Peek();
                if (x == _closeP[_openP.IndexOf(open)])
                {
                    tracker.Pop();
                }
                else
                {
                    return errorScores[x];
                }
            }
        }

        return 0;
    }

    private long Repair(string input)
    {
        var score = 0L;

        Stack<char> tracker = new Stack<char>();
        foreach (var x in input)
        {
            if (_openP.Contains(x)) tracker.Push(x);
            else
            {
                var open = tracker.Peek();
                if (x == _closeP[_openP.IndexOf(open)])
                {
                    tracker.Pop();
                }
            }
        }

        foreach (var open in tracker)
        {
            score *= 5;
            score += repairScores[open];
        }

        return score;
    }

    public override object Result1()
    {
       return  _lines.Sum(FindError);
    }

    public override object Result2()
    {
        var scores = _lines.Where(s => FindError(s) == 0).Select(Repair).OrderBy(l => l).ToArray();
        return scores[(int)Math.Floor(scores.Length / 2d)];
    }
}