using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AoC.Utils;

namespace AoC.y2021;

public class Day03 : Day
{
    private string _input;
    private List<string> _bitLst;
    private string gammaRt;
    private string epsilonRt;

    public Day03()
    {
        _input = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010".Replace("\r", "");
        _input = Input;
    }

    public override object Result1()
    {
        _bitLst = _input.AsListOf<string>();
        var len = _bitLst[0].Length;

        gammaRt = "";
        epsilonRt = "";

        var mid = _bitLst.Count / 2;

        for (int i = 0; i < len; i++)
        {
            var x = _bitLst.Count(s => s[i] == '1');
            if (x > mid)
            {
                gammaRt += "1";
                epsilonRt += "0";
            }
            else
            {
                epsilonRt += "1";
                gammaRt += "0";
            }
        }

        var consumptionRt = ConvertFromBinary(gammaRt) * ConvertFromBinary(epsilonRt);

        return consumptionRt;
    }

    int ConvertFromBinary(string binary)
    {
        return Convert.ToInt32(binary, 2);
    }

    public override object Result2()
    {
        var oxyGenRt = ConvertFromBinary(Calc(true));
        var co2Rt = ConvertFromBinary(Calc(false));
        return oxyGenRt * co2Rt;
    }

    string Calc(bool positive)
    {
        var list = _bitLst.ToList();

        char target = positive ? '1' : '0';

        for (int i = 0; i < _bitLst[0].Length; i++)
        {
            var count = list.Count(s => s[i] == target);

            var mid = list.Count/2f;
            if (positive ? count >= mid : count <= mid)
            {
                list.RemoveAll(s => s[i] != target);
            }
            else
            {
                list.RemoveAll(s => s[i] == target);
            }

            if (list.Count == 1) break;
        }

        return list[0];
    }
}