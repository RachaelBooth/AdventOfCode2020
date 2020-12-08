using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Day3
{
    public class Solver : ISolver
    {
        private Map map;

        public Solver()
        {
            var mapLines = new InputReader<MapLine>(3).ReadInputAsLines();
            map = new Map(mapLines);
        }

        public void SolvePartOne()
        {
            var treeCount = CountTreesForSlope(3, 1);
            Console.WriteLine(treeCount);
        }

        public void SolvePartTwo()
        {
            var a = CountTreesForSlope(1, 1);
            var b = CountTreesForSlope(3, 1);
            var c = CountTreesForSlope(5, 1);
            var d = CountTreesForSlope(7, 1);
            var e = CountTreesForSlope(1, 2);
            var result = a * b * c * d * e;
            Console.WriteLine(result);
        }

        private int CountTreesForSlope(int right, int down)
        {
            var x = 0;
            var y = 0;
            var treeCount = 0;
            while (y <= map.maxY)
            {
                if (map.map[(x, y)])
                {
                    treeCount++;
                }

                x = x + right;
                if (x > map.maxX)
                {
                    x = x % (map.maxX + 1);
                }
                y = y + down;
            }

            return treeCount;
        }
    }
}
