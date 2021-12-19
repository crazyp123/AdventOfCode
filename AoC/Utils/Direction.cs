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

    public static class DirectionUtils
    {
        public static readonly Direction[] Directions = new[]
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right
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
    }
}