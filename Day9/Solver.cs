using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Base;

namespace AdventOfCode2020.Day9
{
    public class Solver : ISolver
    {
        private readonly IEnumerable<long> xmasData;

        public Solver()
        {
           xmasData = new InputReader<long>(9).ReadInputAsLines();
        }

        public void SolvePartOne()
        {
            var result = FindNonSum(xmasData);
            Console.WriteLine(result);
        }
        
        public void SolvePartTwo()
        {
            var data = xmasData.ToList();
            var nonSum = FindNonSum(data);
            var sumLocation = FindContiguousSum(nonSum, data);
            var result = FindEncryptionWeakness(data.GetRange(sumLocation.start, sumLocation.count));
            Console.WriteLine(result);
        }

        private long FindEncryptionWeakness(List<long> contiguousSum)
        {
            return contiguousSum.Min() + contiguousSum.Max();
        }

        private (int start, int count) FindContiguousSum(long desiredSum, List<long> data)
        {
            var i = 0;
            while (i < data.Count)
            {
                var s = data[i];
                var j = 1;
                // Note: all input numbers are > 0
                while (i + j < data.Count && s < desiredSum)
                {
                    s = s + data[i + j];
                    j = j + 1;
                }

                if (s == desiredSum)
                {
                    return (i, j);
                }

                i++;
            }
            throw new Exception("No contigious sum found");
        }

        private long FindNonSum(IEnumerable<long> data)
        {
            var previousSection = data.Take(25).ToList();

            foreach (var number in data.Skip(25))
            {
                if (!IsSumOfPair(number, previousSection))
                {
                    return number;
                }

                previousSection.RemoveAt(0);
                previousSection.Add(number);
            }
            throw new Exception("Didn't find a non sum - we assume there is one in the data otherwise the puzzle wouldn't work");
        }

        private bool IsSumOfPair(long number, List<long> section)
        {
            var i = 0;
            while (i < section.Count - 1)
            {
                var j = i + 1;
                while (j < section.Count)
                {
                    if (section[i] != section[j] && section[i] + section[j] == number)
                    {
                        return true;
                    }

                    j++;
                }

                i++;
            }

            return false;
        }
    }
}
