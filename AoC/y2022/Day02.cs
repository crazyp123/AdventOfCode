using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022;

public class Day02 : Day
{
    enum Hand
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    private List<(char, char)> _guide;

    private Dictionary<char, Hand> map = new Dictionary<char, Hand>
    {
        { 'A', Hand.Rock },
        { 'B', Hand.Paper },
        { 'C', Hand.Scissors },
        { 'X', Hand.Rock },
        { 'Y', Hand.Paper },
        { 'Z', Hand.Scissors }
    };

    private string _test = @"A Y
B X
C Z";

    public override object Result1()
    {
        _guide = Input.AsListOfPatterns<char, char>("c c");
        return _guide.Select(t => Score(map[t.Item1], map[t.Item2])).Sum();
    }

    public override object Result2()
    {
        var x  = _test.AsListOfPatterns<char, char>("c c");
        return x.Select(t => Score2(map[t.Item1], t.Item2)).Sum();
    }

    int Score(Hand them, Hand me)
    {
        var i = (int)me;
        if (me == them)
        {
            return i + 3;
        }
        switch (me)
        {
            case Hand.Rock when them == Hand.Scissors:
            case Hand.Paper when them == Hand.Rock:
            case Hand.Scissors when them == Hand.Paper:
                return i + 6;
        }
        return i;
    }

    int Score2(Hand opp, char x)
    {
        if (x == 'X')
        {
            switch (opp)
            {
                case Hand.Rock:
                    return Score(opp, Hand.Scissors);
                case Hand.Paper:
                    return Score(opp, Hand.Rock);
                case Hand.Scissors:
                    return Score(opp, Hand.Paper);
            }
        }

        if (x == 'Y')
        {
            return Score(opp, opp);
        }

        if (x == 'Z')
        {
            switch (opp)
            {
                case Hand.Rock:
                    return Score(opp, Hand.Paper);
                case Hand.Paper:
                    return Score(opp, Hand.Scissors);
                case Hand.Scissors:
                    return Score(opp, Hand.Rock);
            }
        }

        return 0;
    }
}