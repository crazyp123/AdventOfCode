using System.Collections.Generic;
using System.Linq;
using AdventOfCode;

namespace AoC.y2018
{
    public class Day9
    {
        private int players = 439;
        private int marbles = 71307;
        protected internal List<long> scores;
        protected internal LinkedList<long> circle;
        protected internal int marble;
        protected internal LinkedListNode<long> current;

        public Day9()
        {
            Setup();

            Part1();

            Setup();

            Part2();
        }

        void Setup()
        {
            scores = Enumerable.Range(0, players).Select(i => (long)0).ToList();

            circle = new LinkedList<long>();
            current = circle.AddFirst(0);
            marble = 1;
        }

        void Part1()
        {
            while (marble < marbles)
            {
                Play();
            }


            Utils.Answer(9, 1, scores.Max());
        }

        void Part2()
        {
            marbles *= 100;

            while (marble < marbles)
            {
                Play();
            }


            Utils.Answer(9, 2, scores.Max());
        }


        void Play()
        {
            for (int player = 0; player < players; player++)
            {

                if (marble % 23 == 0)
                {
                    var toRemove = GetFromCircle(current, 7, false);
                    current = GetFromCircle(toRemove, 1);
                    circle.Remove(toRemove);
                    scores[player] += (((long)marble) + toRemove.Value);
                }
                else
                {
                    var next = GetFromCircle(current, 1);
                    current = circle.AddAfter(next, marble);
                }

                marble++;
            }
        }

        LinkedListNode<long> GetFromCircle(LinkedListNode<long> from, long pos, bool clockwise = true)
        {
            var next = from;
            for (int i = 0; i < pos; i++)
            {
                next = clockwise ? (next.Next ?? circle.First) : (next.Previous ?? circle.Last);
            }
            return next;
        }
    }
}