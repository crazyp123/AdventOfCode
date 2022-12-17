using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
using Microsoft.VisualBasic;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ShortestPath;

namespace AoC.y2022
{
    public class Day16 : Day
    {
        private AdjacencyGraph<Valve, EquatableEdge<Valve>> _graph;
        private Dictionary<string, Valve> _valves;
        private Valve _startValve;
        private List<(Valve valve, List<string> tunnels)> _data;

        public Day16()
        {
            // test input
            var input = @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";

            input = Input;

            _data = input.AsListOf<string>().Select(line =>
            {
                var parts = line.Split(';');

                var p1 = parts[0].Replace("Valve ", "").Replace("has flow rate=", "").Split(' ');
                var valve = p1[0].Trim();
                var rate = p1[1].Trim().AsInt();
                var tunnels = parts[1].Replace("tunnels lead to valves ", "").Replace("tunnel leads to valve ", "").Trim().Split(',').Select(s => s.Trim())
                    .ToList();
                return (valve: new Valve { Name = valve, Rate = rate }, tunnels);
            }).ToList();

            _valves = _data.ToDictionary(t => t.valve.Name, t => t.valve);

            _graph = new AdjacencyGraph<Valve, EquatableEdge<Valve>>();

            foreach (var (valve, tunnels) in _data)
            {
                foreach (var tunnel in tunnels)
                {
                    _graph.AddVerticesAndEdge(new EquatableEdge<Valve>(valve, _valves[tunnel]));
                }
            }

            _startValve = _valves["AA"];

        }

        public override object Result1()
        {
            //var alg = new FloydWarshallAllShortestPathAlgorithm<Valve, EquatableEdge<Valve>>(_graph, edge => 1);
            //alg.Compute();

            //var paths = new Dictionary<Valve, Dictionary<Valve, List<Valve>>>();

            //foreach (var (valve, tunnels) in _data)
            //{
            //    var vpaths = new Dictionary<Valve, List<Valve>>();
            //    foreach (var (valve2, tunnels2) in _data)
            //    {
            //        if (valve == valve2 || valve2.Rate == 0) continue;

            //        if (alg.TryGetPath(valve, valve2, out var path))
            //        {
            //            vpaths.Add(valve2, path.Select(e => e.Target).ToList());
            //        }
            //    }

            //    paths.Add(valve, vpaths);
            //}

            //var current = _startValve;

            //var final = new List<(Valve, bool)>();

            //for (var min = 1; min <= 30; min++)
            //{
            //    var search = paths[current].Where(p => final.All(t => t.Item1 != p.Key)).ToList();
            //    if (search.Count == 0) break;

            //    var next = search.MaxBy(pair => pair.Key.Rate - pair.Value.Count);

            //    final.AddRange(next.Value.Select(v => (v, v == next.Key)));

            //    current = next.Key;

            //    min += next.Value.Count;
            //}

            //final = new List<(Valve, bool)> { (_startValve, false) }.Concat(final).ToList();
            //return Calculate(final);

            var selected = _valves.Values.Where(v => v.Rate > 0).OrderByDescending(v => v.Rate).Take(15).ToList();
            var perms = selected.Permutations();


            //var test = new List<(Valve, bool)>
            //{
            //    (_valves["DD"], true),
            //    (_valves["BB"], true)  ,
            //    (_valves["JJ"], true)  ,
            //    (_valves["HH"], true)  ,
            //    (_valves["EE"], true)  ,
            //    (_valves["CC"], true)
            //};

            //return Calculate(FindPath(test.Select(t => t.Item1).ToArray()));

            return perms.Select(FindPath).Select(Calculate).Max();



            // too low  925
        }

        private List<(Valve, bool)> FindPath(Valve[] valves)
        {
            var current = _startValve;
            var path = new List<(Valve, bool)> { (_startValve, false) };
            foreach (var valve in valves)
            {
                path.AddRange(GetPath(current, valve).Select(v => (v, v == valve)));
                current = valve;
            }
            return path;
        }

        List<Valve> FindPathUsingOrder(Valve[] order)
        {
            var toVisit = order.ToList();
            var final = new List<Valve>();

            var current = _startValve;
            var next = toVisit[0];

            while (true)
            {
                final.AddRange(GetPath(current, next));
                current = next;

                toVisit.RemoveAll(v => final.Contains(v));

                if (toVisit.Count == 0 || toVisit.All(v => v.Rate == 0)) break;
                next = toVisit[0];
            }

            return final;
        }

        int Calculate(IEnumerable<(Valve, bool)> path)
        {
            var list = path.ToList();
            var openValves = new List<Valve>();
            var sum = 0;

            var p = 0;

            for (int min = 1; min <= 30; min++)
            {
                sum += openValves.Sum(x => x.Rate);


                if (list.Count > p)
                {
                    var t = list[p];
                    if (t.Item2 && !openValves.Contains(t.Item1))
                    {
                        openValves.Add(t.Item1);
                    }
                    else
                    {
                        p++;
                    }
                }

            }
            return sum;
        }

        int Calculate(IEnumerable<Valve> path)
        {
            var release = 0;
            var min = 1;
            foreach (var valve in path)
            {
                if (valve.Rate > 0)
                {
                    //open
                    release += (30 - min) * valve.Rate;
                    min++;
                }

                min++;

                if (min >= 30) break;
            }

            return release;
        }

        List<Valve> GetPath(Valve from, Valve to)
        {
            var findPath = _graph.ShortestPathsDijkstra(e => 1, from);
            findPath.Invoke(to, out var path);
            return path.Select(e => e.Target).ToList();
        }

        public override object Result2()
        {
            throw new System.NotImplementedException();
        }

        record Valve
        {
            public string Name { get; set; }
            public int Rate { get; set; }

        }
    }
}