using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day4
{
    public class Solver : ISolver
    {
        private readonly IEnumerable<Passport> passports;

        public Solver()
        {
            passports = new InputReader<Passport>(4).ReadInputAsLineGroups();
        }

        public void SolvePartOne()
        {
            var result = passports.Count(p => p.FieldsArePresent());
            Console.WriteLine(result);
        }

        public void SolvePartTwo()
        {
            var result = passports.Count(p => p.IsValid());
            Console.WriteLine(result);
        }
    }
}
