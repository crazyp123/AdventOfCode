using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using AoC.Utils;
using QuikGraph.Algorithms;

namespace AoC.y2022
{
    public class Day08 : Day
    {
        private Grid<int> _grid;

        public Day08()
        {
            var test = @"30373
25512
65332
33549
35390";
            _grid = Input.AsListOf<string>().Select(s => s.Trim().Select(c => int.Parse(c.ToString())).ToList()).ToList().ToGrid();
        }

        public override object Result1()
        {

            var x = _grid.GetRows().SelectMany(row =>
            {
                var a = GetVisible(row);
                var b = GetVisible(row.OrderByDescending(c => c.X));
                return a.Concat(b);
            });

            var y = _grid.GetCols().SelectMany(col =>
            {
                var a = GetVisible(col);
                var b = GetVisible(col.OrderByDescending(c => c.Y));
                return a.Concat(b);
            });

            return x.Concat(y).Distinct().Count();
        }

        private List<GridCell<int>> GetVisible(IEnumerable<GridCell<int>> row)
        {
            var visible = new List<GridCell<int>>();
            var _row = row as GridCell<int>[] ?? row.ToArray();

            for (int i = 0; i < _row.Length; i++)
            {
                var cell = _row[i];
                if (i == 0)
                {
                    visible.Add(cell);
                    continue;
                }

                if (cell.Value > _row[..i].MaxBy(c => c.Value)!.Value)
                {
                    visible.Add(cell);
                }
            }

            return visible;
        }

        public override object Result2()
        {
            var x = _grid.MapCells((x, y, cell) =>
                DirectionUtils.Directions.Aggregate(1, (current, dir) => current * _grid.GetAllToEdge(cell, dir).Count(next => cell.Value >= next.Value)))
                .Max();


            return _grid.MapCells((x, y, cell) => VisibleFromTree(cell)).Max();
        }

        private int VisibleFromTree(GridCell<int> cell)
        {
            if (cell.IsOnEdge()) return 0;

            var prod = 1;

            foreach (var dir in DirectionUtils.Directions)
            {
                var num = 0;
                var i = 1;
                while (true)
                {
                    var next = _grid.GetNeighborCell(cell, dir, i);

                    if (next == null)
                    {
                        break;
                    }

                    if (cell.Value >= next.Value)
                    {
                        num++;
                    }

                    if (cell.Value <= next.Value)
                    {
                        break;
                    }

                    i++;
                }

                prod *= num;
            }
            return prod;
        }
    }
}