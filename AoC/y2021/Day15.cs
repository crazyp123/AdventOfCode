using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
using QuikGraph;
using QuikGraph.Algorithms;

namespace AoC.y2021
{
    public class Day15 : Day
    {
        private Grid<int> _grid;

        public Day15()
        {
            // test input
            var input = "1163751742\n1381373672\n2136511328\n3694931569\n7463417111\n1319128137\n1359912421\n3125421639\n1293138521\n2311944581";

            input = Input;

            _grid = input.AsListOf<string>().Select(s => s.AsListOfNumbers()).ToList().ToGrid();
        }

        public override object Result1()
        {
            var graph = _grid.BuildAdjacencyGraph();

            var rootCell = _grid.GetCell(0,0);
            var targetCell = _grid.GetCell(_grid.Width-1, _grid.Height-1);

            var alg = graph.ShortestPathsAStar(edgeWeights: e => e.Target.Value, cell => cell.Value, rootCell);
            alg.Invoke(targetCell, out var result);

            var r = result.ToList();
            return r.Sum(e => e.Target.Value);
        }

        public override object Result2()
        {
            var grid2 = new Grid<int>(_grid.Width * 5, _grid.Height * 5);
            
            var cols = _grid.GetCols().Select(row => Enumerable.Repeat(row, 5).Select((row, ix) => row.Select(cell => Increase(cell.Value, ix)).ToArray()).Aggregate(new List<int>(), (res, row) => res.Concat(row).ToList())).ToList();
            grid2.SetCols(cols);

            var rows = grid2.GetRows().Select(row => Enumerable.Repeat(row.Take(_grid.Width).ToArray(), 5).Select((row, ix) => row.Select(cell => Increase(cell.Value, ix)).ToArray()).Aggregate(new List<int>(), (res, row)=> res.Concat(row).ToList())).ToList();
            grid2.SetRows(rows);

            var graph = grid2.BuildAdjacencyGraph();

            var rootCell = grid2.GetCell(0, 0);
            var targetCell = grid2.GetCell(grid2.Width-1, grid2.Height-1);

            var alg = graph.ShortestPathsAStar(edgeWeights: e => e.Target.Value, cell => cell.Value, rootCell);
            alg.Invoke(targetCell, out var result);

            var r = result.ToList();
            return r.Sum(e => e.Target.Value);

        }

        int Increase(int x, int i)
        {
            var offset = x + i >= 10 ? 1 : 0;
            return ((x + i) % 10) + offset;
        }
    }
}