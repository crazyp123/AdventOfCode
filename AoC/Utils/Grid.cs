using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikGraph;

namespace AoC.Utils;

public class Grid<T>
{
    public readonly GridCell<T>[,] Data;

    public Grid(int w, int h)
    {
        Data = new GridCell<T>[w, h];
        Apply((x, y, cell) => AddCell(x, y, default));
    }

    public Grid(List<List<T>> data)
    {
        var height = data.Count;
        var width = data[0].Count;
        Data = new GridCell<T>[width, height];
        SetRows(data);
    }

    public int Width => Data.GetLength(0);
    public int Height => Data.GetLength(1);

    public List<GridCell<T>> Cells => GetRows().SelectMany(r => r).ToList();


    public bool BottomLeftOrigin = false;

    public Grid<T> Clone()
    {
        var grid = new Grid<T>(Width, Height);
        grid.SetRows(GetRows().Select(row => row.Select(cell => cell.Value).ToList()).ToList());
        return grid;
    }

    public GridCell<T> GetCell(int x, int y)
    {
        if (x >= Width || y >= Height || x < 0 || y < 0) return null;
        return Data[x, y];
    }

    public T GetValue(int x, int y)
    {
        var cell = GetCell(x, y);
        return cell != null ? cell.Value : default;
    }

    public T GetNeighborValue(int x, int y, Direction dir, int step = 1)
    {
        var cell = GetNeighborCell(x, y, dir, step);
        if (cell == null) return default;
        return cell.Value;
    }

    public T GetNeighborValue(int x, int y, DirectionDiagonal dir, int step = 1)
    {
        var cell = GetNeighborCell(x, y, dir, step);
        if (cell == null) return default;
        return cell.Value;
    }

    public GridCell<T> GetNeighborCell(int x, int y, Direction dir, int step = 1)
    {
        var pos = DirectionUtils.ApplyDir(x, y, dir, step);
        return GetCell(pos.x, pos.y);
    }

    public GridCell<T> GetNeighborCell(GridCell<T> cell, Direction dir, int step = 1)
    {
        var pos = DirectionUtils.ApplyDir(cell.X, cell.Y, dir, step);
        return GetCell(pos.x, pos.y);
    }

    public GridCell<T> GetNeighborCell(int x, int y, DirectionDiagonal dir, int step = 1)
    {
        var pos = DirectionUtils.ApplyDir(x, y, dir, step);
        return GetCell(pos.x, pos.y);
    }

    public GridCell<T>[] GetNeighborCells(int x, int y, int step = 1)
    {
        return DirectionUtils.Directions.Select(dir => GetNeighborCell(x, y, dir, step)).Where(cell => cell != null)
            .ToArray();
    }

    public GridCell<T>[] GetNeighborCells(GridCell<T> cell, int step = 1)
    {
        return DirectionUtils.Directions.Select(dir => GetNeighborCell(cell.X, cell.Y, dir, step))
            .Where(cell => cell != null).ToArray();
    }

    public GridCell<T>[] GetDiagonalNeighborCells(GridCell<T> cell, int dist = 1)
    {
        return DirectionUtils.DirectionDiagonals.Select(dir => GetNeighborCell(cell.X, cell.Y, dir, dist))
            .Where(c => c != null).ToArray();
    }

    public GridCell<T>[] GetDiagonalNeighborCells(int x, int y, int dist = 1)
    {
        return DirectionUtils.DirectionDiagonals.Select(dir => GetNeighborCell(x, y, dir, dist)).Where(c => c != null)
            .ToArray();
    }

    public GridCell<T>[] GetAllNeighborCells(int x, int y, int step = 1)
    {
        return GetNeighborCells(x, y, step).Concat(GetDiagonalNeighborCells(x, y, step)).ToArray();
    }

    public GridCell<T>[] GetAllNeighborCells(GridCell<T> cell, int step = 1)
    {
        return GetNeighborCells(cell, step).Concat(GetDiagonalNeighborCells(cell, step)).ToArray();
    }

    /// <summary>
    ///     TO BE REVIEWED
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    [Obsolete]
    public GridCell<T>[] GetAllToEdge(GridCell<T> cell, Direction dir)
    {
        return dir switch
        {
            Direction.Down => GetCol(cell.X)[(cell.Y + 1)..],
            Direction.Up => cell.Y > 0 ? GetCol(cell.X)[..cell.Y].Reverse().ToArray() : Array.Empty<GridCell<T>>(),
            Direction.Right => GetRow(cell.Y)[(cell.X + 1)..],
            Direction.Left => cell.X > 0 ? GetRow(cell.Y)[..cell.X].Reverse().ToArray() : Array.Empty<GridCell<T>>(),
            _ => default
        };
    }

    public void SetRow(int y, T[] row)
    {
        for (var x = 0; x < Width; x++) Data[x, y] = new GridCell<T>(row[x], this, x, y);
    }

    public void SetRows(List<List<T>> rows)
    {
        for (var y = 0; y < rows.Count; y++) SetRow(y, rows[y].ToArray());
    }

    public void SetRow(int y, T[] row, object[] metadata)
    {
        for (var x = 0; x < Width; x++)
            Data[x, y] = new GridCell<T>(row[x], this, x, y)
            {
                Metadata = metadata[x]
            };
    }

    public void SetRow(int y, T[] row, object metadata)
    {
        for (var x = 0; x < Width; x++)
            Data[x, y] = new GridCell<T>(row[x], this, x, y)
            {
                Metadata = metadata
            };
    }

    public void SetCol(int x, T[] col, object metadata)
    {
        for (var y = 0; y < Height; y++)
            Data[x, y] = new GridCell<T>(col[y], this, x, y)
            {
                Metadata = metadata
            };
    }

    public GridCell<T>[] GetRow(int y)
    {
        var row = new GridCell<T>[Width];
        for (var x = 0; x < Width; x++) row[x] = Data[x, y];

        return row;
    }

    public GridCell<T>[] GetCol(int x)
    {
        var col = new GridCell<T>[Height];
        for (var y = 0; y < Height; y++) col[y] = Data[x, y];
        return col;
    }

    public GridCell<T>[][] GetRows()
    {
        var rows = new GridCell<T>[Height][];
        for (var i = 0; i < Height; i++) rows[i] = GetRow(i);
        return rows;
    }

    public GridCell<T>[][] GetCols()
    {
        var cols = new GridCell<T>[Width][];
        for (var i = 0; i < Width; i++) cols[i] = GetCol(i);
        return cols;
    }

    public void SetCols(List<List<T>> cols)
    {
        for (var x = 0; x < cols.Count; x++) SetCol(x, cols[x].ToArray(), null);
    }

    public GridCell<T> AddCell(int x, int y, T value, object metadata = null)
    {
        var cell = new GridCell<T>(this, x, y, value, metadata);
        Data[x, y] = cell;
        return cell;
    }

    public void SetCell(int x, int y, GridCell<T> cell)
    {
        Data[x, y] = cell;
    }

    public void Set(int x, int y, T item)
    {
        GetCell(x, y).Value = item;
    }

    public void Set(int x, int y, T item, object metadata)
    {
        var cell = GetCell(x, y);
        if (cell is null)
        {
            AddCell(x, y, item, metadata);
        }
        else
        {
            cell.Value = item;
            cell.Metadata = metadata;
        }
    }

    public void Apply(Action<int, int, GridCell<T>> action)
    {
        for (var w = 0; w < Width; w++)
        for (var h = 0; h < Height; h++)
            action(w, h, Data[w, h]);
    }

    public void Apply(Action<GridCell<T>> action)
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            action(GetCell(x, y));
    }

    public IEnumerable<Z> MapCells<Z>(Func<int, int, GridCell<T>, Z> mapFunc)
    {
        return Enumerable.Range(0, Width)
            .SelectMany(x => Enumerable.Range(0, Height).Select(y => mapFunc(x, y, GetCell(x, y))));
    }

    public void Fill(Func<int, int, GridCell<T>> getItem)
    {
        for (var w = 0; w < Width; w++)
        for (var h = 0; h < Height; h++)
            Data[w, h] = getItem(w, h);
    }

    public List<GridCell<T>> Flood(int x, int y, Func<GridCell<T>, bool> condition)
    {
        return FloodRec(x, y, condition, new List<GridCell<T>>());
    }

    public List<GridCell<T>> Flood(GridCell<T> cell, Func<GridCell<T>, bool> condition)
    {
        return FloodRec(cell, condition, new List<GridCell<T>>());
    }

    private List<GridCell<T>> FloodRec(int x, int y, Func<GridCell<T>, bool> condition,
        List<GridCell<T>> visited)
    {
        var cell = GetCell(x, y);
        return FloodRec(cell, condition, visited);
    }

    private List<GridCell<T>> FloodRec(GridCell<T> cell, Func<GridCell<T>, bool> condition,
        List<GridCell<T>> visited)
    {
        if (cell is not null) visited.Add(cell);

        if (cell is null || !condition(cell)) return new List<GridCell<T>>();

        var result = new List<GridCell<T>> { cell };

        var neighbors = GetNeighborCells(cell.X, cell.Y).Where(c => !visited.Contains(c));
        foreach (var neighbor in neighbors) result.AddRange(FloodRec(neighbor.X, neighbor.Y, condition, visited));

        return result;
    }

    public GridCell<T> Move(GridCell<T> cell, Direction dir, int distance = 1)
    {
        var x = cell.X;
        var y = cell.Y;
        switch (dir)
        {
            case Direction.Up:
                y += BottomLeftOrigin ? -distance : distance;
                break;
            case Direction.Down:
                y -= BottomLeftOrigin ? -distance : distance;
                break;
            case Direction.Left:
                x -= distance;
                break;
            case Direction.Right:
                x += distance;
                break;
        }

        return GetCell(x, y);
    }

    public void MoveSet(GridCell<T> cell, Direction dir, int distance, Action<GridCell<T>> action)
    {
        var x = cell.X;
        var y = cell.Y;
        for (var i = 1; i <= distance; i++)
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

            action(Data[newX, newY]);
        }
    }

    public void MoveSet(int x, int y, Direction dir, int distance, Func<int, int, T, T> action)
    {
        for (var i = 1; i <= distance; i++)
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

    /// <summary>
    ///     Folds the current grid along an axis like a piece of paper. Returns a new grid.
    /// </summary>
    /// <param name="vertically">x or y axis</param>
    /// <param name="foldSide">the side to fold e.g 0 is left to right or top to bottom</param>
    /// <param name="foldOnItem">Set whether to pivot on index or item. If true, the pivoting position row/col will be dropped.</param>
    /// <param name="mergeCells">merge function</param>
    /// <returns></returns>
    public Grid<T> FoldInward(bool vertically, int position, int foldSide, bool foldOnItem,
        Action<GridCell<T>, GridCell<T>, GridCell<T>> mergeCells)
    {
        var _w = vertically && foldOnItem ? Width - 1 : Width;
        var _h = !vertically && foldOnItem ? Height - 1 : Height;

        var width = vertically ? Math.Max(position, _w - position) : _w;
        var height = vertically ? _h : Math.Max(position, _h - position);

        var grid = new Grid<T>(width, height);
        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            var x0 = vertically ? foldSide == 0 ? width - 1 - x : x : x;
            var y0 = !vertically ? foldSide == 0 ? height - 1 - y : y : y;

            var x1 = vertically ? foldSide == 0 ? width + x : width + width - 1 - x : x;
            var y1 = !vertically ? foldSide == 0 ? height + y : height + height - 1 - y : y;

            if (foldOnItem)
            {
                x1 += vertically ? 1 : 0;
                y1 += !vertically ? 1 : 0;
            }

            var cell0 = GetCell(x0, y0);
            var cell1 = GetCell(x1, y1);
            var newCell = grid.AddCell(x, y, default);

            mergeCells(cell0, cell1, newCell);
        }

        return grid;
    }

    public string Print(Func<GridCell<T>, string> mapCell, bool flipX = false, bool flipY = false)
    {
        var sb = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var _x = flipX ? Width - x - 1 : x;
                var _y = flipY ? Height - y - 1 : y;
                var cell = GetCell(_x, _y);
                sb.Append(mapCell(cell));
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public Image ToImage(Func<GridCell<T>, KnownColor> getCellColor)
    {
        var image = new Bitmap(Width, Height);

        Cells.ForEach(cell =>
        {
            var kc = getCellColor(cell);
            var color = Color.FromKnownColor(kc);
            image.SetPixel(cell.X, cell.Y, color);
        });

        return image;
    }

    public AdjacencyGraph<GridCell<T>, SEquatableEdge<GridCell<T>>> BuildAdjacencyGraph(bool allowDiagonal = false)
    {
        return BuildAdjacencyGraph(cell => allowDiagonal ? cell.GetAllNeighbors() : cell.GetNeighbors());
    }

    public AdjacencyGraph<GridCell<T>, SEquatableEdge<GridCell<T>>> BuildAdjacencyGraph(
        Func<GridCell<T>, IEnumerable<GridCell<T>>> expand)
    {
        var graph = new AdjacencyGraph<GridCell<T>, SEquatableEdge<GridCell<T>>>();

        foreach (var cell in Cells)
        {
            var neighbors = expand(cell);
            foreach (var neighbor in neighbors)
            {
                if (neighbor is null) continue;
                graph.AddVerticesAndEdge(new SEquatableEdge<GridCell<T>>(cell, neighbor));
            }
        }

        return graph;
    }

    public List<List<GridCell<T>>> MatchKernels(Grid<T> kernel, int startX = 0, int startY = 0)
    {
        var result = new ConcurrentBag<List<GridCell<T>>>();

        Parallel.ForEach(Cells, cell =>
        {
            var (match, list) = CheckKernel(cell);
            if (match) result.Add(list);
        });

        return result.ToList();

        (bool, List<GridCell<T>> ) CheckKernel(GridCell<T> cell)
        {
            var list = new List<GridCell<T>>();

            for (var x = 0; x < kernel.Width; x++)
            for (var y = 0; y < kernel.Height; y++)
            {
                var kCell = kernel.GetValue(x, y);
                var cCell = GetCell(cell.X + x, cell.Y + y);
                if (cCell is null ||
                    (!EqualityComparer<T>.Default.Equals(kCell, default) && !Equals(cCell.Value, kCell)))
                    return (false, null);
                list.Add(cCell);
            }

            return (true, list);
        }
    }
}