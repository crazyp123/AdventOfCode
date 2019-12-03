using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode;

namespace AoC.y2019
{
    public class Day1
    {
        private List<int> _input;

        public Day1()
        {
            _input = Utils.GetInput(2019, 1).AsListOf<int>();

            Part1();
            Part2();
        }


        public void Part1()
        {


            var r = _input.Select(Calc).Sum();

            Utils.Answer(1, 1, r);

        }

        public void Part2()
        {
            var r = _input.Select(z => CalcRec(z, 0)).Sum();

            Utils.Answer(1, 2, r);
        }

        private int Calc(int mass)
        {
            return ((int)Math.Floor((decimal)mass / 3)) - 2;
        }

        public int CalcRec(int mass, int tot = 0)
        {
            while (true)
            {
                var x = Calc(mass);

                if (x <= 0) return tot;

                mass = x;
                tot = x + tot;
            }
        }
    }
}