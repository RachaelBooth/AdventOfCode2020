using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Day15
{
    public class Solver : ISolver
    {
        private List<int> input = new List<int> { 7, 12, 1, 0, 16, 2 };

        private Dictionary<int, (int last, int? previous)> LastSpoken = new Dictionary<int, (int last, int? previous)>();

        public void SolvePartOne()
        {
            var turn = 1;
            foreach (var i in input)
            {
                LastSpoken[i] = (turn, null);
                turn++;
            }

            var lastTurn = LastSpoken[2];
            var spoken = 2;

            while (turn <= 2020)
            {
                int next;
                if (lastTurn.previous.HasValue)
                {
                    next = lastTurn.last - lastTurn.previous.Value;
                }
                else
                {
                    next = 0;
                }

                if (LastSpoken.TryGetValue(next, out var res))
                {
                    LastSpoken[next] = (turn, res.last);
                }
                else
                {
                    LastSpoken[next] = (turn, null);
                }

                lastTurn = LastSpoken[next];
                spoken = next;
                turn++;
            }

            Console.WriteLine(spoken);
        }

        public void SolvePartTwo()
        {
            var turn = 1;
            foreach (var i in input)
            {
                LastSpoken[i] = (turn, null);
                turn++;
            }

            var lastTurn = LastSpoken[2];
            var spoken = 2;

            while (turn <= 30000000)
            {
                int next;
                if (lastTurn.previous.HasValue)
                {
                    next = lastTurn.last - lastTurn.previous.Value;
                }
                else
                {
                    next = 0;
                }

                if (LastSpoken.TryGetValue(next, out var res))
                {
                    LastSpoken[next] = (turn, res.last);
                }
                else
                {
                    LastSpoken[next] = (turn, null);
                }

                lastTurn = LastSpoken[next];
                spoken = next;
                turn++;
            }

            Console.WriteLine(spoken);
        }
    }
}
