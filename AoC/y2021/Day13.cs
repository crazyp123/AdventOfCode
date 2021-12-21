using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2021
{
    public class Day13 : Day
    {
        private List<(int x, int y)> _points;
        private List<(string axis, int value)> _folds;
        private Grid<char> _grid;

        public Day13()
        {
            // test input
            var input = "";

            input = Input;

            var list = input.AsListOf<string>();

            _points = list.TakeWhile(s => !string.IsNullOrWhiteSpace(s)).Select(s =>
            {
                var split = s.Split(",", StringSplitOptions.TrimEntries);
                return (x:int.Parse(split[0]),y: int.Parse(split[1]));
            }).ToList();


            _folds = list.Where(s => s.StartsWith("fold")).Select(s =>
            {
                var split = s.Replace("fold along", "").Trim().Split("=", StringSplitOptions.TrimEntries);
                return (axis:split[0], value:int.Parse(split[1]));
            }).ToList();


            var width = _points.Max(t => t.x)+1;
            var height = _points.Max(t => t.y) + 1;
            _grid = new Grid<char>(width, height);
            _grid.Apply(cell => cell.Value = '.');
            foreach (var (x, y) in _points)
            {
                _grid.Set(x, y, '#');
            }
        }

        public override object Result1()
        {
            var grid = _grid;
            foreach (var (axis, pos) in _folds.Take(1))
            {
                var vertically = axis == "x";
                grid = grid.FoldInward(vertically, pos, vertically ? 0 : 1, true, (cell0, cell1, merged) =>
                {
                    merged.Value = cell0 is not null && cell0.Value == '#' || cell1 is not null && cell1.Value == '#'
                        ? '#'
                        : ' ';
                });
            }

            return grid.Cells.Count(c => c.Value == '#');
        }

        public override object Result2()
        {
            var grid = _grid;
            foreach (var (axis, pos) in _folds)
            {
                var vertically = axis == "x";
                grid = grid.FoldInward(vertically, pos, vertically ? 0 : 1, true, (cell0, cell1, merged) =>
                {
                    merged.Value = cell0 is not null && cell0.Value == '#' || cell1 is not null && cell1.Value == '#'
                        ? '#'
                        : ' ';
                });
            }

            return grid.Print(cell => cell.Value.ToString(), true);
        }
    }
}