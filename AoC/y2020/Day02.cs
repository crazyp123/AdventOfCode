using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2020
{
    public class Day02 :Day
    {
        private List<(int, int, char, string)> _pss;

        public Day02()
        {
            _pss = Input.AsListOfPatterns<int, int, char, string>("n-n c: s");
        }

        public override object Result1()
        {
            return _pss.Count(IsValid);
        }

        public override object Result2()
        {
            return _pss.Count(IsValid2);
        }

        bool IsValid((int, int, char, string) log)
        {
            var n = log.Item4.Count(c => c == log.Item3);
            return n >= log.Item1 && n <= log.Item2;
        }

        bool IsValid2((int, int, char, string) log)
        {
            return log.Item4[log.Item1 - 1] == log.Item3 ^ log.Item4[log.Item2 - 1] == log.Item3;
        }
    }
}