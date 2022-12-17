using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Utils;
using Spectre.Console;

namespace AoC.y2022
{
    public class Day10 : Day
    {
        private int _x;
        private List<string> _program;

        public Day10()
        {
            // test input
            var input = tes;



            //            input = @"noop
            //addx 3
            //addx -5";

            input = Input;

            _program = input.AsListOf<string>();

            _x = 1;

        }

        public override object Result1()
        {
            var finishes = new Dictionary<int, int>();

            var sum = 0;

            var cycle = 1;

            var p = 0;
            while (true)
            {
                var program = p < _program.Count ? _program[p] : null;
                p++;

                if (program == null || program.Trim() == "noop")
                {
                    cycle += 1;
                }
                else
                {
                    var num = ParseAddx(program);
                    finishes.Add(cycle + 2, num);

                    if (new int[] { 20, 60, 100, 140, 180, 220 }.Contains(cycle + 1))
                    {
                        sum += ((cycle + 1) * _x);

                        if (cycle + 1 == 220) return sum;
                    }



                    cycle += 2;
                }

                if (finishes.ContainsKey(cycle)) _x += finishes[cycle];

                if (new int[] { 20, 60, 100, 140, 180, 220 }.Contains(cycle))
                {
                    sum += (cycle * _x);

                    if (cycle == 220) return sum;
                }
            }
        }

        private static int ParseAddx(string str)
        {
            try
            {
                var num = int.Parse(str.Replace("addx", "").Trim());
                return num;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override object Result2()
        {
            var result = new StringBuilder();

            var finishes = new Dictionary<int, int>();

            var cycle = 1;

            var x = 1;

            var p = 0;
            while (true)
            {
                var program = p < _program.Count ? _program[p] : null;
                p++;

                Print(cycle, x, result);

                if (program == null || program.Trim() == "noop")
                {
                    cycle += 1;
                }
                else
                {
                    var num = ParseAddx(program);
                    finishes.Add(cycle + 2, num);

                    Print(cycle + 1, x, result);



                    cycle += 2;
                }

                if (finishes.ContainsKey(cycle)) x += finishes[cycle];



                if (cycle > 240) break;
            }

            Console.WriteLine(result);

            return AnsiConsole.Ask("Read", "");
        }


        void Print(int cycle, int x, StringBuilder str)
        {
            var hpos = (cycle) % 40;

            var s = x - 1 < 0 ? 0 : x - 1;
            var sprite = Enumerable.Range(s, 3).ToList();


            if (sprite.Contains(hpos - 1))
            {
                str.Append("#");
            }
            else if (hpos == 0) str.Append("\n");
            else
            {
                str.Append(".");
            }
        }


        private string tes = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";
    }
}