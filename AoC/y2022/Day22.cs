using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022
{
    public class Day22 : Day
    {
        private Grid<string> _grid;
        private List<object> _instr;

        public Day22()
        {
            // test input
            var input = @"        ...#
        .#..
        #...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5";

            //  input = Input;


            var split = input.Split("\r\n\r");
            var lines = split[0].Split("\n");

            _grid = new Grid<string>(lines[0].Trim('\r').Length, lines.Length);

            _grid.Apply(cell => cell.Value = lines[cell.Y].Trim('\r')[cell.X].ToString());

            _instr = new List<object>();

            var prevNum = "";
            var trim = split[1].Trim();
            for (var i = 0; i < trim.Length; i++)
            {
                var c = trim[i];
                if (char.IsNumber(c))
                {
                    prevNum += c;
                }
                else
                {
                    if (int.TryParse(prevNum, out var num)) _instr.Add(num);
                    _instr.Add(c);

                    prevNum = "";
                }

                if (i == trim.Length - 1 && prevNum != "")
                {
                    _instr.Add(prevNum.AsInt());
                }
            }
        }

        public override object Result1()
        {
            var dir = Direction.Right;
            var pos = _grid.GetRow(0).First(c => c.Value == ".");

            foreach (var o in _instr)
            {
                switch (o)
                {
                    case char nextDir:
                        dir = nextDir switch
                        {
                            'L' => dir == Direction.Right ? Direction.Up :
                                dir == Direction.Up ? Direction.Left :
                                dir == Direction.Left ? Direction.Down : Direction.Right,
                            'R' => dir == Direction.Right ? Direction.Down :
                                dir == Direction.Up ? Direction.Right :
                                dir == Direction.Left ? Direction.Up : Direction.Left,
                        };
                        break;
                    case int steps:
                        {
                            for (int i = 0; i < steps; i++)
                            {
                                var next = _grid.Move(pos, dir);

                                if (IsWall(next))
                                {
                                    next = dir switch
                                    {
                                        Direction.Up => _grid.GetCol(pos.X).OrderByDescending(c => c.Y)
                                            .First(c => !IsWall(c)),
                                        Direction.Down => _grid.GetCol(pos.X).OrderBy(c => c.Y)
                                            .First(c => !IsWall(c)),
                                        Direction.Left => _grid.GetRow(pos.Y).OrderByDescending(c => c.X)
                                            .First(c => !IsWall(c)),
                                        Direction.Right => _grid.GetRow(pos.Y).OrderBy(c => c.X)
                                            .First(c => !IsWall(c))
                                    };
                                }

                                pos = next;
                            }
                            break;
                        }
                }
            }

            var z = dir switch
            {
                Direction.Up => 3,
                Direction.Down => 1,
                Direction.Left => 2,
                Direction.Right => 0,
            };

            return 1000 * (pos.Y + 1) + 4 * (pos.X + 1) + z;
        }

        public override object Result2()
        {
            throw new System.NotImplementedException();
        }

        bool IsWall(GridCell<string> cell)
        {
            return (cell == null || cell.Value == " " || cell.Value == "#");
        }
    }
}