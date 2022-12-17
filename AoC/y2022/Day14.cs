using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using AoC.Utils;
using QuikGraph;

namespace AoC.y2022
{
    public class Day14 : Day
    {
        private int _xOffset;
        private Grid<char> _grid;
        private List<List<(int, int)>> _rocks;

        private List<Image> _images = new List<Image>();

        private bool _createImg = true;

        public Day14()
        {
            // test input
            var input = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";

            input = Input;

            _rocks = input.AsListOf<string>().Select(s => s.AsListOfPatterns<int, int>("n,n", "->")).ToList();

            InitGrid();
        }

        private void InitGrid()
        {
            var minx = _rocks.SelectMany(l => l).MinBy(t => t.Item1).Item1;
            var maxx = _rocks.SelectMany(l => l).MaxBy(t => t.Item1).Item1;
            var maxy = _rocks.SelectMany(l => l).MaxBy(t => t.Item2).Item2;


            var margin = 10;
            _xOffset = minx - (margin / 2);

            _grid = new Grid<char>(maxx - minx + margin, maxy + 2);
            _grid.Apply(cell =>
            {
                cell.Value = '.';
                cell.Metadata = false;
            });
            foreach (var rockLine in _rocks)
            {
                for (var i = 0; i < rockLine.Count - 1; i++)
                {
                    var (x, y) = rockLine[i];
                    var (x1, y1) = rockLine[i + 1];

                    var dir = x < x1 ? Direction.Right : x > x1 ? Direction.Left : y1 > y ? Direction.Up : Direction.Down;
                    var steps = new[] { Math.Abs(x - x1), Math.Abs(y - y1) }.Max();
                    var cell = _grid.GetCell(x - _xOffset, y);
                    for (int j = 0; j <= steps; j++)
                    {
                        var gridCell = _grid.Move(cell, dir, j);
                        gridCell.Value = '#';
                        gridCell.Metadata = true;
                    }
                }
            }
        }

        public override object Result1()
        {
            var rests = 0;

            while (Drop())
            {
                rests++;
            }

            if (_createImg) ImageUtils.CreateGif("day14p1.gif", _images);

            return rests;
        }

        public override object Result2()
        {
            InitGrid();

            var rests = 0;

            while (Drop(true))
            {
                rests++;
            }

            return rests;
        }


        bool Drop(bool part2 = false)
        {
            var sandO = _grid.GetCell(500 - _xOffset, 0);

            var current = sandO;
            var next = _grid.Move(current, Direction.Up);

            while ((!part2 && next != null) || (part2 && current != null))
            {
                if (sandO.Value == 'o') return false;
                if (part2 && next == null)
                {
                    current.Value = 'o';
                    current.Metadata = true;
                    return true;
                }

                if (next.Value is '#' or 'o')
                {
                    var left = _grid.Move(next, Direction.Left);
                    var right = _grid.Move(next, Direction.Right);

                    if (left is { Value: '.' })
                    {
                        next = left;
                    }
                    else if (right is { Value: '.' })
                    {
                        next = right;
                    }
                    else if (((bool)left.Metadata && (bool)right.Metadata) || left.IsOnEdge())
                    {
                        // rest
                        current.Value = 'o';
                        current.Metadata = true;

                        //      Console.WriteLine(_grid.Print(cell => cell.Value.ToString()));
                        //      Console.ReadLine();

                        if (_createImg)
                            _images.Add(_grid.ToImage(cell => cell.Value switch
                            {
                                '#' => KnownColor.Black,
                                'o' => KnownColor.SandyBrown,
                                _ => KnownColor.White,
                            }));

                        return true;
                    }
                }

                current = next;
                next = _grid.Move(next, Direction.Up);
            }

            return false;
        }




    }
}