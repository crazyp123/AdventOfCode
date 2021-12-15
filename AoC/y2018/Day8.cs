using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2018
{
    public class Day8
    {
        private Node Root;

        public Day8()
        {
            var input = Utils.Utils.GetInput(2018, 8).AsListOf<int>(" ");

            Root = ParseNode(input);

            Part1();
            Part2();
        }

        void Part1()
        {
            Utils.Utils.Answer(8, 1, Root.Sum());
        }

        void Part2()
        {
            Utils.Utils.Answer(8, 2, Root.Value());
        }


        Node ParseNode(List<int> input)
        {
            var node = new Node
            {
                ChildCount = input[0],
                MetaCount = input[1]
            };

            input.RemoveRange(0, 2);

            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = ParseNode(input);

                node.Children.Add(child);
            }

            node.Metadata.AddRange(input.Take(node.MetaCount));
            input.RemoveRange(0, node.MetaCount);

            return node;
        }

        public class Node
        {
            public int ChildCount;
            public int MetaCount;
            public List<Node> Children = new List<Node>();
            public List<int> Metadata = new List<int>();

            public int Sum()
            {
                return Metadata.Sum() + Children.Sum(node => node.Sum());
            }

            public int Value()
            {
                if (ChildCount == 0) return Metadata.Sum();

                return Metadata.Sum(i => i > 0 && i <= ChildCount ? Children[i - 1].Value() : 0);
            }
        }
    }
}