using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode._2018
{
    public class Day11
    {
        protected internal int[,] Grid;
        protected internal int gridWidth;
        protected internal int gridHeight;

        private int serial = 8868;

        public Day11()
        {
            gridWidth = 300;
            gridHeight = 300;
            Grid = new int[gridWidth, gridHeight];
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Grid[x, y] = Compute(x, y);
                }
            }

            Part1();
            Part2();
        }

        void Part1()
        {
            int max = 0;
            var answer = "";

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var sum = GetSquareSumAt(x, y, 3);
                    if (sum > max)
                    {
                        max = sum;
                        answer = $"{x},{y}";
                    }
                }
            }

            Utils.Answer(11, 1, answer);
        }

        object locker = new object();

        void Part2()
        {
            int max = 0;
            var answer = "";

            Parallel.For(0, gridWidth, x =>
            {
                Parallel.For(0, gridHeight, y =>
                {
                    var previousSum = 0;
                    for (int s = 1; s < gridWidth; s++)
                    {
                        var sum = previousSum + GetOuterSquareSumAt(x, y, s);
                        lock (locker)
                        {
                            
                            if (sum > max)
                            {
                                max = sum;
                                answer = $"{x},{y},{s}";
                            }

                            previousSum = sum;
                        }
                    }
                });
            });

            Utils.Answer(11, 2, answer);
        }

        int Compute(int x, int y)
        {
            var rackID = x + 10;
            var pl =rackID * y;
            pl += serial;
            pl *= rackID;
            var hundreds = Math.Abs(pl / 100 % 10);
            return hundreds - 5;
        }

        int GetOuterSquareSumAt(int x, int y, int s)
        {
            if (x + s >= gridWidth || y + s >= gridHeight)
            {
                return 0;
            }

            var sum = 0;
            for (int w = 0; w < s; w++)
            {
                for (int h = 0; h < s; h++)
                {
                    var index0 = x + w;
                    var index1 = y + h;
                    if (w == s - 1 || h == s - 1)
                    {
                        sum += Grid[index0, index1];
                    }
                }
            }

            return sum;
        }

        int GetSquareSumAt(int x, int y, int s)
        {
            if (x + s >= gridWidth || y + s >= gridHeight)
            {
                return 0;
            }
            var sum = 0;
            for (int w = 0; w < s; w++)
            {
                for (int h = 0; h < s; h++)
                {
                    var index0 = x + w;
                    var index1 = y + h;
                    sum += Grid[index0, index1];
                }
            }
            return sum;
        }
    }
}