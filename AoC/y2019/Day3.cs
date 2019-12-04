using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using AdventOfCode;

namespace AoC.y2019
{
    public class Day3 : Day
    {
        private List<string[]> lines;
        public Day3()
        {
            lines = Input.AsListOf<string>().Select(s => s.Split(',')).ToList();
        }

        private List<List<(int, int)>> Positions()
        {
            var positions = new List<List<(int, int)>>();

            foreach (var line in lines)
            {
                var oX = 0;
                var oY = 0;
                var line1 = new List<(int, int)>();
                foreach (var command in line)
                {
                    var dir = ParseDir(command.First());
                    var distance = int.Parse(command.Remove(0, 1));

                    for (int i = 0; i < distance; i++)
                    {
                        var (x, y) = ApplyDir(oX, oY, dir);
                        line1.Add((x, y));
                        oX = x;
                        oY = y;
                    }
                }

                positions.Add(line1);
            }

            return positions;
        }

        public override object Result1()
        {
            var positions = Positions();
            var intersections = positions[0].Intersect(positions[1]).ToList();
            return intersections.Select(tuple => Utils.ManhattanDistance(0, 0, tuple.Item1, tuple.Item2)).Min();
        }

        public override object Result2()
        {
            var positions = Positions();
            var intersections = positions[0].Intersect(positions[1]).ToList();


            List<int> sums = new List<int>();

            foreach (var intersection in intersections)
            {
                var one = positions[0].IndexOf(intersection) + 1;
                var two = positions[1].IndexOf(intersection) + 1;

                sums.Add(one + two);
            }

            return sums.Min();
        }

        Direction ParseDir(char c)
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

        (int, int) ApplyDir(int x, int y, Direction dir, int dist = 1)
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

    public class FlexGrid<T>
    {
        readonly List<List<T>> Data;

        private List<int> rows;
        private List<int> colMap;

    }

    public class Grid<T>
    {
        public readonly T[,] Data;

        public int Width => Data.GetLength(0);
        public int Height => Data.GetLength(1);

        public Grid(int w, int h)
        {
            Data = new T[w, h];
        }

        public T Get(int x, int y)
        {
            return Data[x, y];
        }

        public void Set(int x, int y, T item)
        {
            Data[x, y] = item;
        }

        public void Apply(Action<int, int, T> action)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    action(w, h, Data[w, h]);
                }
            }
        }

        public void Fill(Func<int, int, T> getItem)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    Data[w, h] = getItem(w, h);
                }
            }
        }

        public void MoveSet(int x, int y, Direction dir, int distance, Func<int, int, T, T> action)
        {
            for (int i = 1; i <= distance; i++)
            {
                var newX = x;
                var newY = y;
                switch (dir)
                {
                    case Direction.Up:
                        newY = y + 1;
                        break;
                    case Direction.Down:
                        newY = y - 1;
                        break;
                    case Direction.Left:
                        newX = x - 1;
                        break;
                    case Direction.Right:
                        newX = x + 1;
                        break;
                }
                Data[newX, newY] = action(newX, newY, Data[newX, newY]);
            }
        }

    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}