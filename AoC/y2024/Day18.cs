using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
using QuikGraph.Algorithms;

namespace AoC.y2024;

public class Day18 : Day
{
    private Grid<int> _grid;
    private List<(int, int)> _locations;
    private GridCell<int> _start;
    private GridCell<int> _end;

    public Day18()
    {
        _grid = new Grid<int>(71, 71);
        _locations = Input.AsListOfPatterns<int, int>("n,n");
        _start = _grid.GetCell(0, 0);
        _end = _grid.GetCell(_grid.Width - 1, _grid.Height - 1);
    }

    public override object Result1()
    {
        foreach (var (x, y) in _locations.Take(1024)) _grid.Set(x, y, 1);

        var graph = _grid.BuildAdjacencyGraph(c => c.GetNeighbors().Where(x => x.Value == 0));
        var start = _grid.GetCell(0, 0);
        var end = _grid.GetCell(_grid.Width - 1, _grid.Height - 1);
        var alg = graph.ShortestPathsDijkstra(e => 1, start);

        alg.Invoke(end, out var path);

        return path.Count();
    }

    public override object Result2()
    {
        for (var i = 0; i < _locations.Count; i++)
        {
            var (x, y) = _locations[i];
            _grid.Set(x, y, 1, i);
        }

        var locationsToVisit = _locations.Select((t, i) => (x: t.Item1, y: t.Item2, ix: i)).ToList();

        return Find(1023, locationsToVisit.Count - 1);
    }

    private string Find(int fromIx, int toIx)
    {
        var midIx = (toIx - fromIx) / 2 + fromIx;

        _grid.Apply(c => c.Value = 0);
        foreach (var t in _locations[..midIx]) _grid.Set(t.Item1, t.Item2, 1);

        var graph = _grid.BuildAdjacencyGraph(c => c.GetNeighbors().Where(x => x.Value == 0));
        var alg = graph.ShortestPathsDijkstra(e => 1, _start);
        if (alg.Invoke(_end, out var path))
            fromIx = midIx;
        else
            toIx = midIx;

        if (toIx - fromIx <= 1) return $"{_locations[fromIx].Item1},{_locations[fromIx].Item2}";

        return Find(fromIx, toIx);
    }
}