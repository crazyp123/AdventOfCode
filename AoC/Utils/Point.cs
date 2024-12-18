using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils;

public struct Point
{
    public int X;
    public int Y;
    public int Z;

    public Point(int x, int y) : this()
    {
        X = x;
        Y = y;
        Z = 0;
    }

    public Point(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public void Normalize()
    {
        X = Calculations.Clamp(X, -1, 1);
        Y = Calculations.Clamp(Y, -1, 1);
        Z = Calculations.Clamp(Z, -1, 1);
    }

    public bool Equals(Point other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object obj)
    {
        return obj is Point other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public static Point operator +(Point a, Point b)
    {
        return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Point operator -(Point a, Point b)
    {
        return new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Point operator *(Point a, int scalar)
    {
        return new Point(a.X * scalar, a.Y * scalar, a.Z * scalar);
    }

    public override string ToString()
    {
        return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Z)}: {Z}";
    }

    public Direction ToDirection()
    {
        return (X, Y) switch
        {
            (0, -1) => Direction.Up,
            (0, 1) => Direction.Down,
            (-1, 0) => Direction.Left,
            (1, 0) => Direction.Right,
            _ => default
        };
    }
}

public class Line
{
    public Point A { get; set; }
    public Point B { get; set; }

    public object Meta { get; set; }

    public Line(Point a, Point b)
    {
        A = a;
        B = b;
    }

    private Point GetDirection()
    {
        return B - A;
    }

    public Point[] GetPoints()
    {
        var lineDirection = GetDirection();
        lineDirection.Normalize();

        var points = new List<Point> { A };

        var distance = IsVertical() ? Math.Abs(A.Y - B.Y) : Math.Abs(A.X - B.X);
        for (var i = 1; i < distance; i++) points.Add(A + lineDirection * i);

        points.Add(B);

        return points.ToArray();
    }

    public bool IsVertical()
    {
        return A.X == B.X;
    }

    public bool IsHorizontal()
    {
        return A.Y == B.Y;
    }

    public bool IsDiagonal()
    {
        return Math.Abs(A.X - B.X) == Math.Abs(A.Y - B.Y);
    }

    public Line Join(Line other)
    {
        var newA = new[] { A, B }.Except(new[] { other.A, other.B }).FirstOrDefault();
        var newB = new[] { other.A, other.B }.Except(new[] { A, B }).FirstOrDefault();
        A = newA;
        B = newB;
        return this;
    }

    protected bool Equals(Line other)
    {
        return (A.Equals(other.A) && B.Equals(other.B)) || (A.Equals(other.B) && B.Equals(other.A));
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Line)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(A, B);
    }

    public override string ToString()
    {
        return $"{nameof(A)}: {A}, {nameof(B)}: {B}";
    }
}