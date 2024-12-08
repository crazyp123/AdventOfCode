using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day08 : Day
{
    private Grid<char> _grid;
    private Dictionary<char, List<GridCell<char>>> _antennas;

    public Day08()
    {
        _grid = Input.AsGrid();
        _antennas = _grid.Cells
            .Where(c => c.Value != '.')
            .GroupBy(c => c.Value)
            .ToDictionary(c => c.Key, g => g.ToList());
    }

    public override object Result1()
    {
        return Count(1);
    }

    public override object Result2()
    {
        return Count(0);
    }

    private object Count(int onlyStep)
    {
        var antenodes = new HashSet<GridCell<char>>();
        foreach (var pair in _antennas)
        foreach (var antenna in pair.Value)
        foreach (var otherAntenna in pair.Value)
        {
            if (Equals(antenna, otherAntenna)) continue;

            var step = onlyStep;

            while (onlyStep == 0 || step == 1)
            {
                var direction = (otherAntenna.ToPoint() - antenna.ToPoint()) * step;
                var antenode1Pos = otherAntenna.ToPoint() + direction;
                var antenode2Pos = antenna.ToPoint() - direction;

                var antenode1 = _grid.GetCell(antenode1Pos);
                var antenode2 = _grid.GetCell(antenode2Pos);

                if (antenode1 != null) antenodes.Add(antenode1);
                if (antenode2 != null) antenodes.Add(antenode2);

                if (antenode1 == null && antenode2 == null) break;
                step++;
            }
        }

        return antenodes.Count();
    }
}