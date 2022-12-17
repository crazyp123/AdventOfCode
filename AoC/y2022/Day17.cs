using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using AoC.Utils;

namespace AoC.y2022
{
    public class Day17 : Day
    {
        private List<Direction> _jets;
        private List<Grid<string>> _shapes;
        private World2D<string> _world;

        public Day17()
        {
            // test input
            var input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

            // input = Input;

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

            _jets = input.Trim().Select(c => c == '<' ? Direction.Left : Direction.Right).ToList();


            _world = new World2D<string>(7, 2022 * 4);
        }

        Grid<string> ParseShape(string shape)
        {
            var grid = new Grid<string>(shape.Trim().AsListOf<string>().Select(s => s.Trim().ToCharArray().Select(c => c.ToString()).ToList()).ToList());
            grid.Apply(c => c.Value = c.Value == "." ? null : c.Value);
            return grid;
        }

        public override object Result1()
        {
            return GetHeight(2022);
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

    }
}