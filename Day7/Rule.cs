using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day7
{
    public class Rule
    {
        public string bagColour;
        public List<(int quantity, string colour)> contents;

        public Rule(string bagColour, List<(int quantity, string colour)> contents)
        {
            this.bagColour = bagColour;
            this.contents = contents;
        }

        public static Rule Parse(string line)
        {
            var parts = line.Split(" contain ");
            var bagColour = parts[0].Substring(0, parts[0].Length - " bags".Length);
            var contents = new List<(int quantity, string colour)>();
            foreach (var bagDescription in parts[1].TrimEnd('.').Split(", "))
            {
                if (bagDescription != "no other bags")
                {
                    var descriptionParts = bagDescription.Split(' ');
                    var number = int.Parse(descriptionParts[0]);
                    var colour = string.Join(" ", descriptionParts.Skip(1).TakeWhile(p => p != "bag" && p != "bags"));
                    contents.Add((number, colour));
                }
            }
            return new Rule(bagColour, contents);
        }
    }
}
