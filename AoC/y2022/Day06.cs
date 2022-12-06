using System.Linq;
using AoC.Utils;

namespace AoC.y2022;

public class Day06 : Day
{

    public override object Result1()
    {
        return FindMarker(4);
    }

    public override object Result2()
    {
        return FindMarker(14);
    }

    private object FindMarker(int x)
    {
        for (int i = 0; i < Input.Length - x; i++)
        {
            if (Input[i..(i + x)].Distinct().Count() == x)
            {
                return i + x;
            }
        }

        return null;
    }
}