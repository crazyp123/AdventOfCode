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
            var result = long.Parse(parts[0]);
            var numbers = parts[1].AsListOf<long>(" ");
            return new Equation(numbers, result);
        }).ToList();
    }

    private class Equation
    {
        public long[] Numbers;
        public long Result;

        public Equation(IEnumerable<long> numbers, long result)
        {
            Numbers = numbers.ToArray();
            Result = result;
        }

        public bool Test(bool withCombine = false)
        {
            return GetResultsRec(Numbers[0], Numbers[1..], withCombine);
        }

        private bool GetResultsRec(long num, long[] nums, bool withCombine = false)
        {
            var sum = num + nums[0];
            var product = num * nums[0];
            var combine = $"{num}{nums[0]}".AsLong();
            if (nums.Length == 1) return sum == Result || product == Result || (withCombine && combine == Result);

            return GetResultsRec(sum, nums[1..], withCombine) || GetResultsRec(product, nums[1..], withCombine) ||
                   (withCombine && GetResultsRec(combine, nums[1..], true));
        }
    }

    public override object Result1()
    {
        return _equations.Where(equation => equation.Test()).Sum(e => e.Result);
    }

    public override object Result2()
    {
        return _equations.Where(equation => equation.Test(true)).Sum(e => e.Result);
    }
}