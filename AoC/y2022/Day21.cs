using System;
using System.Collections.Generic;
using AoC.Utils;

namespace AoC.y2022
{
    public class Day21 : Day
    {
        private Dictionary<string, object> _monkeys;

        public Day21()
        {
            // test input
            var input = @"root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32";

            input = Input;

            _monkeys = new Dictionary<string, object>();

            foreach (var line in input.AsListOf<string>())
            {
                var parts = line.Split(':');
                var name = parts[0].Trim();

                if (long.TryParse(parts[1].Trim(), out var num))
                {
                    _monkeys.Add(name, num);
                }
                else
                {
                    _monkeys.Add(name, parts[1].Trim());
                }
            }
        }

        long Compute(object i)
        {
            if (i is long l) return l;

            var trimmed = i as string;
            var expression = trimmed!.Split(' ');
            if (expression.Length == 1)
            {
                return long.TryParse(trimmed, out var num) ? num : Compute(_monkeys[trimmed]);
            }

            var x = Compute(expression[0]);
            var y = Compute(expression[2]);

            return expression[1] switch
            {
                "+" => x + y,
                "-" => x - y,
                "*" => x * y,
                "/" => x / y,
            };
        }

        public override object Result1()
        {
            var root = _monkeys["root"];
            return Compute(root);
        }

        public override object Result2()
        {
            var root = _monkeys["root"];
            var expression = (root as string)!.Split(' ');
            var a = expression[0];
            var b = expression[2];

            long GetDiff(long humn)
            {
                _monkeys["humn"] = humn;
                var x = Compute(a);
                var y = Compute(b);
                return x - y;
            }
            long from = 10000000000000L;
            long to = 0;

            long diff, num;

            do
            {
                num = (from + to) / 2L;

                diff = GetDiff(num);

                if (diff > 0) to = num;
                else from = num;

            } while (diff != 0);

            return num;
        }


    }
}