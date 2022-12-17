using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Transactions;
using AoC.Utils;
using QuikGraph.Algorithms;

namespace AoC.y2022
{
    public class Day13 : Day
    {
        private List<List<object>> _packets = new List<List<object>>();

        public Day13()
        {
            // test input
            var input = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";


            _packets = Input.AsListOf<string>("\n\n").Select(s =>
                    s.AsListOf<string>().Where(s => !string.IsNullOrEmpty(s.Trim()))
                        .Select(line => ParseList(line[1..^1])).ToList())
                .ToList();
        }

        public override object Result1()
        {
            return _packets
                .Select((list, ix) => (list, n: ix + 1))
                .Where(g => GetOrder(g.list[0], g.list[1]) == 1)
                .Sum(t => t.n);
        }

        public override object Result2()
        {
            var packs = _packets.SelectMany(list => list).ToList();

            var a = ParseList("[2]");
            var b = ParseList("[6]");
            packs.AddRange(new[] { a, b, });

            packs.Sort((o, o1) => GetOrder(o, o1) * -1);

            var ax = packs.IndexOf(a);
            var bx = packs.IndexOf(b);
            return (ax + 1) * (bx + 1);
        }


        int GetOrder(object left, object right)
        {
            if (left is int lint && right is List<object>)
            {
                left = new List<object> { lint };
            }

            if (right is int rint && left is List<object>)
            {
                right = new List<object> { rint };
            }

            if (left is int L && right is int R)
            {
                if (L < R) return 1;
                if (L == R) return 0;
                return -1;
            }

            if (left is List<object> leftList && right is List<object> rightList)
            {
                var i = 0;
                while (i < leftList.Count && i < rightList.Count)
                {
                    switch (GetOrder(leftList[i], rightList[i]))
                    {
                        case 1:
                            return 1;
                        case -1:
                            return -1;
                    }
                    i++;
                }

                if (leftList.Count < rightList.Count) return 1;
                if (leftList.Count == rightList.Count) return 0;

                return -1;
            }

            return 0;
        }

        object ParseList(string line)
        {
            return ParseListRec(line, new List<object>(), out _);
        }

        object ParseListRec(string line, List<object> current, out int moves)
        {
            moves = 0;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (char.IsNumber(c))
                {
                    var num = c.ToString();
                    if (i + 1 < line.Length && char.IsNumber(line[i + 1]))
                    {
                        num += line[i + 1];
                        i++;
                    }
                    current.Add(num.AsInt());
                }
                else if (c == '[')
                {
                    var child = ParseListRec(line[(i + 1)..], new List<object>(), out var innerMoves);
                    current.Add(child);
                    i += innerMoves + 1;
                }
                else if (c == ']')
                {
                    moves = i;
                    return current;
                }
            }

            return current;
        }
    }

}