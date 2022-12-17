using System.Linq;
using AoC.Utils;
using QuikGraph;
using QuikGraph.Algorithms;

namespace AoC.y2022
{
    public class Day12 : Day
    {
        private Grid<char> _grid;
        private AdjacencyGraph<GridCell<char>, SEquatableEdge<GridCell<char>>> _graph;

        public Day12()
        {
            // test input
            var input = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";

            input = Input;

            _grid = input.AsListOf<string>().Select(s => s.Trim().ToCharArray().ToList()).ToList().ToGrid();

            _grid.Apply(cell =>
            {
                var letter = cell.Value switch
                {
                    'S' => 'a',
                    'E' => 'z',
                    _ => cell.Value
                };

                cell.Metadata = letter.PositionInAlphabet();
            });

            _graph = _grid.BuildAdjacencyGraph(cell => cell.GetNeighbors(1).Where(target =>
            {
                var dist = (int)target.Metadata - (int)cell.Metadata;
                return dist <= 1;
            }));

        }

        public override object Result1()
        {
            var start = _grid.Cells.Find(c => c.Value == 'S');
            var end = _grid.Cells.Find(cell => cell.Value == 'E');

            _graph.ShortestPathsAStar(edge => 1, cell => 1, start).Invoke(end, out var path);

            return path.Count();
        }

        public override object Result2()
        {
            var end = _grid.Cells.Find(cell => cell.Value == 'E');
            return _grid.Cells.Where(cell => cell.Value == 'S' || cell.Value == 'a').Select(start =>
            {
                _graph.ShortestPathsAStar(edge => 1, cell => 1, start).Invoke(end, out var path);
                return path?.Count() ?? 0;
            }).ToList().Where(i => i > 0).Min();
        }
    }
}