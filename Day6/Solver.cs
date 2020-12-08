using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day6
{
    public class Solver : ISolver
    {
        public Solver()
        {

        }

        public void SolvePartOne()
        {
            var input = new InputReader<string>(6).ReadInputAsLines();

            var groups = new List<List<char>>();

            var current = new List<char>();
            foreach (var line in input)
            {
                if (line == "")
                {
                    groups.Add(current.Distinct().ToList());
                    current = new List<char>();
                }
                else
                {
                    current.AddRange(line.ToCharArray());
                }
            }
            // Include the last one...
            groups.Add(current.Distinct().ToList());

            var result = groups.Sum(g => g.Distinct().ToList().Count);
            Console.WriteLine(result);
        }

        public void SolvePartTwo()
        {
            var input = new InputReader<string>(6).ReadInputAsLines();

            var countSum = 0;

            var current = Enumerable.Range('a', 26).Select(c => (char)c).ToList();
            foreach (var line in input)
            {
                if (line == "")
                {
                    countSum = countSum + current.Count;
                    current = Enumerable.Range('a', 26).Select(c => (char)c).ToList();
                }
                else
                {
                    current = current.Intersect(line.ToCharArray()).ToList();
                }
            }
            // Include the last one...
            countSum = countSum + current.Count;

            Console.WriteLine(countSum);
        }
    }
}
