using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day1
{
    public class Solver : ISolver
    {
        private List<int> expenseReport;

        public Solver()
        {
            expenseReport = new InputReader<int>(1).ReadInput().ToList();
        }

        public void SolvePartOne()
        {
            var i = 0;
            while (i < expenseReport.Count)
            {
                var j = i + 1;
                while (j < expenseReport.Count)
                {
                    if (expenseReport[i] + expenseReport[j] == 2020)
                    {
                        var result = expenseReport[i] * expenseReport[j];
                        Console.WriteLine(result);
                        return;
                    }
                    j++;
                }
                i++;
            }
        }

        public void SolvePartTwo()
        {
            var i = 0;
            while (i < expenseReport.Count)
            {
                var j = i + 1;
                while (j < expenseReport.Count)
                {
                    var k = j + 1;
                    while (k < expenseReport.Count)
                    {
                        if (expenseReport[i] + expenseReport[j] + expenseReport[k] == 2020)
                        {
                            var result = expenseReport[i] * expenseReport[j] * expenseReport[k];
                            Console.WriteLine(result);
                            return;
                        }
                        k++;
                    }
                    j++;
                }
                i++;
            }
        }
    }
}
