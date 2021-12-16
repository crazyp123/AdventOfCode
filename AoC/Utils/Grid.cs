using System;

namespace AoC.Utils
{
    public class GridCell<T>
    {
        public T Value { get; set; }
        public object Metadata { get; set; }

        public GridCell()
        {
        }

        public GridCell(T value)
        {
            Value = value;
        }
    }

    public class Grid<T>
    {
        public readonly GridCell<T>[,] Data;

        public int Width => Data.GetLength(0);
        public int Height => Data.GetLength(1);

        public Grid(int w, int h)
        {
            Data = new GridCell<T>[w, h];
            Data.Initialize();
        }

        public T Get(int x, int y)
        {
            return Data[x, y].Value;
        }

        public GridCell<T> GetCell(int x, int y)
        {
            return Data[x, y];
        }

        public void SetRow(int y, T[] row)
        {
            for (int x = 0; x < Width; x++)
            {
                Data[x, y] = new GridCell<T>(row[x]);
            }
        }

        public void SetRow(int y, T[] row, object[] metadata)
        {
            for (int x = 0; x < Width; x++)
            {
                Data[x, y] = new GridCell<T>(row[x])
                {
                    Metadata = metadata[x]
                };
            }
        }

        public void SetRow(int y, T[] row, object metadata)
        {
            for (int x = 0; x < Width; x++)
            {
                Data[x, y] = new GridCell<T>(row[x])
                {
                    Metadata = metadata
                };
            }
        }

        public GridCell<T>[] GetRow(int y)
        {
            var row = new GridCell<T>[Width];
            for (int x = 0; x < Width; x++)
            {
                row[x] = Data[x, y];
            }

            return row;
        }

        public GridCell<T>[] GetCol(int x)
        {
            var col = new GridCell<T>[Height];
            for (int y = 0; y < Height; y++)
            {
                col[y] = Data[x, y];
            }
            return col;
        }

        public GridCell<T>[][] GetRows()
        {
            var rows = new GridCell<T>[Height][];
            for (int i = 0; i < Height; i++)
            {
                rows[i] = GetRow(i);
            }
            return rows;
        }

        public GridCell<T>[][] GetCols()
        {
            var cols = new GridCell<T>[Width][];
            for (int i = 0; i < Width; i++)
            {
                cols[i] = GetCol(i);
            }
            return cols;
        }

        public void Set(int x, int y, T item)
        {
            Data[x, y].Value = item;
        }

        public void Set(int x, int y, T item, object metadata)
        {
            Data[x, y].Value = item;
            Data[x, y].Metadata = metadata;
        }

        public void Apply(Action<int, int, GridCell<T>> action)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    action(w, h, Data[w, h]);
                }
            }
        }

        public void Fill(Func<int, int, GridCell<T>> getItem)
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
                Data[newX, newY].Value = action(newX, newY, Data[newX, newY].Value);
            }
        }

    }
}