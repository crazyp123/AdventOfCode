using System.Collections.Generic;
using AoC.Utils;

namespace AoC.y2020
{
    public class Day03 : Day
    {
        private List<string> _map;

        public Day03()
        {
            _map = Input.AsListOf<string>();
        }

        public override object Result1()
        {
            return CountTrees(3,1);
        }

        public override object Result2()
        {
            return
                CountTrees(1, 1) *
                CountTrees(3, 1) *
                CountTrees(5, 1) *
                CountTrees(7, 1) *
                CountTrees(1, 2);
        }

        private long CountTrees(int right, int down)
        {
            
            var trees = 0L;

            var row = 1;
            var col = 1;

            var done = false;

            while (!done)
            {
                col += right;
                row += down;

                if (_map[row - 1][(col - 1) % _map[0].Length] == '#')
                {
                    trees++;
                }

                done = row == _map.Count;
            }


            return trees;
        }


    }
}