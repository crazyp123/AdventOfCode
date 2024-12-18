using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day15 : Day
{
    private Grid<char> _grid;
    private List<Direction> _sequence;

    public Day15()
    {
        ReadGrid();
    }

    private void ReadGrid(bool part2 = false)
    {
        var test = @"#######
#...#.#
#.....#
#..OO@#
#..O..#
#.....#
#######

<vv<<^^<<^^".Replace("\r", "");

        var test2 = @"##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^".Replace("\r", "");

        var parts = Input.Split("\n\n", StringSplitOptions.TrimEntries);
        var grid = parts[0];
        if (part2) grid = grid.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.");
        _grid = grid.AsGrid();
        _grid.BottomLeftOrigin = true;
        _sequence = parts[1].Replace("\n", "").Select(DirectionUtils.ParseDir).ToList();
    }

    public override object Result1()
    {
        var robot = _grid.Find('@');

        foreach (var direction in _sequence) MovePosition(ref robot, direction);

        return _grid.Cells.Where(cell => cell.Value == 'O').Sum(cell => 100 * cell.Y + cell.X);
    }

    private bool MovePosition(ref GridCell<char> item, Direction direction)
    {
        var next = _grid.Move(item, direction);
        switch (next.Value)
        {
            case '#':
                return false;
            case '.':
                _grid.SwapValue(item, next);
                if (next.Value == '@') item = next;
                return true;
            case 'O':
                if (MovePosition(ref next, direction))
                {
                    _grid.SwapValue(item, next);
                    if (next.Value == '@') item = next;
                    return true;
                }

                break;
            case '[':
            case ']':
                if (!CanMove(next, direction)) return false;

                var crateOtherHalf = direction is Direction.Up or Direction.Down
                    ? _grid.Move(next, next.Value == '[' ? Direction.Right : Direction.Left)
                    : null;

                if (crateOtherHalf != null && !CanMove(crateOtherHalf, direction)) return false;

                MovePosition(ref next, direction);
                if (crateOtherHalf != null) MovePosition(ref crateOtherHalf, direction);

                _grid.SwapValue(item, next);
                if (next.Value == '@') item = next;
                return true;
        }

        return false;
    }

    private bool CanMove(GridCell<char> cell, Direction direction)
    {
        var next = _grid.Move(cell, direction);
        switch (next.Value)
        {
            case '#':
                return false;
            case '.':
                return true;
            case 'O':
                return CanMove(next, direction);
            case '[':
            case ']':
                var canMove = CanMove(next, direction);
                if (direction is not (Direction.Up or Direction.Down)) return canMove;

                var crateOtherHalf = _grid.Move(next, next.Value == '[' ? Direction.Right : Direction.Left);
                canMove &= CanMove(crateOtherHalf, direction);

                return canMove;
        }

        return false;
    }

    public override object Result2()
    {
        ReadGrid(true);

        var images = new List<Image>();

        var robot = _grid.Find('@');

        foreach (var direction in _sequence)
            //   images.Add(_grid.ToImage(cell => (KnownColor.White, cell.Value.ToString())));
            MovePosition(ref robot, direction);

        //   ImageUtils.CreateGif("C:\\SourceCode\\day15.gif", images);

        var outliers = _grid.Cells.Where(cell => cell.Value == '[' && _grid.Move(cell, Direction.Right).Value != ']')
            .ToList();


        return _grid.Cells.Where(cell => cell.Value == '[').Sum(cell => 100 * cell.Y + cell.X);
    }
}