using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022
{
    public class Day11 : Day
    {
        private List<Monkey> _monkeys;

        public Day11()
        {
            // test input
            var input = @"
Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";

            input = Input;


            var asListOf = input.AsListOf<string>("\n\n");
            _monkeys = asListOf.Select(list => new Monkey(list.Trim())).ToList();
        }

        public override object Result1()
        {
            for (int i = 0; i < 20; i++)
            {
                foreach (var monkey in _monkeys)
                {
                    while (monkey.Items.Any())
                    {
                        var res = monkey.Inspect(true, 1);

                        _monkeys[res.monkey].Items.Add(res.item);
                    }
                }

            }

            var top = _monkeys.OrderByDescending(m => m.Inspected).Take(2).ToArray();

            return top[0].Inspected * top[1].Inspected;
        }

        public override object Result2()
        {
            var superMod = _monkeys.Select(m => m.TestDivisibleBy).Aggregate(1L, (monkey, monkey1) => monkey * monkey1);
            for (int i = 0; i < 10000; i++)
            {

                foreach (var monkey in _monkeys)
                {
                    while (monkey.Items.Any())
                    {
                        var res = monkey.Inspect(false, superMod);

                        _monkeys[res.monkey].Items.Add(res.item);
                    }
                }


            }

            var top = _monkeys.OrderByDescending(m => m.Inspected).Take(2).ToArray();

            return top[0].Inspected * top[1].Inspected;

            // 17903764569 too low
        }


        class Monkey
        {
            public int Num { get; set; }
            public List<long> Items = new List<long>();

            public Func<long, long> Operation;

            public long TestDivisibleBy { get; set; }

            public int TrueMonkey { get; set; }
            public int FalseMonkey { get; set; }

            public long Inspected { get; private set; }

            public Monkey(string text)
            {
                var lines = text.AsListOf<string>();
                Num = lines[0].Replace(":", "").Split(" ").Last().AsInt();
                Items = lines[1].Replace("Starting items: ", "").AsListOf<long>(",");
                var op = lines[2].Replace("Operation: new = ", "").Trim().Split(" ");

                var parsed = long.TryParse(op.Last(), out var opNum);

                Operation = op[1] switch
                {
                    "+" => old => old + (parsed ? opNum : old),
                    "*" => old => old * (parsed ? opNum : old),
                    _ => throw new ArgumentOutOfRangeException()
                };

                TestDivisibleBy = lines[3].Split(" ").Last().AsLong();

                TrueMonkey = lines[4].Split(" ").Last().AsInt();
                FalseMonkey = lines[5].Split(" ").Last().AsInt();

            }

            public (int monkey, long item) Inspect(bool divideByThree, long sup)
            {
                var worryItem = Items.First();
                Items.RemoveAt(0);

                worryItem = Operation(worryItem);

                if (divideByThree)
                {
                    worryItem /= 3;
                }
                else
                {
                    worryItem %= sup;
                }

                Inspected++;

                return worryItem % TestDivisibleBy == 0 ? (TrueMonkey, worryItem) : (FalseMonkey, worryItem);
            }

        }
    }
}