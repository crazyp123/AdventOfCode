using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022
{
    public class Day17 : Day
    {
        private List<Direction> _jets;
        private List<Grid<string>> _shapes;
        private List<Grid<int>> _shapes2;
        private World2D<string> _world;

        public Day17()
        {
            // test input
            var input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

            input = Input;

            _shapes = new List<Grid<string>>
            {
                ParseShape("####"),
                ParseShape(@".#.
###
.#."),
                ParseShape(@"###
..#
..#"),
                ParseShape(@"#
#
#
#"),
                ParseShape(@"##
##")
            };

            _shapes2 = new List<Grid<int>>
            {
                new(new List<List<int>>
                {
                    new() { 1, 1, 1, 1 }
                }),
                new(new List<List<int>>
                {
                    new() { 0, 1, 0 },
                    new() { 1, 1, 1 },
                    new() { 0, 1, 0 }
                }),
                new(new List<List<int>>
                {
                    new() { 1, 1, 1 },
                    new() { 0, 0, 1 },
                    new() { 0, 0, 1 }
                }),
                new(new List<List<int>>
                {
                    new() { 1 },
                    new() { 1 },
                    new() { 1 },
                    new() { 1 },
                }),
                new(new List<List<int>>
                {
                    new() { 1, 1 },
                    new() { 1, 1 }
                })
            };

            _jets = input.Trim().Select(c => c == '<' ? Direction.Left : Direction.Right).ToList();


            _world = new World2D<string>(7, 2022 * 4);
        }

        Grid<string> ParseShape(string shape)
        {
            var grid = new Grid<string>(shape.Trim().AsListOf<string>()
                .Select(s => s.Trim().ToCharArray().Select(c => c.ToString()).ToList()).ToList());
            grid.Apply(c => c.Value = c.Value == "." ? null : c.Value);
            return grid;
        }

        public override object Result1()
        {
            return GetHeight2(2022);
        }

        public override object Result2()
        {
            return GetHeight(1000000000000);
        }

        long GetHeight(long count)
        {
            var objCount = 0L;

            var nextShapeIx = 0;
            var nextJetIx = 0;
            var falling = false;

            var topY = 0;

            WorldObject<string> currentObject = null;

            while (objCount < count || falling)
            {
                if (!falling)
                {
                    var shape = _shapes[nextShapeIx % _shapes.Count];
                    currentObject = _world.AddObject(shape, new Point(2, topY + 3));
                    nextShapeIx++;
                    falling = true;

                    objCount++;
                }
                else
                {
                    var dir = _jets[nextJetIx % _jets.Count];

                    nextJetIx++;

                    currentObject.TryMove(dir);

                    falling = currentObject.TryMove(Direction.Down);

                    topY = _world.Objects.Max(o => o.WorldCells.Max(c => c.Y)) + 1;
                }
            }

            return topY;
        }

        long GetHeight2(long count)
        {
            var objCount = 0L;

            var nextShapeIx = 0;
            var nextJetIx = 0;
            var falling = false;
            var topY = 0L;

            var row = new long[7] { 0, 0, 0, 0, 0, 0, 0 };

            Grid<int> shapeGrid = null;

            (long x, long y) pos = (0, 0);

            var shapePositions = new List<(long x, long y)>();

            while (objCount < count || falling)
            {
                if (!falling)
                {
                    shapeGrid = _shapes2[nextShapeIx % _shapes.Count];
                    pos = (2, topY + 3);

                    shapePositions = shapeGrid.Cells
                        .Where(c => c.Value != 0)
                        .Select(cell => (x: pos.x + cell.X, y: pos.y + cell.Y))
                        .ToList();

                    falling = true;
                    nextShapeIx++;
                    objCount++;
                }
                else
                {
                    var dir = _jets[nextJetIx % _jets.Count];

                    nextJetIx++;

                    var jetMove = MoveShape(dir, shapeGrid, pos, row, out pos, out var shapePos);
                    if (jetMove)
                    {
                        shapePositions = shapePos;
                    }

                    falling = MoveShape(Direction.Down, shapeGrid, pos, row, out pos, out var shapePos2);
                    if (!falling)
                    {
                        for (int w = 0; w < shapeGrid.Width; w++)
                        {
                            row[pos.x + w] += shapeGrid.GetCol(w).Where(c => c.Value != 0).Max(c => c.Y) + 1;
                        }
                    }

                    topY = row.Max();
                }
            }

            return topY;
        }

        bool MoveShape(Direction dir, Grid<int> shapeGrid, (long x, long y) pos, long[] bottom,
            out (long x, long y) nextPosition, out List<(long, long)> shapePositions)
        {
            nextPosition = pos;
            shapePositions = null;

            var x = pos.x;
            var y = pos.y;

            switch (dir)
            {
                case Direction.Down:
                    y -= 1;
                    break;
                case Direction.Left:
                    x -= 1;
                    break;
                case Direction.Right:
                    x += 1;
                    break;
            }

            var newPositions = shapeGrid.Cells
                .Where(c => c.Value != 0)
                .Select(cell => (x: x + cell.X, y: y + cell.Y))
                .ToList();


            if (newPositions.Any(c => c.x is < 0 or > 6 || bottom[c.x] > c.y)) return false;

            nextPosition = (x, y);
            shapePositions = newPositions;

            return true;
        }
    }
}