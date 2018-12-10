using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    public class Day9
    {
        private int players = 439;
        private int marbles = 71307;

        public Day9()
        {
            Part1();
        }

        void Part1()
        {
            var circle = new List<int> { 0 };

            //for (int i = 0; i < marbles; i++)
            //{
            //    circle.Add(0);
            //}

            var scores = Enumerable.Range(0, 10).Select(i => 0).ToList();

            var marble = 1;
            var current = 0;

            Play();

            void Play()
            {
                for (int i = 0; i < players; i++)
                {
                    var nextLoc = (current + 2) % (marble + 1);

                    nextLoc = nextLoc + 1 % (marble + 1);

                    circle.Insert(nextLoc, marble);

                    marble++;
                    current = nextLoc;
                }
            }
        }
    }
}