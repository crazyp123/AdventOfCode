using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day07 : Day
{
    private List<Equation> _equations;

    public Day07()
    {
        var test = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20".Replace("\r", "");
        _equations = Input.AsListOf<string>().Select(s =>
        {
            var parts = s.Split(":", StringSplitOptions.TrimEntries);
            var result = ulong.Parse(parts[0]);
            var numbers = parts[1].AsListOf<ulong>(" ");
            return new Equation(numbers, result);
        }).ToList();

    }

    private class Equation
    {
        public ulong[] Numbers;
        public ulong Result;

        public Equation(IEnumerable<ulong> numbers, ulong result)
        {
            Numbers = numbers.ToArray();
            Result = result;
        }

        public bool Test(bool withCombine = false)
        {
            if (withCombine) return GetResultsRecWithCombine(Numbers[0], Numbers[1..]);
            return GetResultsRec(Numbers[0], Numbers[1..]);
        }

        private bool GetResultsRec(ulong num, ulong[] nums)
        {
            var sum = num + nums[0];
            var product = num * nums[0];

            if (nums.Length == 1) return sum == Result || product == Result;

            return GetResultsRec(sum, nums[1..]) || GetResultsRec(product, nums[1..]);
        }

        private bool GetResultsRecWithCombine(ulong num, ulong[] nums)
        {
            var sum = num + nums[0];
            var product = num * nums[0];
            var combine = $"{num}{nums[0]}".AsULong();
            if (nums.Length == 1) return sum == Result || product == Result || combine == Result;

            return GetResultsRecWithCombine(sum, nums[1..]) || GetResultsRecWithCombine(product, nums[1..]) ||
                   GetResultsRecWithCombine(combine, nums[1..]);
        }
    }

    public override object Result1()
    {
        
        return Sum(_equations.Where(equation => equation.Test()));
    }

    public override object Result2()
    {
        return Sum(_equations.Where(equation => equation.Test(true)));    }

    private ulong Sum(IEnumerable<Equation> list)
    {
        ulong res = 0;
        foreach (var i in list)
        {
            res += i.Result;
        }

        return res;
    }
}