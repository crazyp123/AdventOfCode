using System;
using AoC.Utils;

namespace AoC.y2019
{
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
}