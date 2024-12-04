using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day04 : Day
{
    private string _test;

    public Day04()
    {
        _test = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX".Replace("\r", "");
    }

    public override object Result1()
    {
        var grid = Input.AsListOf<string>().Select(s => s.ToList()).ToList().ToGrid();

        var words = 0;

        grid.Apply((x, y, cell) =>
        {
            if (cell.Value == 'X' || cell.Value == 'S')
            {
                var w1 = cell.Value.ToString();
                var w2 = cell.Value.ToString();
                var w3 = cell.Value.ToString();
                var w4 = cell.Value.ToString();

                for (var i = 1; i <= 3; i++)
                {
                    w1 += grid.GetNeighborValue(x, y, Direction.Right, i);
                    w2 += grid.GetNeighborValue(x, y, Direction.Down, i);
                    w3 += grid.GetNeighborValue(x, y, DirectionDiagonal.BottomRight, i);
                    w4 += grid.GetNeighborValue(x, y, DirectionDiagonal.BottomLeft, i);
                }

                words += new List<string> { w1, w2, w3, w4 }.Count(s => s == "XMAS" || s == "SAMX");
            }
        });
        return words;
    }

    public override object Result2()
    {
        var grid = Input.AsListOf<string>().Select(s => s.ToList()).ToList().ToGrid();

        var kernel = new Grid<char>(3, 3);
        kernel.SetRow(0, ['M', default, 'M']);
        kernel.SetRow(1, [default, 'A', default]);
        kernel.SetRow(2, ['S', default, 'S']);

        var kernel1 = new Grid<char>(3, 3);
        kernel1.SetRow(0, ['S', default, 'S']);
        kernel1.SetRow(1, [default, 'A', default]);
        kernel1.SetRow(2, ['M', default, 'M']);

        var kernel2 = new Grid<char>(3, 3);
        kernel2.SetRow(0, ['M', default, 'S']);
        kernel2.SetRow(1, [default, 'A', default]);
        kernel2.SetRow(2, ['M', default, 'S']);

        var kernel3 = new Grid<char>(3, 3);
        kernel3.SetRow(0, ['S', default, 'M']);
        kernel3.SetRow(1, [default, 'A', default]);
        kernel3.SetRow(2, ['S', default, 'M']);

        // 40151 too high
        // 1980 too high
        // 996 too low
        return grid.MatchKernels(kernel).Count +
               grid.MatchKernels(kernel1).Count +
               grid.MatchKernels(kernel2).Count +
               grid.MatchKernels(kernel3).Count;
    }
}