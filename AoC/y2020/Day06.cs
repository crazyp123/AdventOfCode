using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2020
{
    public class Day06 : Day
    {
        private List<string> _answers;

        public Day06()
        {
            _answers = Input.AsListOf<string>("\n\n");
        }

        public override object Result1()
        {
            var i = _answers.Select(s => s.Replace("\n", "")).ToList();
            return i.Sum(group => group.Distinct().Count());
        }

        public override object Result2()
        {
            var i = _answers.Select(s => s.Split("\n")).ToList();

            return i.Sum(g =>
            {
                var all = string.Concat(g).Distinct().ToList();
                return all.Count(c => g.All(s => s.Contains(c)));
            });
        }
    }
}