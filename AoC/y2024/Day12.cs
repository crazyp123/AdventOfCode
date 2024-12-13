using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AoC.Utils;
using Spectre.Console;
using Color = System.Drawing.Color;
using Point = AoC.Utils.Point;

namespace AoC.y2024;

public class Day12 : Day
{
    private Grid<char> _grid;
    private List<GardenRegion> _regions;

    public Day12()
    {
        var test = @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE".Replace("\r", ""); // part 2 = 1206

        var test2 = @"AAAA
BBCD
BBCC
EEEC".Replace("\r", ""); // part 2 = 80


        var test3 = @"OOOOO
OXOXO
OOOOO
OXOXO
OOOOO".Replace("\r", ""); // part 2 = 436

        var test4 = @"EEEEE
EXXXX
EEEEE
EXXXX
EEEEE".Replace("\r", ""); // part 2 = 236

        var test5 = @"AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA".Replace("\r", ""); // part 2 = 368

        _grid = Input.AsGrid();
        _grid.Apply(c => c.Metadata = new GardenPlot());
        _regions = GetRegions();
    }


    public override object Result1()
    {
        _grid.ToImage(c => (c.Metadata as GardenPlot).Color).Save("C:\\SourceCode\\garden.png");

        return _regions.Sum(r => r.GetPrice());
    }

    public static KnownColor RandomColor()
    {
        var randomGen = new Random();
        var names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
        return names[randomGen.Next(names.Length)];
    }

    public override object Result2()
    {
        return _regions.Sum(r => r.GetDiscountedPrice());
    }

    private List<GardenRegion> GetRegions()
    {
        var regions = new List<GardenRegion>();
        var toVisit = _grid.Cells.ToList();
        while (toVisit.Count > 0)
        {
            var first = toVisit.First();
            var region = _grid.Flood(first, cell =>
            {
                if (cell.Metadata is GardenPlot { Visited: true } plot) return false;
                return cell.Value == first.Value;
            });
            region.ForEach(c => { ((GardenPlot)c.Metadata).Visited = true; });
            toVisit.RemoveAll(c => region.Contains(c));
            regions.Add(new GardenRegion(region));
        }

        return regions;
    }

    private class GardenPlot
    {
        public bool Visited { get; set; }
        public KnownColor Color { get; set; }
    }

    private class GardenRegion
    {
        public List<GridCell<char>> Plots { get; set; }

        public KnownColor color = RandomColor();

        public GardenRegion(List<GridCell<char>> plots)
        {
            Plots = plots;

            Plots.ForEach(c => { ((GardenPlot)c.Metadata).Color = color; });
        }

        public int GetPrice()
        {
            var area = Plots.Count;
            return area * Plots.Sum(cell =>
            {
                var neighbors = cell.GetNeighbors();
                var edgeLines = 4 - neighbors.Length;

                return neighbors.Count(n => !Plots.Contains(n)) + edgeLines;
            });
        }

        public int GetDiscountedPrice()
        {
            var polygonLines = GetPolygonLines(Plots);
            return Plots.Count * polygonLines.Count;
        }
    }


    public static List<Line> GetPolygonLines<T>(List<GridCell<T>> cells)
    {
        var lines = new List<Line>();

        foreach (var cell in cells)
        foreach (var direction in DirectionUtils.Directions)
        {
            var neighbor = cell.Grid.GetNeighborCell(cell, direction);
            if (neighbor is not null && cells.Contains(neighbor)) continue;

            var line = direction switch
            {
                Direction.Up => new Line(new Point(cell.X, cell.Y + 1), new Point(cell.X + 1, cell.Y + 1)),
                Direction.Down => new Line(cell.ToPoint(), new Point(cell.X + 1, cell.Y)),
                Direction.Left => new Line(cell.ToPoint(), new Point(cell.X, cell.Y + 1)),
                Direction.Right => new Line(new Point(cell.X + 1, cell.Y), new Point(cell.X + 1, cell.Y + 1)),
                _ => default
            };

            line.Meta = direction;

            var existingLines = lines.Where(l => CanJoin(l, line, cells)).ToList();
            lines.RemoveAll(l => existingLines.Contains(l));

            existingLines.ForEach(x => line!.Join(x));

            lines.Add(line);
        }

        return lines;
    }

    public static bool CanJoin<T>(Line line, Line other, List<GridCell<T>> cells)
    {
        var points = new[] { line.A, line.B, other.A, other.B };
        var samePoint = points.Distinct().Count() == 3;
        var sameOrientation =
            (line.IsVertical() && other.IsVertical()) || (line.IsHorizontal() && other.IsHorizontal());

        if (!samePoint || !sameOrientation) return false;

        var sameSide = line.Meta != null && other.Meta != null && (Direction)line.Meta == (Direction)other.Meta;
        return sameSide;
    }
}