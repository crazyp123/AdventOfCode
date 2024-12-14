using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day14 : Day
{
    private List<((int x, int y) p, (int x, int y) v)> _robots;
    private int width = 101;
    private int height = 103;

    public Day14()
    {
        _robots = Input.AsListOf<string>().Select(s =>
        {
            var parts = s.Split(" ");
            var p = parts[0].Replace("p=", "").Split(",").Select(int.Parse).ToList();
            var v = parts[1].Replace("v=", "").Split(",").Select(int.Parse).ToList();
            return (p: (x: p[0], y: p[1]), v: (x: v[0], y: v[1]));
        }).ToList();
    }

    public override object Result1()
    {
        int q1 = 0, q2 = 0, q3 = 0, q4 = 0;
        foreach (var (p, v) in _robots)
        {
            var x = Mod(p.x + v.x * 100, width);
            var y = Mod(p.y + v.y * 100, height);

            if (x > width / 2 && y > height / 2) q1++;
            if (x < width / 2 && y > height / 2) q2++;
            if (x < width / 2 && y < height / 2) q3++;
            if (x > width / 2 && y < height / 2) q4++;
        }

        return q1 * q2 * q3 * q4;
    }

    public override object Result2()
    {
        var grid = new Grid<int>(width, height);
        var ticks = 1;

        while (true)
        {
            grid.Apply(c => c.Value = 0);
            foreach (var (p, v) in _robots)
            {
                var x = Mod(p.x + v.x * ticks, width);
                var y = Mod(p.y + v.y * ticks, height);
                grid.Set(x, y, grid.GetValue(x, y) + 1);
            }

            if (grid.Cells.Any(c =>
                {
                    if (c.Value == 0) return false;
                    var flood = grid.Flood(c, o => o.Value > 0);
                    return flood.Count > 50;
                }))
            {
                Console.WriteLine(grid.Print(c => c.Value > 0 ? "#" : "."));
                Console.WriteLine($"Ticks {ticks} - Can you see a Christmas tree? (Press Space if yes)");
                if (Console.ReadKey().Key == ConsoleKey.Spacebar) break;
            }

            ticks++;
        }

        return ticks;
    }


    private int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }
}