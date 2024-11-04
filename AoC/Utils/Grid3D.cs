using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using QuikGraph;

namespace AoC.Utils
{
    public class Grid3D<T>
    {
        public readonly GridCell3D<T>[,,] Data;

        public int Width => Data.GetLength(0);
        public int Height => Data.GetLength(1);
        public int Depth => Data.GetLength(2);

        public Grid3D(int w, int h, int d)
        {
            Data = new GridCell3D<T>[w, h, d];
            Apply((x, y, z, cell) => AddCell(x, y, z, default));
        }

        public GridCell3D<T> GetCell(int x, int y, int z)
        {
            if (x >= Width || y >= Height || x < 0 || y < 0 || z >= Depth || z < 0) return null;
            return Data[x, y, z];
        }

        public IEnumerable<GridCell3D<T>> Cells()
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    for (int d = 0; d < Depth; d++)
                    {
                        yield return Data[w, h, d];
                    }

                }
            }
        }

        public T GetValue(int x, int y, int z)
        {
            return GetCell(x, y, z).Value;
        }

        public T GetNeighborValue(int x, int y, int z, Direction3D dir, int step = 1)
        {
            return GetNeighborCell(x, y, z, dir, step).Value;
        }

        public GridCell3D<T> GetNeighborCell(int x, int y, int z, Direction3D dir, int step = 1)
        {
            var pos = DirectionUtils.ApplyDir(x, y, z, dir, step);
            return GetCell(pos.x, pos.y, pos.z);
        }

        public GridCell3D<T> GetNeighborCell(GridCell3D<T> cell, Direction3D dir, int step = 1)
        {
            var pos = DirectionUtils.ApplyDir(cell.X, cell.Y, cell.Z, dir, step);
            return GetCell(pos.x, pos.y, pos.z);
        }

        public GridCell3D<T>[] GetNeighborCells(int x, int y, int z, int step = 1)
        {
            return DirectionUtils.Directions3D.Select(dir => GetNeighborCell(x, y, z, dir, step)).Where(cell => cell != null).ToArray();
        }

        public GridCell3D<T>[] GetNeighborCells(GridCell3D<T> cell, int step = 1)
        {
            return DirectionUtils.Directions3D.Select(dir => GetNeighborCell(cell, dir, step)).Where(cell => cell != null).ToArray();
        }

        //public GridCell<T>[] GetDiagonalNeighborCells(GridCell<T> cell, int dist = 1)
        //{
        //    return DirectionUtils.DirectionDiagonals.Select(dir => GetNeighborCell(cell.X, cell.Y, dir, dist)).Where(c => c != null).ToArray();
        //}

        //public GridCell<T>[] GetDiagonalNeighborCells(int x, int y, int dist = 1)
        //{
        //    return DirectionUtils.DirectionDiagonals.Select(dir => GetNeighborCell(x, y, dir, dist)).Where(c => c != null).ToArray();
        //}

        //public GridCell<T>[] GetAllNeighborCells(int x, int y, int step = 1)
        //{
        //    return GetNeighborCells(x, y, step).Concat(GetDiagonalNeighborCells(x, y, step)).ToArray();
        //}

        //public GridCell<T>[] GetAllNeighborCells(GridCell<T> cell, int step = 1)
        //{
        //    return GetNeighborCells(cell, step).Concat(GetDiagonalNeighborCells(cell, step)).ToArray();
        //}

        //public void SetRow(int y, T[] row)
        //{
        //    for (int x = 0; x < Width; x++)
        //    {
        //        Data[x, y] = new GridCell<T>(row[x], this, x, y);
        //    }
        //}

        //public void SetRows(List<List<T>> rows)
        //{
        //    for (int y = 0; y < rows.Count; y++)
        //    {
        //        SetRow(y, rows[y].ToArray());
        //    }
        //}

        //public void SetRow(int y, T[] row, object[] metadata)
        //{
        //    for (int x = 0; x < Width; x++)
        //    {
        //        Data[x, y] = new GridCell<T>(row[x], this, x, y)
        //        {
        //            Metadata = metadata[x]
        //        };
        //    }
        //}

        //public void SetRow(int y, T[] row, object metadata)
        //{
        //    for (int x = 0; x < Width; x++)
        //    {
        //        Data[x, y] = new GridCell<T>(row[x], this, x, y)
        //        {
        //            Metadata = metadata
        //        };
        //    }
        //}

        //public void SetCol(int x, T[] col, object metadata)
        //{
        //    for (int y = 0; y < Height; y++)
        //    {
        //        Data[x, y] = new GridCell<T>(col[y], this, x, y)
        //        {
        //            Metadata = metadata
        //        };
        //    }
        //}

        //public GridCell<T>[] GetRow(int y)
        //{
        //    var row = new GridCell<T>[Width];
        //    for (int x = 0; x < Width; x++)
        //    {
        //        row[x] = Data[x, y];
        //    }

        //    return row;
        //}

        //public GridCell<T>[] GetCol(int x)
        //{
        //    var col = new GridCell<T>[Height];
        //    for (int y = 0; y < Height; y++)
        //    {
        //        col[y] = Data[x, y];
        //    }
        //    return col;
        //}

        //public GridCell<T>[][] GetRows()
        //{
        //    var rows = new GridCell<T>[Height][];
        //    for (int i = 0; i < Height; i++)
        //    {
        //        rows[i] = GetRow(i);
        //    }
        //    return rows;
        //}

        //public GridCell<T>[][] GetCols()
        //{
        //    var cols = new GridCell<T>[Width][];
        //    for (int i = 0; i < Width; i++)
        //    {
        //        cols[i] = GetCol(i);
        //    }
        //    return cols;
        //}

        //public void SetCols(List<List<T>> cols)
        //{
        //    for (int x = 0; x < cols.Count; x++)
        //    {
        //        SetCol(x, cols[x].ToArray(), null);
        //    }
        //}

        public GridCell3D<T> AddCell(int x, int y, int z, T value, object metadata = null)
        {
            var cell = new GridCell3D<T>(this, x, y, z, value, metadata);
            Data[x, y, z] = cell;
            return cell;
        }

        //public void SetCell(int x, int y, GridCell<T> cell)
        //{
        //    Data[x, y] = cell;
        //}

        //public void Set(int x, int y, T item)
        //{
        //    GetCell(x, y).Value = item;
        //}

        //public void Set(int x, int y, T item, object metadata)
        //{
        //    var cell = GetCell(x, y);
        //    if (cell is null)
        //    {
        //        AddCell(x, y, item, metadata);
        //    }
        //    else
        //    {
        //        cell.Value = item;
        //        cell.Metadata = metadata;
        //    }
        //}

        public void Apply(Action<int, int, int, GridCell3D<T>> action)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    for (int d = 0; d < Depth; d++)
                    {
                        action(w, h, d, Data[w, h, d]);
                    }

                }
            }
        }

        public void Apply(Action<GridCell3D<T>> action)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    for (int z = 0; z < Depth; z++)
                        action(GetCell(x, y, z));
        }

        //public IEnumerable<Z> MapCells<Z>(Func<int, int, GridCell<T>, Z> mapFunc)
        //{
        //    return Enumerable.Range(0, Width)
        //        .SelectMany(x => Enumerable.Range(0, Height).Select(y => mapFunc(x, y, GetCell(x, y))));
        //}

        public void Fill(Func<int, int, int, GridCell3D<T>> getItem)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    for (int d = 0; d < Depth; d++)
                        Data[w, h, d] = getItem(w, h, d);
                }
            }
        }



        //public GridCell<T> Move(GridCell<T> cell, Direction dir, int distance = 1)
        //{
        //    var x = cell.X;
        //    var y = cell.Y;
        //    var z = 0;
        //    switch (dir)
        //    {
        //        case Direction.Up:
        //            y += distance;
        //            break;
        //        case Direction.Down:
        //            y -= distance;
        //            break;
        //        case Direction.Left:
        //            x -= distance;
        //            break;
        //        case Direction.Right:
        //            x += distance;
        //            break;
        //    }

        //    return GetCell(x, y, z);
        //}

        //public void MoveSet(GridCell3D<T> cell, Direction dir, int distance, Action<GridCell3D<T>> action)
        //{
        //    var x = cell.X;
        //    var y = cell.Y;
        //    for (int i = 1; i <= distance; i++)
        //    {
        //        var newX = x;
        //        var newY = y;
        //        switch (dir)
        //        {
        //            case Direction.Up:
        //                newY = y + 1;
        //                break;
        //            case Direction.Down:
        //                newY = y - 1;
        //                break;
        //            case Direction.Left:
        //                newX = x - 1;
        //                break;
        //            case Direction.Right:
        //                newX = x + 1;
        //                break;
        //        }
        //        action(Data[newX, newY]);
        //    }
        //}

        //public string Print(Func<GridCell<T>, string> mapCell, bool flipX = false, bool flipY = false)
        //{
        //    var sb = new StringBuilder();
        //    for (int y = 0; y < Height; y++)
        //    {
        //        for (int x = 0; x < Width; x++)
        //        {
        //            var _x = flipX ? Width - x - 1 : x;
        //            var _y = flipY ? Height - y - 1 : y;
        //            var cell = GetCell(_x, _y);
        //            sb.Append(mapCell(cell));
        //        }
        //        sb.AppendLine();
        //    }
        //    return sb.ToString();
        //}

        public AdjacencyGraph<GridCell3D<T>, SEquatableEdge<GridCell3D<T>>> BuildAdjacencyGraph(bool allowDiagonal = false)
        {
            return BuildAdjacencyGraph(cell => cell.GetNeighbors());
        }

        public AdjacencyGraph<GridCell3D<T>, SEquatableEdge<GridCell3D<T>>> BuildAdjacencyGraph(Func<GridCell3D<T>, IEnumerable<GridCell3D<T>>> expand)
        {
            var graph = new AdjacencyGraph<GridCell3D<T>, SEquatableEdge<GridCell3D<T>>>();

            foreach (var cell in Cells())
            {
                var neighbors = expand(cell);
                foreach (var neighbor in neighbors)
                {
                    if (neighbor is null) continue;
                    graph.AddVerticesAndEdge(new SEquatableEdge<GridCell3D<T>>(cell, neighbor));
                }
            }

            return graph;
        }
    }
}