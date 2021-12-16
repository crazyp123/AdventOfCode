using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
namespace AoC.y2021;

public class Day04 : Day
{
    private List<Grid<int>> _boards;
    private List<int> _random;
    private Dictionary<int, int> _winners;

    public Day04()
    {
        var asListOf = Input.AsListOf<string>();
        _random = asListOf[0].AsListOf<int>(",");
        _boards = new List<Grid<int>>();
        var i = 0;
        Grid<int> grid = null;

        foreach (var x in asListOf.Skip(1))
        {
            if (string.IsNullOrEmpty(x.Trim()))
            {
                grid = new Grid<int>(5, 5);
                _boards.Add(grid);
                i = 0;
            }
            else
            {
                grid.SetRow(i, x.AsListOf<int>(" ", StringSplitOptions.RemoveEmptyEntries).ToArray(), false);
                i++;
            }
        }

        _winners = new Dictionary<int, int>();
        
        foreach (var num in _random)
        {
            for (var index = 0; index < _boards.Count; index++)
            {
                if (_winners.ContainsKey(index))
                {
                    continue;
                }

                var board = _boards[index];
                board.Apply((x, y, cell) =>
                {
                    if (cell.Value == num)
                    {
                        cell.Metadata = true;
                    }
                });

                if (board.GetCols().Any(col => col.All(cell => (bool)cell.Metadata)) ||
                    board.GetRows().Any(row => row.All(cell => (bool)cell.Metadata)))
                {
                    var tot = 0;
                    board.Apply((i, i1, cell) =>
                    {
                        if (!(bool)cell.Metadata)
                        {
                            tot += cell.Value;
                        }
                    });
                    _winners.Add(index, tot * num);
                }
            }
        }

    }

    public override object Result1()
    {
        return _winners.First().Value;
    }

    public override object Result2()
    {
        return _winners.Last().Value;
    }
}