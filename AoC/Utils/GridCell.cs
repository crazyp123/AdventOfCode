using System;
using System.Collections.Generic;

namespace AoC.Utils;

public class GridCell<T>
{
    public Grid<T> Grid { get; }
    public int X { get; }
    public int Y { get; }

    public T Value { get; set; }
    public object Metadata { get; set; }

    public GridCell(Grid<T> grid, int x, int y)
    {
        Grid = grid;
        X = x;
        Y = y;
    }

    public GridCell(T value, Grid<T> grid, int x, int y)
    {
        Value = value;
        Grid = grid;
        X = x;
        Y = y;
    }

    public GridCell(Grid<T> grid, int x, int y, T value, object metadata)
    {
        Grid = grid;
        X = x;
        Y = y;
        Value = value;
        Metadata = metadata;
    }

    public Point ToPoint()
    {
        return new Point(X, Y);
    }

    public GridCell<T> Clone()
    {
        return new GridCell<T>(Grid, X, Y, Value, Metadata);
    }

    public GridCell<T>[] GetNeighbors(int dist = 1)
    {
        return Grid.GetNeighborCells(X, Y, dist);
    }

    public GridCell<T>[] GetAllNeighbors(int dist = 1)
    {
        return Grid.GetAllNeighborCells(X, Y, dist);
    }

    public GridCell<T>[] GetDiagonalNeighbors(int dist = 1)
    {
        return Grid.GetDiagonalNeighborCells(X, Y, dist);
    }

    public List<GridCell<T>> Flood(Func<GridCell<T>, bool> condition)
    {
        return Grid.Flood(X, Y, condition);
    }

    public bool IsOnEdge()
    {
        return X == 0 || Y == 0 || X == Grid.Width - 1 || Y == Grid.Height - 1;
    }

    public int Distance(GridCell<T> other)
    {
        return Calculations.ManhattanDistance(X, Y, other.X, other.Y);
    }

    public bool Equals(GridCell<T> other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((GridCell<T>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Value)}: {Value}, {nameof(Metadata)}: {Metadata}";
    }
}