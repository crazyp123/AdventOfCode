using System;

namespace AoC.Utils
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum DirectionDiagonal
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public static class DirectionUtils
    {
        public static readonly Direction[] Directions = new[]
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right
        };

        public static readonly DirectionDiagonal[] DirectionDiagonals = new[]
        {
            DirectionDiagonal.TopLeft,
            DirectionDiagonal.TopRight,
            DirectionDiagonal.BottomLeft,
            DirectionDiagonal.BottomRight
        };

        public static Direction ToDirection(this char c)
        {
            return ParseDir(c);
        }

        public static Direction ParseDir(char c)
        {
            switch (c)
            {
                case 'D':
                    return Direction.Down;
                case 'U':
                    return Direction.Up;
                case 'L':
                    return Direction.Left;
                case 'R':
                    return Direction.Right;
                default:
                    throw new NotImplementedException();
            }
        }

        public static (int x, int y) Apply(this (int, int) xy, Direction dir, int dist = 1)
        {
            return ApplyDir(xy.Item1, xy.Item2, dir, dist);
        }

        public static (int x, int y) ApplyDir(int x, int y, Direction dir, int dist = 1)
        {
            var newX = x;
            var newY = y;
            switch (dir)
            {
                case Direction.Up:
                    newY = y + dist;
                    break;
                case Direction.Down:
                    newY = y - dist;
                    break;
                case Direction.Left:
                    newX = x - dist;
                    break;
                case Direction.Right:
                    newX = x + dist;
                    break;
            }
            return (newX, newY);
        }

        public static (int x, int y) ApplyDir(int x, int y, DirectionDiagonal dir, int dist = 1)
        {
            var newX = x;
            var newY = y;
            switch (dir)
            {
                case DirectionDiagonal.TopLeft:
                    (newX, newY) = ApplyDir(newX, newY, Direction.Up, dist);
                    (newX, newY) = ApplyDir(newX, newY, Direction.Left, dist);
                    break;
                case DirectionDiagonal.TopRight:
                    (newX, newY) = ApplyDir(newX, newY, Direction.Up, dist);
                    (newX, newY) = ApplyDir(newX, newY, Direction.Right, dist);
                    break;
                case DirectionDiagonal.BottomLeft:
                    (newX, newY) = ApplyDir(newX, newY, Direction.Down, dist);
                    (newX, newY) = ApplyDir(newX, newY, Direction.Left, dist);
                    break;
                case DirectionDiagonal.BottomRight:
                    (newX, newY) = ApplyDir(newX, newY, Direction.Down, dist);
                    (newX, newY) = ApplyDir(newX, newY, Direction.Right, dist);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
            return (newX, newY);
        }
    }
}