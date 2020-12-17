﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.GridHelpers
{
    public static class LocationTupleExtensions
    {
        public static (int x, int y, int z) ToThreeDimensions(this (int x, int y) location)
        {
            return (location.x, location.y, 0);
        }

        public static (int x, int y, int z, int w) ToFourDimensions(this (int x, int y) location)
        {
            return (location.x, location.y, 0, 0);
        }

        public static (int x, int y, int z) Plus(this (int x, int y, int z) location, (int x, int y, int z) summand)
        {
            return (location.x + summand.x, location.y + summand.y, location.z + summand.z);
        }

        public static (int x, int y, int z) Minus(this (int x, int y, int z) location, (int x, int y, int z) subtrahend)
        {
            return location.Plus(subtrahend.Times(-1));
        }

        public static (int x, int y, int z) Times(this (int x, int y, int z) location, (int x, int y, int z) multiplicand)
        {
            return (location.x * multiplicand.x, location.y * multiplicand.y, location.z * multiplicand.z);
        }

        public static (int x, int y, int z) Times(this (int x, int y, int z) location, int multiplicand)
        {
            return location.Times((multiplicand, multiplicand, multiplicand));
        }

        public static IEnumerable<(int x, int y, int z)> NeighbouringLocations(this (int x, int y, int z) location)
        {
            var basicDiffs = new List<(int x, int y, int z)> { (1, 0, 0), (0, 1, 0), (0, 0, 1), (1, 1, 0), (1, 0, 1), (0, 1, 1), (1, 1, 1), (1, -1, 0), (1, 0, -1), (0, 1, -1), (1, -1, 1), (1, 1, -1), (1, -1, -1) };
            return basicDiffs.SelectMany(d => new List<(int x, int y, int z)> { location.Plus(d), location.Minus(d) });
        }
    }

    public static class FourDimensionalLocationExtensions
    {
        public static (int x, int y, int z, int w) Plus(this (int x, int y, int z, int w) location, (int x, int y, int z, int w) summand)
        {
            return (location.x + summand.x, location.y + summand.y, location.z + summand.z, location.w + summand.w);
        }

        public static (int x, int y, int z, int w) Minus(this (int x, int y, int z, int w) location, (int x, int y, int z, int w) subtrahend)
        {
            return location.Plus(subtrahend.Times(-1));
        }

        public static (int x, int y, int z, int w) Times(this (int x, int y, int z, int w) location, (int x, int y, int z, int w) multiplicand)
        {
            return (location.x * multiplicand.x, location.y * multiplicand.y, location.z * multiplicand.z, location.w * multiplicand.w);
        }

        public static (int x, int y, int z, int w) Times(this (int x, int y, int z, int w) location, int multiplicand)
        {
            return location.Times((multiplicand, multiplicand, multiplicand, multiplicand));
        }

        public static IEnumerable<(int x, int y, int z, int w)> NeighbouringLocations(this (int x, int y, int z, int w) location)
        {
            return (location.x, location.y, location.z).NeighbouringLocations()
                .Select(l => (l.x, l.y, l.z, location.w))
                .SelectMany(
                    l => new List<(int x, int y, int z, int w)> { l, l.Plus((0, 0, 0, 1)), l.Minus((0, 0, 0, 1)) })
                .Append(location.Plus((0, 0, 0, 1)))
                .Append(location.Minus((0, 0, 0, 1)));
        }
    }
}