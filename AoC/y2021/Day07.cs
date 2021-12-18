using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.y2021
{
    internal class Day07 : Day
    {
        private List<(int Key, int)> _distinct;

        public Day07()
        {
            var input = "16,1,2,0,4,2,7,1,2,14";

            input = Input;

            _distinct = input.AsListOf<int>(",")
                .GroupBy(x => x)
                .Select(x => (x.Key, x.Count()))
                .ToList();
        }

        public override object Result1()
        {
            return _distinct.Min(tuple =>
            {
                var cost = 0;
                foreach (var (x, xc) in _distinct)
                {
                    if (x == tuple.Key)
                    {
                        continue;
                    }

                    cost += Math.Abs(x - tuple.Key) * xc;
                }

                return cost;
            });
        }

        public override object Result2()
        {
            return _distinct.Min(tuple =>
            {
                var cost = 0;
                foreach (var (x, xc) in _distinct)
                {
                    if (x == tuple.Key)
                    {
                        continue;
                    }

                    var abs = Enumerable.Range(1, Math.Abs(x - tuple.Key)).Sum();
                    cost += abs * xc;
                }

                return cost;
            });
        }
    }
}