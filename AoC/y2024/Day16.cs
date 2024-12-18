using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.ShortestPath;

namespace AoC.y2024;

public class Day16 : Day
{
    private Grid<char> _grid;

    public Day16()
    {
        var test = @"#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################".Replace("\r", "");
        _grid = Input.AsGrid();
        _grid.BottomLeftOrigin = true;
    }

    public override object Result1()
    {
        var start = _grid.Find('S');
        var end = _grid.Find('E');
        var graph = BuildGraph();

        var startT = (start, Direction.Right);
        var astar = graph.ShortestPathsAStar(e =>
        {
            var distance = e.Source.Item1.Distance(e.Target.Item1);
            if (e.Target.Item2 == e.Source.Item2) return distance;
            return 1000 + distance;
        }, e => e.Item1.Distance(end), startT);

        var hasPath2 = astar.Invoke((end, Direction.Up), out var path2);
        return GetCost(ToList(path2), startT);
    }

    public override object Result2()
    {
        var start = _grid.Find('S');
        var end = _grid.Find('E');
        var graph = BuildGraph();

        var startT = (start, Direction.Right);
        var endT = (end, Direction.Up);

        var visited = new HashSet<GridCell<char>>();
        visited.Add(start);
        visited.Add(end);

        var astar = GetAlg(graph, end, startT, visited);

        astar.Invoke(endT, out var path);
        var bestCost = GetCost(ToList(path), startT);
        path.ToList().ForEach(x => visited.Add(x.Target.Item1));

        var queue = new Stack<GridCell<char>>();
        foreach (var cell in visited) queue.Push(cell);

        while (queue.Any())
        {
            var cell = queue.Pop();
            astar = GetAlg(graph, end, startT, visited, cell);
            
            astar.Invoke(endT, out var path2);
            
            var cost = GetCost(ToList(path2), startT);
            if (cost == bestCost)
            {
                var newNodes = ToList(path2).Select(c => c.Item1).Except(visited).ToList();
                newNodes.ForEach(x => visited.Add(x));
                newNodes.ForEach(x => queue.Push(x));
            }
        }

        return visited.Count();
    }

    private static TryFunc<(GridCell<char>, Direction), IEnumerable<SEquatableEdge<(GridCell<char>, Direction)>>>
        GetAlg(AdjacencyGraph<(GridCell<char>, Direction), SEquatableEdge<(GridCell<char>, Direction)>> graph,
            GridCell<char> end, (GridCell<char> start, Direction Right) startT,
            HashSet<GridCell<char>> visited, GridCell<char> visitedCell = null)
    {
        var astar = graph.ShortestPathsAStar(e =>
        {
            var extraCost = visitedCell != null && visitedCell.Equals(e.Target.Item1) ? 1 : 0;
            var distance = e.Source.Item1.Distance(e.Target.Item1) + extraCost;
            if (e.Target.Item2 == e.Source.Item2) return distance;
            return 1000 + distance;
        }, e => e.Item1.Distance(end), startT);
        return astar;
    }

    private AdjacencyGraph<(GridCell<char>, Direction), SEquatableEdge<(GridCell<char>, Direction)>> BuildGraph()
    {
        var graph = new AdjacencyGraph<(GridCell<char>, Direction), SEquatableEdge<(GridCell<char>, Direction)>>();

        foreach (var cell in _grid.Cells)
        {
            if (cell.Value == '#') continue;

            foreach (var sd in DirectionUtils.Directions)
            {
                var neighbor = _grid.Move(cell, sd);
                if (neighbor is null || neighbor.Value == '#') continue;

                foreach (var sd2 in DirectionUtils.Directions)
                {
                    graph.AddVerticesAndEdge(new SEquatableEdge<(GridCell<char>, Direction)>((cell, sd), (cell, sd2)));
                    graph.AddVerticesAndEdge(new SEquatableEdge<(GridCell<char>, Direction)>((cell, sd2), (cell, sd)));
                }

                var source = (cell, sd);
                var target = (neighbor, sd);
                graph.AddVerticesAndEdge(new SEquatableEdge<(GridCell<char>, Direction)>(source, target));
            }
        }

        return graph;
    }

    private static List<(GridCell<char>, Direction)> ToList(
        IEnumerable<SEquatableEdge<(GridCell<char>, Direction)>> path)
    {
        return path.Select(e => e.Target).ToList();
    }

    private int GetCost(List<(GridCell<char>, Direction)> path, (GridCell<char>, Direction) start)
    {
        var cost = 0;

        var prev = start;
        foreach (var (cell, direction) in path)
        {
            if (prev.Item2 == direction) cost++;
            else cost += 1000;

            prev = (cell, direction);
        }

        return cost;
    }
}