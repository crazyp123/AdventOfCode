using System.Linq;
using AoC.Utils;

namespace AoC.y2021;

public class Day11 : Day
{
    private Grid<int> _grid;

    public Day11()
    {
        var input = "";

        input = Input;

        var data = input.AsListOf<string>().Select(s => s.AsListOf<int>()).ToList();
        _grid = data.ToGrid();
    }

    int Flashes(int steps)
    {
        var flashes = 0;
        for (int i = 0; i < steps; i++)
        {
            _grid.Apply(cell =>
            {
                cell.Value += 1;
                if (cell.Value > 9)
                {
                    flashes++;
                }
            });

            var ripe = _grid.Cells.Where(cell => cell.Value > 9).ToList();

            foreach (var cell in ripe)
            {
                
            }

        }

        return flashes;
    }

    public override object Result1()
    {
        
    }

    public override object Result2()
    {
        throw new System.NotImplementedException();
    }
}