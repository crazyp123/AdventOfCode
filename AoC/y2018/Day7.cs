using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2018
{
    public class Day7
    {
        private Dictionary<char, Node> Nodes;
        private List<Instr> Instructions;
        private List<char?>[] _workers;

        public Day7()
        {
            Instructions = AdventOfCodeService.GetInput(2018, 7).AsListOf<string>()
                .Select(s => s.Split(' ').Where(s1 => s1.Length == 1 && char.IsUpper(s1[0])).ToArray())
                .Select(strings => new Instr(strings[0][0], strings[1][0]))
                .ToList();

            setup();


            Part1();

            setup();

            Part2();
        }

        private void setup()
        {
            Nodes = Instructions
                .SelectMany(instr => new List<char> { instr.X, instr.Y })
                .Distinct()
                .OrderBy(c => c)
                .ToDictionary(c => c, c => new Node
                {
                    Id = c,
                });

            foreach (var i in Instructions)
            {
                Nodes[i.Y].Links.Add(Nodes[i.X]);
            }
        }


        void Part1()
        {
            var answer = "";

            while (answer.Length != Nodes.Count)
            {
                var next = GetNext();

                answer += next;

                Nodes[next].Done = true;
            }

            Utils.Utils.Answer(7, 1, answer);
        }

        void Part2()
        {
            var workers = Enumerable.Range(0, 5).Select(i => new Worker()).ToList();

            var answer = "";
            int s = 0;
            while (answer.Length != Nodes.Count)
            {
                var c = GetNexts();
                for (var i = 0; i < c.Length; i++)
                {
                    var next = c[i];

                    var worker = workers.FirstOrDefault(w => w.Free());
                    if (worker == null) break;

                    worker.Node = Nodes[next];
                    worker.Node.Taken = true;
                }

                workers.ForEach(w =>
                {
                    if (w.Tick()) answer += w.Node.Id;
                });
                s++;
            }

            Utils.Utils.Answer(7, 2, s);
        }

        char GetNext()
        {
            return Nodes.First(pair => !pair.Value.Done && pair.Value.CanDo()).Key;
        }

        char[] GetNexts()
        {
            return Nodes.Where(pair => !pair.Value.Done && !pair.Value.Taken && pair.Value.CanDo()).Select(pair => pair.Key).ToArray();
        }

        public class Worker
        {
            public Node Node;
            private int n = 0;

            public bool Free()
            {
                return Node == null || Node.Done;
            }

            public bool Tick()
            {
                if (Node == null || Node.Done) return false;

                n++;

                if (Pos() + 60 == n)
                {
                    Node.Done = true;
                    n = 0;
                    return true;
                }

                return false;
            }

            int Pos()
            {
                return char.ToUpper(Node.Id) - 64;
            }
        }


        public class Node
        {
            public char Id;
            public bool Done;

            public bool Taken;

            public HashSet<Node> Links = new HashSet<Node>();

            public bool CanDo() => Links.Count == 0 || Links.All(node => node.Done);

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