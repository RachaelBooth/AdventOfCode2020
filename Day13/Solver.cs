using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day13
{
    public class Solver : ISolver
    {
        public void SolvePartOne()
        {
            var input = new InputReader<string>(13).ReadInputAsLines().ToList();
            var minTimestamp = int.Parse(input[0]);
            var busesInService = input[1].Split(',').Where(x => x != "x").Select(int.Parse).ToList();

            var t = minTimestamp;
            while (true)
            {
                foreach (var bus in busesInService)
                {
                    if (t % bus == 0)
                    {
                        // First bus
                        var minutesWaiting = t - minTimestamp;
                        var result = minutesWaiting * bus;
                        Console.WriteLine(result);
                        return;
                    }
                }

                t++;
            }
        }

        public void SolvePartTwo()
        {
            var input = new InputReader<string>(13).ReadInputAsLines().ToList();
            var busesSchedule = input[1].Split(',').ToList();
       //   var busesSchedule = "67,7,x,59,61".Split(',').ToList();

            var constraints = new List<(long value, long modulus)>();

            var i = 0;
            while (i < busesSchedule.Count)
            {
                if (busesSchedule[i] != "x")
                {
                    var modulus = int.Parse(busesSchedule[i]);
                    var value = (modulus - i) % modulus;
                    constraints.Add((value, modulus));
                }

                i++;
            }

            // Chinese remainder theorem assumes moduli are pairwise coprime. 
            // Input probably "just happens" to fit this, will reassess if answer is wrong/see a higher GCD warning
            var congruence = ReduceModularEquations(constraints);

            congruence.value = congruence.value % congruence.modulus;

            while (congruence.value < 0)
            {
                congruence.value = congruence.value + congruence.modulus;
            }

            Console.WriteLine(congruence.value);
        }

        private (long value, long modulus) ReduceModularEquations(List<(long value, long modulus)> congruences)
        {
            if (congruences.Count == 1)
            {
                return congruences.First();
            }

            var reduced = ReduceModularEquationPair(congruences[0], congruences[1]);
            var updated = congruences.ToList();
            updated.RemoveRange(0, 2);
            updated.Add(reduced);
            return ReduceModularEquations(updated);
        }

        private (long value, long modulus) ReduceModularEquationPair(
            (long value, long modulus) first,
            (long value, long modulus) second)
        {
            if (first.modulus < second.modulus)
            {
                var temp = first;
                first = second;
                second = temp;
            }

            checked
            {
                var bezoutCoefficients = FindBezoutCoefficients(first.modulus, second.modulus);
                var modulus = first.modulus * second.modulus;
                // So I should have made everything big ints because it turns out the multiplications for this intermediate value get too large,
                // But I'd got this far
                // Should've just plugged the system into an existing calculator, tbh, but kinda enjoying doing the implementation until it got to the tracking down the error point
                var value = TimesInMod(modulus, first.value, bezoutCoefficients.t, second.modulus) + TimesInMod(modulus, second.value, bezoutCoefficients.s, first.modulus);
                return (value % modulus, modulus);
            }
        }

        private long TimesInMod(long modulus, params long[] values)
        {
            if (values.Length > 2)
            {
                return values.Aggregate((c, next) => TimesInMod(modulus, c, next));
            }

            if (values.Length == 1)
            {
                return values[0] % modulus;
            }
            
            if (values.Length == 0)
            {
                throw new Exception("Why?");
            }

            checked
            {
                long current = 0;
                long i = 1;
                while (i <= Math.Abs(values[1]))
                {
                    current = (current + values[0]) % modulus;
                    i++;
                }

                if (values[1] < 0)
                {
                    current = -current;
                }

                return current;
            }
        }

        private (long s, long t) FindBezoutCoefficients(long a, long b)
        {
            checked
            {
                var R = new List<long> {a, b};
                // 0 is a dummy value to keep q_i being Q[i] (series of quotients starts with q_1)
                var Q = new List<long> {0};
                var S = new List<long> {1, 0};
                var T = new List<long> {0, 1};

                var i = 1;
                while (true)
                {
                    R.Add(R[i - 1] % R[i]);
                    if (R[i + 1] == 0)
                    {
                        // Done
                        if (R[i] != 1)
                        {
                            Console.WriteLine("WARNING: GCD not 1");
                        }

                        return (S[i], T[i]);
                    }

                    Q.Add((R[i - 1] - R[i + 1]) / R[i]);
                    S.Add(S[i - 1] - Q[i] * S[i]);
                    T.Add(T[i - 1] - Q[i] * T[i]);
                    i++;
                }
            }
        }
    }
}
