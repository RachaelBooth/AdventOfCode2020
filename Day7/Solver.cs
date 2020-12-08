using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day7
{
    public class Solver : ISolver
    {
        private readonly List<Rule> rules;

        public Solver()
        {
            rules = new InputReader<Rule>(7).ReadInputAsLines().ToList();
        }

        public void SolvePartOne()
        {
            var options = new List<string>();
            var toCheck = new List<string> { "shiny gold" };
            while (toCheck.Any())
            {
                var newContainers = toCheck.SelectMany(c => GetPotentialContainers(c));
                toCheck = new List<string>();
                foreach (var container in newContainers)
                {
                    if (!options.Contains(container))
                    {
                        options.Add(container);
                        toCheck.Add(container);
                    }
                }
            }
            Console.WriteLine(options.Count);
        }

        private IEnumerable<string> GetPotentialContainers(string bagColour)
        {
            return rules.Where(r => r.contents.Any(c => c.colour == bagColour)).Select(r => r.bagColour);
        }

        public void SolvePartTwo()
        {
            var result = CountContents("shiny gold");
            Console.WriteLine(result);
        }

        private int CountContents(string bagColour)
        {
            var rule = rules.Find(r => r.bagColour == bagColour);
            if (!rule.contents.Any())
            {
                return 0;
            }

            return rule.contents.Sum(c => c.quantity * (1 + CountContents(c.colour)));
        }
    }
}
