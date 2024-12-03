using System.Linq;
using System.Text.RegularExpressions;
using AoC.Utils;

namespace AoC.y2024;

public class Day03 : Day
{
    private string _test = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

    public override object Result1()
    {
        var x = Input;
        var regex = new Regex(@"(mul\([\d]+,[\d]+\))");
        var matches = regex.Matches(x);
        var ops = matches.Select(match => ParseMultiplyOp(match.Value)).ToList();
        return ops.Sum(o => o.Item1 * o.Item2);
    }

    private static (int, int) ParseMultiplyOp(string value)
    {
        var nums = new Regex(@"\(([\d]+),([\d]+)\)").Matches(value);
        return (nums[0].Groups[1].Value.AsInt(), nums[0].Groups[2].Value.AsInt());
    }

    public override object Result2()
    {
        var test = @"xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
        var regex = new Regex(@"(mul\([\d]+,[\d]+\))|(do\(\))|(don't\(\))");
        var matches = regex.Matches(Input);

        var sum = 0;
        var skip = false;
        foreach (Match match in matches)
        {
            if (match.Value.StartsWith("do()"))
            {
                skip = false;
                continue;
            }

            if (match.Value.StartsWith("don't()"))
            {
                skip = true;
                continue;
            }

            if (skip) continue;

            var op = ParseMultiplyOp(match.Value);
            sum += op.Item1 * op.Item2;
        }

        return sum;
    }
}