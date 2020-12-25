using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Base;
using AdventOfCode.Base.Vectors;

namespace AdventOfCode2020.Day17
{
    public class Solver : ISolver
    {
        public void SolvePartOne()
        {
            var currentCubes = new InputReader(17).Parse3DimensionalGrid(c => c == '#');
            var i = 0;
            while (i < 6)
            {
                currentCubes = RunCycle(currentCubes);
                i++;
            }

            var result = currentCubes.Count(c => c.Value);
            Console.WriteLine(result);
        }

        public void SolvePartTwo()
        {
            var currentCubes = new InputReader(17).Parse4DimensionalGrid(c => c == '#');
            var i = 0;
            while (i < 6)
            {
                currentCubes = RunCycle(currentCubes);
                i++;
            }

            var result = currentCubes.Count(c => c.Value);
            Console.WriteLine(result);
        }

        private Dictionary<(int x, int y, int z), bool> RunCycle(Dictionary<(int x, int y, int z), bool> currentCubes)
        {
            return RunCycle(currentCubes, l => l.NeighbouringLocations());
        }

        private Dictionary<(int x, int y, int z, int w), bool> RunCycle(Dictionary<(int x, int y, int z, int w), bool> currentCubes)
        {
            return RunCycle(currentCubes, l => l.NeighbouringLocations());
        }

        private Dictionary<T, bool> RunCycle<T>(Dictionary<T, bool> currentCubes, Func<T, IEnumerable<T>> getNeighbours)
        {
            var newCubes = new Dictionary<T, bool>();
            var cubesToConsider = currentCubes.Where(c => c.Value).SelectMany(c => getNeighbours(c.Key)).Distinct();
            foreach (var cube in cubesToConsider)
            {
                var activeNeighbourCount = getNeighbours(cube).Count(l => IsActive(l, currentCubes));
                // Active if currently active and 2 or 3 neighbours or currently inactive and exactly 3
                if ((IsActive(cube, currentCubes) && activeNeighbourCount == 2) || activeNeighbourCount == 3)
                {
                    newCubes.Add(cube, true);
                }
                else
                {
                    // Could leave this out actually - have kinda assumed dictionary contains some of each, but...
                    newCubes.Add(cube, false);
                }
            }

            return newCubes;
        }

        private bool IsActive<T>(T location, Dictionary<T, bool> currentCubeDictionary)
        {
            if (currentCubeDictionary.ContainsKey(location))
            {
                return currentCubeDictionary[location];
            }

            return false;
        }
    }
}
