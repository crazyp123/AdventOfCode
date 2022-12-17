using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using Point = AoC.Utils.Point;

namespace AoC.y2022
{
    public class Day15 : Day
    {
        private List<(Point sensor, Point beacon, int distance)> _data;
        private int _targetY = 10;

        public Day15()
        {
            // test input
            var input = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";

            input = Input;
            _targetY = 2000000;


            _data = input.AsListOf<string>().Select(s =>
            {
                var info = s.Split(':');
                var ss = info[0].Replace("Sensor at x=", "").Replace("y=", "").Trim().Split(',');
                var sensor = new Point(ss[0].Trim().AsInt(), ss[1].Trim().AsInt());

                var bs = info[1].Replace("closest beacon is at x=", "").Replace("y=", "").Trim().Split(',');
                var beacon = new Point(bs[0].Trim().AsInt(), bs[1].Trim().AsInt());
                return (sensor, beacon, distance: Calculations.ManhattanDistance(sensor, beacon));
            }).ToList();
        }

        public override object Result1()
        {
            var from = _data.MinBy(t => t.sensor.X);
            var to = _data.MaxBy(t => t.sensor.X);

            var fromX = from.sensor.X - from.distance;
            var toX = to.sensor.X + to.distance;

            var points = new HashSet<Point>();

            for (int x = fromX; x <= toX; x++)
            {
                var point = new Point(x, _targetY);

                if (IsCovered(point)) points.Add(point);
            }

            return points.Count;
        }

        public override object Result2()
        {
            var boxes = _data.Select(t => new Polygon(new LinearRing(new[]
            {
                new Coordinate(t.sensor.X - t.distance, t.sensor.Y),
                new Coordinate(t.sensor.X, t.sensor.Y + t.distance),
                new Coordinate(t.sensor.X + t.distance, t.sensor.Y),
                new Coordinate(t.sensor.X, t.sensor.Y - t.distance),
                new Coordinate(t.sensor.X - t.distance, t.sensor.Y),
            }))).ToList();

            var intersections = new List<Coordinate>();

            foreach (var poly in boxes)
                foreach (var poly2 in boxes.Where(poly2 => poly != poly2))
                    intersections.AddRange(poly.Intersection(poly2).Coordinates);

            var points = intersections.Select(c => new Point((int)c.X, (int)c.Y)).SelectMany(p => p.GetAllNeighbors())
                .Distinct().ToList();

            return (from point in points
                    where IsNotCovered(point) && point.GetAllNeighbors().All(IsCovered)
                    select point.X * 4000000L + point.Y).FirstOrDefault();
        }

        bool IsCovered(Point point)
        {
            foreach (var (sensor, beacon, distance) in _data)
            {
                if (beacon.Equals(point)) continue;
                if (Calculations.ManhattanDistance(point, sensor) <= distance) return true;
            }

            return false;
        }

        bool IsNotCovered(Point point)
        {
            return _data.All(t => Calculations.ManhattanDistance(point, t.sensor) > t.distance);
        }
    }
}