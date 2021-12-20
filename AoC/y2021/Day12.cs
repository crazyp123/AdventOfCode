using System.Linq;
using AoC.Utils;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Search;

namespace AoC.y2021
{
    public class Day12 : Day
    {
        private AdjacencyGraph<string, SEdge<string>> _graph;

        public Day12()
        {
            // test input
            var input = "";

            input = Input;

            var list = input.AsListOfPatterns<string, string>("s-s");
            _graph = list.Select(tuple => new SEdge<string>(tuple.Item1, tuple.Item2)).ToAdjacencyGraph<string, SEdge<string>>();
        }

        public override object Result1()
        {
            var alg = new DepthFirstSearchAlgorithm<string, SEdge<string>>(_graph);

            alg.SetRootVertex("start");

            _graph.TryGetOutEdges("start", out var edges);


            return 1;
        }

        public override object Result2()
        {
            throw new System.NotImplementedException();
        }
    }
}