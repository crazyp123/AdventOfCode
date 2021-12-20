using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2021;

public class Day11 : Day
{
    private Grid<int> _grid;

    public Day11()
    {
        var input = "11111\n19991\n19191\n19991\n11111";

        input = Input;

        var data = input.AsListOf<string>().Select(s => s.AsListOfNumbers()).ToList();
        _grid = data.ToGrid();
        _grid.Apply(cell => cell.Metadata = false);
    }

    IEnumerable<int> Flashes(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            var flashes = 0;
            _grid.Apply(cell => cell.Value += 1);


            var ripe = _grid.Cells.Where(cell => cell.Value > 9).ToList();

            foreach (var cell in ripe)
            {
                flashes += Flashed(cell);
            }

            _grid.Cells.Where(cell => cell.Value > 9).Apply(cell =>
            {
                cell.Value = 0;
                cell.Metadata = false;
            });

            yield return flashes;

        }

        int Flashed(GridCell<int> c)
        {
            if ((bool)c.Metadata || c.Value <= 9) return 0;

            var flashes = 1;
            c.Metadata = true;

            foreach (var neighborCell in _grid.GetAllNeighborCells(c))
            {
                neighborCell.Value += 1;
                flashes += Flashed(neighborCell);
            }

            return flashes;
        }
    }

    public override object Result1()
    {
        return Flashes(100).Sum();
    }

    public override object Result2()
    {
        var count = _grid.Cells.Count;
        return Flashes(1000).TakeWhile(i => i != count).Count();
    }
}