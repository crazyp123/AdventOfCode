using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
using QuikGraph;
using QuikGraph.Algorithms;

namespace AoC.y2024;

public class Day10 : Day
{
    private AdjacencyGraph<GridCell<int>, SEquatableEdge<GridCell<int>>> _graph;
    private List<GridCell<int>> _trailHeads;
    private List<GridCell<int>> _trailEnds;

    public Day10()
    {
        var test = @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732".Replace("\r", "");
        var map = Input.AsListOf<string>().Select(s => s.Select(c => int.Parse(c.ToString())).ToList()).ToList()
            .ToGrid();
        _graph = map.BuildAdjacencyGraph(cell => cell.GetNeighbors().Where(x => x.Value - cell.Value == 1));

        _trailHeads = map.Cells.Where(c => c.Value == 0).ToList();
        _trailEnds = map.Cells.Where(c => c.Value == 9).ToList();
    }

    public override object Result1()
    {
        return _trailHeads.Sum(PathScore);
    }

    public override object Result2()
    {
        return _trailHeads.Sum(PathRating);
    }

    private int PathScore(GridCell<int> start)
    {
        return _trailEnds.Sum(end =>
        {
            var bfs = _graph.TreeBreadthFirstSearch(start);
            return bfs.Invoke(end, out var path) ? 1 : 0;
        });
    }

    private int PathRating(GridCell<int> start)
    {
        return _trailEnds.Sum(end => _graph.FindAllPaths(start, end).Count);
    }
}