using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day4
{
    public class Solver : ISolver
    {
        private List<Passport> passports = new List<Passport>();

        public Solver()
        {
            var input = new InputReader<string>(4).ReadInput();

            var current = new List<string>();
            foreach (var line in input)
            {
                if (line == "")
                {
                    passports.Add(new Passport(current));
                    current = new List<string>();
                }
                else
                {
                    current.Add(line);
                }
            }
            // Include the last one...
            passports.Add(new Passport(current));
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
