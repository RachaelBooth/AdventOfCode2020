using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AdventOfCode.Base;

namespace AdventOfCode2020.Day10
{
    public class Solver : ISolver
    {
        private List<int> adapters;

        public Solver()
        {
            adapters = new InputReader<int>(10).ReadInputAsLines().ToList();
            adapters.Sort();
        }

        public void SolvePartOne()
        {
            // Assume that all in increasing order is a valid chain using all the adapters

            var chain = adapters.Prepend(0).Append(adapters.Max() + 3).ToList();

            var i = 0;
            var oneJoltDifferences = 0;
            var threeJoltDifferences = 0;
            while (i < chain.Count - 1)
            {
                var diff = chain[i + 1] - chain[i];
                if (diff == 1)
                {
                    oneJoltDifferences++;
                }
                else if (diff == 3)
                {
                    threeJoltDifferences++;
                }
                else
                {
                    Console.WriteLine($"Diff wasn't one or three - check validity: {diff}");
                }

                i++;
            }

            var result = oneJoltDifferences * threeJoltDifferences;
            Console.WriteLine(result);
        }

        public void SolvePartTwo()
        {
            var currentEnds = new List<(int maxA, long ways)> {(0, 1)};

            // Always forced to use max adapter immediately before the device, so may as well just consider the adapters list

            var i = 0;
            while (i < adapters.Count)
            {
                var adapter = adapters[i];
                long ways = 0;
                var newArrangements = new List<(int maxA, long ways)>();
                foreach (var end in currentEnds)
                {
                    if (adapter - end.maxA <= 3)
                    {
                        ways = ways + end.ways;
                        // Keep version not including this adapter
                        newArrangements.Add(end);
                    }
                }
                // Include this adapter, with potentially several ways of having got there
                newArrangements.Add((adapter, ways));
                currentEnds = newArrangements;
                i++;
            }

            var result = currentEnds.First(a => a.maxA == adapters.Max());
            Console.WriteLine(result);
        }
    }
}
