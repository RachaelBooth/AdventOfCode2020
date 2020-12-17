using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day16
{
    public class Solver : ISolver
    {
        private List<TicketField> TicketFields = new List<TicketField>();
        private List<int> YourTicket = new List<int>();
        private List<List<int>> NearbyTickets = new List<List<int>>();

        public Solver()
        {
            var input = new InputReader<string>(16).ReadInputAsLines();

            var inTicketFields = true;
            var inYourTicket = false;

            foreach (var line in input)
            {
                if (inTicketFields)
                {
                    if (line != "")
                    {
                        TicketFields.Add(TicketField.Parse(line));
                    }
                    else
                    {
                        inTicketFields = false;
                        inYourTicket = true;
                    }
                }
                else if (inYourTicket)
                {
                    if (line == "")
                    {
                        inYourTicket = false;
                    }
                    else if (line != "your ticket:")
                    {
                        YourTicket = line.Split(",").Select(int.Parse).ToList();
                    }
                }
                else
                {
                    if (line != "nearby tickets:")
                    {
                        NearbyTickets.Add(line.Split(",").Select(int.Parse).ToList());
                    }
                }

            }
        }

        public void SolvePartOne()
        {
            var invalidValues = NearbyTickets.SelectMany(t => t.Where(v => TicketFields.All(f => !f.IsInRange(v))));
            var result = invalidValues.Sum();
            Console.WriteLine(result);
        }

        public void SolvePartTwo()
        {
            var validTickets = NearbyTickets.Where(t => t.All(v => TicketFields.Any(f => f.IsInRange(v)))).ToList();
            validTickets.Add(YourTicket);

            var possibleFieldPositions = new List<(TicketField field, List<int> possiblePositions)>();
            var fixedPositions = new Dictionary<string, int>();
            foreach (var field in TicketFields)
            {
                var possiblePositions = new List<int>();
                var i = 0;
                while (i < validTickets[0].Count)
                {
                    if (!fixedPositions.Values.Contains(i))
                    {
                        if (validTickets.All(t => field.IsInRange(t[i])))
                        {
                            possiblePositions.Add(i);
                        }
                    }
                    i++;
                }
                if (possiblePositions.Count == 1)
                {
                    fixedPositions.Add(field.Field, i);
                }
                else
                {
                    possibleFieldPositions.Add((field, possiblePositions));
                }
            }

            // Assume that there is a unique solution, because puzzle input will be chosen that way
            while (possibleFieldPositions.Any())
            {
                // Try restricting by fields with only one valid index
                var unfixed = new List<(TicketField field, List<int> possiblePositions)>();
                foreach (var p in possibleFieldPositions)
                {
                    var restrictedPositions = p.possiblePositions.Where(pos => !fixedPositions.Values.Contains(pos)).ToList();
                    if (restrictedPositions.Count == 1)
                    {
                        fixedPositions.Add(p.field.Field, restrictedPositions[0]);
                    }
                    else
                    {
                        unfixed.Add((p.field, restrictedPositions));
                    }
                }
                possibleFieldPositions = unfixed;
                // Now try restricted by indices only appearing once
                var i = 0;
                while (i < YourTicket.Count)
                {
                    if (!fixedPositions.ContainsValue(i))
                    {
                        var fieldsWithIValid = possibleFieldPositions.Where(p => p.possiblePositions.Contains(i)).ToList();
                        if (fieldsWithIValid.Count == 1)
                        {
                            fixedPositions.Add(fieldsWithIValid[0].field.Field, i);
                            possibleFieldPositions = possibleFieldPositions.Where(p => p.field != fieldsWithIValid[0].field).Select(p =>
                            {
                                if (p.possiblePositions.Contains(i))
                                {
                                    return (p.field, p.possiblePositions.Where(pos => pos != i).ToList());
                                }
                                return p;
                            }).ToList();
                        }
                    }
                    i++;
                }
            }

            var departureKeys = fixedPositions.Keys.Where(k => k.StartsWith("departure"));
            var departureIndices = departureKeys.Select(k => fixedPositions[k]);
            var result = departureIndices.Aggregate((long) 1, (current, next) => ((long) YourTicket[next]) * current);
            Console.WriteLine(result);
        }
    }

    public class TicketField
    {
        private List<(int min, int max)> ValidRanges;
        public string Field;

        public TicketField(string field, IEnumerable<(int min, int max)> validRanges)
        {
            Field = field;
            ValidRanges = validRanges.ToList();
        }

        public bool IsInRange(int value)
        {
            return ValidRanges.Any(r => value >= r.min && value <= r.max);
        }

        public static TicketField Parse(string line)
        {
            var parts = line.Split(": ");
            var field = parts[0];
            var ranges = parts[1].Split(" ").Where(p => p != "or").Select(p =>
            {
                var split = p.Split('-');
                return (int.Parse(split[0]), int.Parse(split[1]));
            });
            return new TicketField(field, ranges);
        }
    }
}
