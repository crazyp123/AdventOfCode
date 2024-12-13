using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day13 : Day
{
    public Day13()
    {
        var test = @"Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279".Replace("\r", "");
    }

    private List<ClawMachine> GetMachines(bool part2 = false)
    {
        var asListOfGroups = Input.AsListOfGroups<string>();
        return asListOfGroups
            .Select(list =>
            {
                var buttonA =
                    list[0].Replace("Button A: ", "").Replace("X+", "").Replace("Y+", "")
                        .ParsePattern<int, int>("n, n");
                var buttonB =
                    list[1].Replace("Button B: ", "").Replace("X+", "").Replace("Y+", "")
                        .ParsePattern<int, int>("n, n");
                (long, long) prizeLocation =
                    list[2].Replace("Prize: ", "").Replace("X=", "").Replace("Y=", "").ParsePattern<int, int>("n, n");

                if (part2) prizeLocation = (prizeLocation.Item1 + 10000000000000, prizeLocation.Item2 + 10000000000000);

                return new ClawMachine(buttonA, buttonB, prizeLocation);
            })
            .ToList();
    }

    public override object Result1()
    {
        return GetMachines().Sum(m => m.OptimizedCost());
    }

    public override object Result2()
    {
        return GetMachines(true).Sum(m => m.OptimizedCost());
    }

    private class ClawMachine((long x, long y) buttonA, (long x, long y) buttonB, (long x, long y) prize)
    {
        public (long x, long y) prize = prize;

        public long OptimizedCost()
        {
            // Solve the linear system:
            // a * Ax + b * Bx = PrizeX
            // a * Ay + b * By = PrizeY

            // Compute the determinant of the 2x2 matrix
            var d = buttonA.x * buttonB.y - buttonA.y * buttonB.x;

            // No solution exists if the determinant is zero (parallel vectors)
            if (d == 0) return 0;

            var scaledA = prize.x * buttonB.y - prize.y * buttonB.x;
            var scaledB = prize.y * buttonA.x - prize.x * buttonA.y;

            // Check if a and b are integers (must be divisible by determinant)
            if (scaledA % d != 0 || scaledB % d != 0) return 0;

            var a = scaledA / d;
            var b = scaledB / d;

            if (a < 0 || b < 0) return 0;

            var cost = a * 3 + b * 1;
            return cost;
        }
    }
}