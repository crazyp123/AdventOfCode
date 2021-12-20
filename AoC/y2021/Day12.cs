using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
using QuikGraph;

namespace AoC.y2021
{
    public class Day12 : Day
    {
        private UndirectedGraph<string, SEdge<string>> _graph;


        public Day12()
        {
            // test input
            var input = "start-A\nstart-b\nA-c\nA-b\nb-d\nA-end\nb-end";

            input = Input;

            var list = input.AsListOfPatterns<string, string>("s-s");
            _graph = list.Select(tuple => new SEdge<string>(tuple.Item1, tuple.Item2)).ToUndirectedGraph<string, SEdge<string>>();
        }

        public override object Result1()
        {
            return PathsToEnd("start",  new List<List<string>>(), new List<string>()).Count;
        }

        public override object Result2()
        {
            return PathsToEnd("start", new List<List<string>>(), new List<string>(), true).Count;
        }

        List<List<string>> PathsToEnd(string from, List<List<string>> paths, List<string> path, bool checkCount = false)
        {
            var thisPath = path.ToList();
            thisPath.Add(from);

            var nodes = _graph.AdjacentVertices(from);

            foreach (var node in nodes)
            {
                if (char.IsLower(node[0]) && thisPath.Contains(node))
                {
                    if (!checkCount)
                    {
                        continue;
                    }

                    var maxCountReached = thisPath.Where(c => char.IsLower(c[0])).GroupBy(s => s).Any(g => g.Count() == 2);

                    if (node == "start" || node == "end" || maxCountReached)
                    {
                        continue;
                    }
                }

                if (node == "end")
                {
                    var x = thisPath.ToList();
                        x.Add(node);
                    paths.Add(x);
                    continue;
                }

                PathsToEnd(node, paths, thisPath, checkCount);

            }

            return paths;
        }


    }
}