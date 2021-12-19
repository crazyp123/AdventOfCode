using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils
{
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

        public GridCell<T>[] GetNeighbors(int dist = 1)
        {
            return Grid.GetNeighborCells(X, Y, dist);
        }

        public List<GridCell<T>> Flood(Func<GridCell<T>, bool> condition)
        {
            return Grid.Flood(X, Y, condition);
        }

        protected bool Equals(GridCell<T> other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GridCell<T>)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
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

        public Grid(List<List<T>> data)
        {
            var height = data.Count;
            var width = data[0].Count;
            Data = new GridCell<T>[width, height];
            SetRows(data);
        }

        public GridCell<T> GetCell(int x, int y)
        {
            if (x >= Width || y >= Height || x < 0 || y < 0) return null;
            return Data[x, y];
        }

        public T GetValue(int x, int y)
        {
            return GetCell(x, y).Value;
        }

        public T GetNeighborValue(int x, int y, Direction dir, int step = 1)
        {
            return GetNeighborCell(x, y, dir, step).Value;
        }

        public GridCell<T> GetNeighborCell(int x, int y, Direction dir, int step = 1)
        {
            var pos = DirectionUtils.ApplyDir(x, y, dir, step);
            return GetCell(pos.x, pos.y);
        }

        public GridCell<T>[] GetNeighborCells(int x, int y, int step = 1)
        {
            return DirectionUtils.Directions.Select(dir => GetNeighborCell(x, y, dir, step)).Where(cell => cell != null).ToArray();
        }

        public void SetRow(int y, T[] row)
        {
            for (int x = 0; x < Width; x++)
            {
                Data[x, y] = new GridCell<T>(row[x], this, x, y);
            }
        }

        public void SetRows(List<List<T>> rows)
        {
            for (int y = 0; y < rows.Count; y++)
            {
                SetRow(y, rows[y].ToArray());
            }
        }

        public void SetRow(int y, T[] row, object[] metadata)
        {
            for (int x = 0; x < Width; x++)
            {
                Data[x, y] = new GridCell<T>(row[x], this, x, y)
                {
                    Metadata = metadata[x]
                };
            }
        }

        public void SetRow(int y, T[] row, object metadata)
        {
            for (int x = 0; x < Width; x++)
            {
                Data[x, y] = new GridCell<T>(row[x], this, x, y)
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

        public IEnumerable<Z> MapCells<Z>(Func<int, int, GridCell<T>, Z> mapFunc)
        {
            return Enumerable.Range(0, Width)
                .SelectMany(x => Enumerable.Range(0, Height).Select(y => mapFunc(x, y, GetCell(x, y))));
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

        public List<GridCell<T>> Flood(int x, int y, Func<GridCell<T>, bool> condition)
        {
            return FloodRec(x, y, condition, new List<GridCell<T>>());
        }

        private List<GridCell<T>> FloodRec(int x, int y, Func<GridCell<T>, bool> condition,
            List<GridCell<T>> visited)
        {
            var cell = GetCell(x, y);
            if(cell is not null) visited.Add(cell);

            if (cell is null || !condition(cell)) return new List<GridCell<T>>();

            var result = new List<GridCell<T>> { cell };

            var neighbors = GetNeighborCells(x, y).Where(c => !visited.Contains(c));
            foreach (var neighbor in neighbors)
            {
                result.AddRange(FloodRec(neighbor.X, neighbor.Y, condition, visited));
            }
            return result;
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