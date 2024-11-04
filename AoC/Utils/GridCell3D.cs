using System;

namespace AoC.Utils;

public class GridCell3D<T>
{
    public Grid3D<T> Grid { get; }
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public T Value { get; set; }
    public object Metadata { get; set; }

    public GridCell3D(Grid3D<T> grid, int x, int y, int z)
    {
        Grid = grid;
        X = x;
        Y = y;
        Z = z;
    }

    public GridCell3D(T value, Grid3D<T> grid, int x, int y, int z)
    {
        Value = value;
        Grid = grid;
        X = x;
        Y = y;
        Z = z;
    }

    public GridCell3D(Grid3D<T> grid, int x, int y, int z, T value, object metadata)
    {
        Grid = grid;
        X = x;
        Y = y;
        Z = z;
        Value = value;
        Metadata = metadata;
    }

    public GridCell3D<T>[] GetNeighbors(int dist = 1)
    {
        return Grid.GetNeighborCells(X, Y, Z, dist);
    }

    public GridCell3D<T> GetNeighbor(Direction3D dir, int dist = 1)
    {
        return Grid.GetNeighborCell(this, dir, dist);
    }

    //public GridCell<T>[] GetAllNeighbors(int dist = 1)
    //{
    //    return Grid.GetAllNeighborCells(X, Y, dist);
    //}

    //public GridCell<T>[] GetDiagonalNeighbors(int dist = 1)
    //{
    //    return Grid.GetDiagonalNeighborCells(X, Y, dist);
    //}


    public bool IsOnEdge()
    {
        return X == 0 || Y == 0 || X == Grid.Width - 1 || Y == Grid.Height - 1;
    }

    public int Distance(GridCell3D<T> other)
    {
        return Calculations.ManhattanDistance(X, Y, other.X, other.Y);
    }

    protected bool Equals(GridCell3D<T> other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GridCell3D<T>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Z)}: {Z}, {nameof(Value)}: {Value}, {nameof(Metadata)}: {Metadata}";
    }
}