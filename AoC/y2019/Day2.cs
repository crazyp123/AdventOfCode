using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC.Utils;

namespace AoC.y2019
{
    public class Day2
    {
        private List<int> _input;

        public Day2()
        {
            _input = Utils.Utils.GetInput(2019, 2).AsListOf<int>(",");
        }

        public void Part1()
        {
            Utils.Utils.Answer(2, 1, Run(12, 2));
        }

        public void Part2()
        {
            var nouns = Enumerable.Range(0, 99);
            var verbs = Enumerable.Range(0, 99);

            foreach (var noun in nouns)
            {
                foreach (var verb in verbs)
                {
                    if (Run(noun, verb) == 19690720)
                    {
                        Utils.Utils.Answer(2, 2, 100 * noun + verb);
                        return;
                    }
                }
            }

        }

        private int Run(int noun, int verb)
        {
            var result = _input.ToArray();

            result[1] = noun;
            result[2] = verb;

            for (var i = 0; i < result.Length; i += 4)
            {
                if (result[i] == 99) break;

                var code = result[i];

                var x = result[i + 1];
                var y = result[i + 2];
                var z = result[i + 3];

                if (code == 1)
                {
                    result[z] = result[x] + result[y];
                }
                else if (code == 2)
                {
                    result[z] = result[x] * result[y];
                }
            }

            return result[0];
        }
    }
}