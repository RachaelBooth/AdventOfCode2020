using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day5
{
    public class Solver : ISolver
    {
        public IEnumerable<string> passes;

        public Solver()
        {
            passes = new InputReader<string>(5).ReadInput();
        }

        public void SolvePartOne()
        {
            var result = passes.Select(SeatId).Max();
            Console.WriteLine(result);
        }

        public void SolvePartTwo()
        {
            var seatIds = passes.Select(SeatId).ToList();
            var max = seatIds.Max();
            var s = 0;
            while (s < max)
            {
                if (!seatIds.Contains(s))
                {
                    if (seatIds.Contains(s - 1) && seatIds.Contains(s + 1))
                    {
                        Console.WriteLine(s);
                        return;
                    }
                }

                s++;
            }
        }

        private int SeatId(string pass)
        {
            var binary = string.Join("", pass.ToCharArray().Select(c => c == 'F' || c == 'L' ? 0 : 1));
            return Convert.ToInt32(binary, 2);
        }
    }
}
