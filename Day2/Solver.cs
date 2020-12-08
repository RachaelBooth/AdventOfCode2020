using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day2
{
    public class Solver : ISolver
    {
        private readonly IEnumerable<PasswordData> data;

        public Solver()
        {
            data = new InputReader<PasswordData>(2).ReadInputAsLines();
        }

        public void SolvePartOne()
        {
            var valid = data.Count(p => p.IsValidOldSystem());
            Console.WriteLine(valid);
        }

        public void SolvePartTwo()
        {
            var valid = data.Count(p => p.IsValidNewSystem());
            Console.WriteLine(valid);
        }
    }
}
