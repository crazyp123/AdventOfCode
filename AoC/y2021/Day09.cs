using AoC.Objects;
using AoC.Utils;

namespace AoC.y2021
{
    public class Day09 : Day
    {
        public Day09()
        {
            // testInput
            var input = "";

            input = Input;
        }

        public override object Result1()
        {
            return new SevenSegmentNum().GetAsciiNum(3);
        }

        public override object Result2()
        {
            throw new System.NotImplementedException();
        }
    }
}