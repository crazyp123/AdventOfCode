using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AoC.Utils;
using Spectre.Console;

namespace AoC.y2024;

public class Day06 : Day
{
    private Grid<char> _grid;
    private GridCell<char> _guardPosition;
    private Direction _guardDirection;


    public Day06()
    {
        var test = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...".Replace("\r", "");
        var list = Input.AsListOf<string>().Select(s => s.ToList()).ToList();
        _grid = list.ToGrid();
        _grid.BottomLeftOrigin = true;

        _guardPosition = _grid.Cells.Find(cell =>
            cell.Value == '>' || cell.Value == '<' || cell.Value == '^' || cell.Value == 'v');
        _guardDirection = GetDirection(_guardPosition.Value);
        _guardPosition.Value = '.';
    }


    public override object Result1()
    {
        //  var images = Images(_guardPosition, _guardDirection);
        //  ImageUtils.CreateGif("C:\\SourceCode\\day6.gif", images, ImageUtils.Fps.Fps60);
        return Visited(_guardPosition, _guardDirection).Count;
    }

    private HashSet<GridCell<char>> Visited(GridCell<char> guardPosition, Direction direction)
    {
        var visited = new HashSet<GridCell<char>>() { guardPosition };
        while (true)
        {
            var next = _grid.Move(guardPosition, direction);
            if (next == null) break;

            if (next.Value == '#')
            {
                direction = direction.TurnRight();
            }
            else if (next.Value == '.')
            {
                visited.Add(next);
                guardPosition = next;
            }
        }

        return visited;
    }

    private List<Image> Images(GridCell<char> guardPosition, Direction direction)
    {
        var visited = new List<Image>();
        visited.Add(ToImage(guardPosition));
        while (true)
        {
            var next = _grid.Move(guardPosition, direction);
            if (next == null) break;

            if (next.Value == '#')
            {
                direction = direction.TurnRight();
            }
            else if (next.Value == '.')
            {
                visited.Add(ToImage(next));
                guardPosition = next;
            }

            if (visited.Count == 100) break;
        }

        return visited;

        Image ToImage(GridCell<char> gridCell)
        {
            return _grid.ToImage(cell =>
            {
                if (cell.Equals(gridCell)) return KnownColor.Red;
                return cell.Value switch
                {
                    '#' => KnownColor.Black,
                    '.' => KnownColor.White,
                    _ => KnownColor.Blue
                };
            });
        }
    }

    public override object Result2()
    {
        var visited = Visited(_guardPosition, _guardDirection);

        var loops = 0;
        var count = 0;

        var emptyCells = visited.Where(cell => !cell.Equals(_guardPosition) && cell.Value == '.').ToList();

        Parallel.ForEach(emptyCells, new ParallelOptions
        {
            MaxDegreeOfParallelism = 20
        }, (cell, s) =>
        {
            if (IsLoop(_grid, (_guardPosition, _guardDirection), cell)) Interlocked.Increment(ref loops);
            Interlocked.Increment(ref count);
            if (count % 100 == 0) Console.WriteLine($"{count / (float)emptyCells.Count:p} - {count} - loops: {loops}");
        });

        return loops;
    }

    private bool IsLoop(Grid<char> grid, (GridCell<char> cell, Direction dir) start, GridCell<char> block)
    {
        var guardPosition = start.cell.Clone();
        var direction = start.dir;

        var visited = new List<(GridCell<char>, Direction)>() { start };
        while (true)
        {
            var next = grid.Move(guardPosition, direction);
            if (next == null) return false;

            if (next.Value == '#' || next.Equals(block))
            {
                direction = direction.TurnRight();
            }
            else if (next.Value == '.')
            {
                var tuple = (next, direction);
                if (visited.Any(t => t.Item1 == next && t.Item2 == direction)) return true;
                visited.Add(tuple);
                guardPosition = next;
            }
        }
    }

    private Direction GetDirection(char c)
    {
        return c switch
        {
            '>' => Direction.Right,
            '<' => Direction.Left,
            '^' => Direction.Up,
            'v' => Direction.Down,
            _ => throw new Exception("Invalid direction")
        };
    }
}