using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    public class Day7
    {
        protected internal Dictionary<char, List<char>> Instructions;

        public Day7()
        {
            var instrs = Utils.GetInput(2018, 7).AsListOf<string>()
                .Select(s => s.Split(' ').Where(s1 => s1.Length == 1 && char.IsUpper(s1[0])).ToArray())
                .Select(strings => new Instr(strings[0][0], strings[1][0]));

            Instructions = instrs
                .GroupBy(instr => instr.Y)
                .ToDictionary(x => x.Key, x => x.Any() ? x.Select(instr => instr.X).ToList() : new List<char>());

             foreach (var c1 in instrs
                 .SelectMany(instr => new List<char> {instr.X, instr.Y})
                 .Distinct()
                 .Where(c => !Instructions.ContainsKey(c)))
             {
                 Instructions.Add(c1, new List<char>());
             }

            Part1();
        }


        void Part1()
        {
            var available = new List<char>(Instructions.Where(pair => pair.Value.Count == 0).Select(pair => pair.Key));
            foreach (var VARIABLE in available)
            {
                Instructions.Remove(VARIABLE);
            }
            var final = "";

            while (available.Count > 0)
            {
                var next = available[0];
                final += next;
                available.RemoveAt(0);

                foreach (var i in Instructions)
                {
                    i.Value.RemoveAll(c => c == next);
                }

                available.AddRange(Instructions.Where(pair => pair.Value.Count == 0).Select(pair => pair.Key));
                available = available.OrderBy(c => c).ToList();
                foreach (var VARIABLE in available)
                {
                    Instructions.Remove(VARIABLE);
                }
            }

            Utils.Answer(7, 1, final);
        }




        public struct Instr
        {
            public char X;
            public char Y;

            public Instr(char x, char y)
            {
                X = x;
                Y = y;
            }
        }
    }
}