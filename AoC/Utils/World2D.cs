using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils;

public class World2D<T>
{
    public readonly Grid<T> Grid;

    public List<WorldObject<T>> Objects { get; } = new List<WorldObject<T>>();

    public World2D(int width, int height)
    {
        Grid = new Grid<T>(width, height);
    }

    public WorldObject<T> AddObject(Grid<T> body, Point position)
    {
        var worldObject = new WorldObject<T>(this, body, position);
        Objects.Add(worldObject);
        return worldObject;
    }
}


public class WorldObject<T>
{
    public readonly World2D<T> World;
    public readonly Grid<T> Body;
    public Point Position { get; private set; } // bottom left

    public List<GridCell<T>> WorldCells { get; private set; } = new List<GridCell<T>>();

    public WorldObject(World2D<T> world, Grid<T> body, Point position)
    {
        World = world;
        Body = body;
        Position = position;

        TrySetPosition(position);
    }

    public bool TrySetPosition(Point pos)
    {
        var worldCells = Body.Cells.Where(c => c.Value != null)
            .Select(cell => World.Grid
                .GetCell(cell.X + pos.X, cell.Y + pos.Y))
            .ToList();

        if (worldCells.Any(c => c is null || (c.Metadata is not null && !this.Equals(c.Metadata)))) return false;

        WorldCells.ForEach(c =>
        {
            c.Value = default;
            c.Metadata = null;
        });

        Position = pos;
        WorldCells = worldCells;

        WorldCells.ForEach(c =>
        {
            c.Value = PaintCell(c);
            c.Metadata = this;
        });

        return true;
    }

    public bool TryMove(Direction dir, int dist = 1)
    {
        return TrySetPosition(Position.Apply(dir, dist));
    }

    public T PaintCell(GridCell<T> cell)
    {
        return Body.GetValue(cell.X - Position.X, cell.Y - Position.Y);
    }

}