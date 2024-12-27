using System;
using System.Drawing;
using System.Linq;
using AoC.Utils;
using QuikGraph.Algorithms;

namespace AoC.y2024;

public class Day20 : Day
{
    public override object Result1()
    {
        var grid = Input.AsGrid();
        var graph = grid.BuildAdjacencyGraph(c => c.GetNeighbors().Where(x => x.Value != '#'));
        var start = grid.Find('S');
        var end = grid.Find('E');

        var invoke = graph.ShortestPathsDijkstra(e => 1, start).Invoke(end, out var path);

        grid.ToImage(c =>
        {
            if (path.Any(e => e.Target.Value == '.' && e.Target.Equals(c))) return KnownColor.LightGreen;
            return c.Value switch
            {
                '#' => KnownColor.Black,
                'S' => KnownColor.Blue,
                'E' => KnownColor.Red,
                _ => KnownColor.White
            };
        }, 50).Save("C:\\SourceCode\\day20.png");


        return invoke ? path.Count() : -1;
    }

    public override object Result2()
    {
        throw new NotImplementedException();
    }
}