using System.Collections.Generic;
using QuikGraph;

namespace AoC.Utils;

public static class GraphExtensions
{
    /// <summary>
    /// Finds all possible paths from source to target in a graph using DFS.
    /// </summary>
    public static List<List<TVertex>> FindAllPaths<TVertex, TEdge>(
        this IVertexAndEdgeListGraph<TVertex, TEdge> graph,
        TVertex source,
        TVertex target)
        where TEdge : IEdge<TVertex>
    {
        var allPaths = new List<List<TVertex>>();
        var currentPath = new List<TVertex>();
        var visited = new HashSet<TVertex>();

        void DFS(TVertex current)
        {
            currentPath.Add(current);
            visited.Add(current);

            if (current.Equals(target))
                allPaths.Add(new List<TVertex>(currentPath));
            else
                foreach (var edge in graph.OutEdges(current))
                    if (!visited.Contains(edge.Target))
                        DFS(edge.Target);

            currentPath.RemoveAt(currentPath.Count - 1);
            visited.Remove(current);
        }

        DFS(source);
        return allPaths;
    }
}