using System;
using AoC.Utils;

namespace AoC.y2020
{
    public class Day01 : Day
    {
        public Day01()
        {
        }

        public override object Result1()
        {
            var i = Input.AsListOf<int>();
            foreach (var i1 in i)
            {
                foreach (var i2 in i)
                {
                    if (i1 + i2 == 2020)
                    {
                        return i1* i2;
                    }
                }
            }

            return null;
        }

        public override object Result2()
        {
            var i = Input.AsListOf<int>();
            foreach (var i1 in i)
            {
                foreach (var i2 in i)
                {
                    foreach (var i3 in i)
                    {
                        if (i1 + i2 + i3 == 2020)
                        {
                            return i1 * i2 * i3;
                        }
                    }

                }
            }
            return null;
        }
    }
}