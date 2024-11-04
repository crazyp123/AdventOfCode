using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
using QuikGraph.Algorithms;

namespace AoC.y2022
{
    public class Day18 : Day
    {
        private Grid3D<bool> _grid;
        private List<GridCell3D<bool>> _cells;

        public Day18()
        {
            // test input
            var input = @"2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5";


            input = Input;

            var points = input
                .AsListOfPatterns<int, int, int>("n,n,n")
                .Select(t => new Point(t.Item1, t.Item2, t.Item3))
                .ToList();

            var offset = 3;

            _grid = new Grid3D<bool>(points.Max(p => p.X) + offset, points.Max(p => p.Y) + offset, points.Max(p => p.Z) + offset);


            _cells = points.Select(p => _grid.GetCell(p.X + 1, p.Y + 1, p.Z + 1)).ToList();
            _cells.ForEach(c => c.Value = true);
        }

        public override object Result1()
        {
            return _cells.Sum(c => c.GetNeighbors().Count(c => c.Value == false));
        }

        public override object Result2()
        {
            var graph = _grid.BuildAdjacencyGraph(cell => cell.GetNeighbors().Where(c => !c.Value));

            var findPath = graph.ShortestPathsDijkstra(edge => 1, _grid.GetCell(0, 0, 0));

            var notReachable = 0;

            foreach (var cell in _cells)
            {
                foreach (var nb in cell.GetNeighbors().Where(c => !c.Value))
                {
                    findPath.Invoke(nb, out var path);
                    if (path == null) notReachable++;
                }
            }

            return (int)Result1() - notReachable;
        }
    }
}