using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022
{
    public class Day09 : Day
    {
        private List<(Direction dir, int)> _motions;

        public Day09()
        {
            // test input
            var input = @"R 4
            U 4
            L 3
            D 1
            R 4
            D 1
            L 5
            R 2";

            var t2 = @"R 5
U 8
L 8  
D 3  
R 17 
D 10 
L 25 
U 20";

            input = Input;
            //input = t2;

            _motions = input.AsListOfPatterns<char, int>("c n").Select(t =>
            {
                var dir = t.Item1 switch
                {
                    'R' => Direction.Right,
                    'U' => Direction.Up,
                    'L' => Direction.Left,
                    _ => Direction.Down
                };
                return (dir, t.Item2);
            }).ToList();
        }

        public override object Result1()
        {
            var grid = new Grid<int>(1000, 1000);
            var head = grid.GetCell(grid.Width / 2, grid.Height / 2);
            var tail = grid.GetCell(grid.Width / 2, grid.Height / 2);
            tail.Value++;

            var frames = new List<Image>();

            foreach (var (dir, steps) in _motions)
            {
                for (int i = 0; i < steps; i++)
                {
                    head = grid.Move(head, dir, 1);

                    tail = UpdateTail(head, tail, out var updated);

                    if (updated) tail.Value++;

                }

                // var image = grid.ToImage(cell => cell.Value > 0 ? KnownColor.SteelBlue : KnownColor.Black);
                //  frames.Add(image);
            }


            //   ImageUtils.CreateGif("day9p1.gif", frames);
            return grid.Cells.Count(z => z.Value > 0);
        }

        public override object Result2()
        {
            var grid = new Grid<int>(1000, 1000);
            var knots = Enumerable.Range(0, 10).Select(i =>
            {
                var x = grid.GetCell(grid.Width / 2, grid.Height / 2);
                x.Value++;
                return x;
            }).ToList();

            foreach (var (dir, steps) in _motions)
            {
                for (int i = 0; i < steps; i++)
                {
                    knots[0] = grid.Move(knots[0], dir);

                    for (int j = 0; j < knots.Count - 1; j++)
                    {
                        var head = knots[j];
                        var tail = knots[j + 1];

                        tail = UpdateTail(head, tail, out var updated);

                        if (updated && j == knots.Count - 2) tail.Value++;

                        knots[j + 1] = tail;
                    }
                }
            }

            return grid.Cells.Count(z => z.Value > 0);
        }

        private GridCell<int> UpdateTail(GridCell<int> head, GridCell<int> tail, out bool updated)
        {
            updated = false;

            var distance = head.Distance(tail);
            switch (distance)
            {
                case 2 when !tail.GetDiagonalNeighbors().Contains(head):
                    updated = true;
                    return tail.GetNeighbors().Intersect(head.GetNeighbors()).First();
                case > 2:
                    updated = true;
                    return tail.GetDiagonalNeighbors().Intersect(head.GetAllNeighbors()).First();
                default:
                    return tail;
            }
        }
    }
}