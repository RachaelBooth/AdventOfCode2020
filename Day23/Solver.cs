using System;
using System.Collections.Generic;

namespace AdventOfCode2020.Day23
{
    public class Solver : ISolver
    {
        public void SolvePartOne()
        {
            var cupCircle = new Dictionary<int, int>();
            cupCircle.Add(3, 8);
            cupCircle.Add(8, 9);
            cupCircle.Add(9, 5);
            cupCircle.Add(5, 4);
            cupCircle.Add(4, 7);
            cupCircle.Add(7, 6);
            cupCircle.Add(6, 1);
            cupCircle.Add(1, 2);
            cupCircle.Add(2, 3);

            var currentCup = 3;

            var move = 0;
            while (move < 100)
            {
                var first = cupCircle[currentCup];
                var second = cupCircle[first];
                var third = cupCircle[second];

                var destinationCup = currentCup - 1;
                if (destinationCup == 0)
                {
                    destinationCup = 9;
                }

                while (destinationCup == first || destinationCup == second || destinationCup == third)
                {
                    destinationCup = destinationCup - 1;
                    if (destinationCup == 0)
                    {
                        destinationCup = 9;
                    }
                }

                var next = cupCircle[third];
                cupCircle[currentCup] = next;
                cupCircle[third] = cupCircle[destinationCup];
                cupCircle[destinationCup] = first;
                currentCup = next;

                move++;
            }

            var i = cupCircle[1];
            while (i != 1)
            {
                Console.Write(i);
                i = cupCircle[i];
            }
        }

        public void SolvePartTwo()
        {
            var cupCircle = new Dictionary<int, int>();
            cupCircle.Add(3, 8);
            cupCircle.Add(8, 9);
            cupCircle.Add(9, 5);
            cupCircle.Add(5, 4);
            cupCircle.Add(4, 7);
            cupCircle.Add(7, 6);
            cupCircle.Add(6, 1);
            cupCircle.Add(1, 2);
            cupCircle.Add(2, 10);
            var c = 10;
            while (c < 1000000)
            {
                cupCircle.Add(c, c + 1);
                c++;
            }
            cupCircle.Add(1000000, 3);

            var currentCup = 3;

            var move = 0;
            while (move < 10000000)
            {
                var first = cupCircle[currentCup];
                var second = cupCircle[first];
                var third = cupCircle[second];

                var destinationCup = currentCup - 1;
                if (destinationCup == 0)
                {
                    destinationCup = 1000000;
                }

                while (destinationCup == first || destinationCup == second || destinationCup == third)
                {
                    destinationCup = destinationCup - 1;
                    if (destinationCup == 0)
                    {
                        destinationCup = 1000000;
                    }
                }

                var next = cupCircle[third];
                cupCircle[currentCup] = next;
                cupCircle[third] = cupCircle[destinationCup];
                cupCircle[destinationCup] = first;
                currentCup = next;

                move++;
            }

            var a = cupCircle[1];
            var b = cupCircle[a];
            var result = ((long) a) * ((long) b);
            Console.WriteLine(result);
        }
    }
}
