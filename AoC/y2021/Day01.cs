using System;
using System.Linq;
using AoC.Utils;

namespace AoC.y2021
{
    public class Day01 : Day
    {
        public override object Result1()
        {
            var nums = Input.AsListOf<int>();
            return nums.Zip(nums.Skip(1)).Count(t => t.Second > t.First);
        }

        public override object Result2()
        {
            var nums = Input.AsListOf<int>();
            var winz = new System.Collections.Generic.List<int>();
            for (int i = 0; i < nums.Count-2; i++)
            {
                var x = nums[i];
                var y = nums[i + 1];
                var z = nums[i + 2];
                var t = x + y + z;
                winz.Add(t);
            }
            return winz.Zip(winz.Skip(1)).Count(t => t.Second > t.First);
        }
    }
}
