using System.Collections.Generic;
using System.Linq;
using AoC.Objects;
using AoC.Utils;

namespace AoC.y2021
{
    public class Day09 : Day
    {
        private Grid<int> _grid;
        private List<GridCell<int>> _lowPoints;

        public Day09()
        {
            // testInput
            var input = "";

            input = Input;

            var data = input.AsListOf<string>().Select(s => s.Select(c => int.Parse(c.ToString())).ToList()).ToList();
            _grid = data.ToGrid();
        }

        public override object Result1()
        {
            _lowPoints = _grid.MapCells((x, y, cell) =>
                {
                    var n = _grid.GetNeighborCells(x, y);
                    return (cell, n);
                }).Where(tuple => tuple.n.All(neighCell => neighCell.Value > tuple.cell.Value))
                .Select(t => t.cell).ToList();

            var risks = _lowPoints.Select(tuple => tuple.Value + 1)
                .ToArray();

            return risks.Sum();
        }

        public override object Result2()
        {
            var topBasins = _lowPoints.Select(lp => lp.Flood(cell => cell.Value != 9).Count).OrderByDescending(i => i)
                .Take(3).ToArray();

         return topBasins.Aggregate(1, (x, x1) => x * x1);
        }
    }
}