using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Base;
using AdventOfCode.Base.Vectors;

namespace AdventOfCode2020.Day24
{
    public class Solver : ISolver
    {
        public void SolvePartOne()
        {
            var tilesFlipped = GetBlackTilesAfterFollowingInstructions();
            Console.WriteLine(tilesFlipped.Count);
        }
        
        public void SolvePartTwo()
        {
            var blackTiles = GetBlackTilesAfterFollowingInstructions();
            var i = 0;
            while (i < 100)
            {
                blackTiles = RunStep(blackTiles);
                i++;
            }
            Console.WriteLine(blackTiles.Count);
        }

        private List<(int n, int e)> GetBlackTilesAfterFollowingInstructions()
        {
            var instructions = new InputReader(24).ReadInputAsLines();
            var tilesFlipped = new List<(int n, int e)>();
            foreach (var instruction in instructions)
            {
                var tile = (0, 0);
                var i = 0;
                while (i < instruction.Length)
                {
                    var c = instruction[i];
                    if (c == 'e')
                    {
                        tile = tile.Plus((0, 2));
                        i++;
                    }
                    else if (c == 'w')
                    {
                        tile = tile.Plus((0, -2));
                        i++;
                    }
                    else
                    {
                        if (c == 'n')
                        {
                            tile = tile.Plus((1, 0));
                        }
                        else
                        {
                            tile = tile.Plus((-1, 0));
                        }

                        var second = instruction[i + 1];
                        if (second == 'e')
                        {
                            tile = tile.Plus((0, 1));
                        }
                        else
                        {
                            tile = tile.Plus((0, -1));
                        }

                        i = i + 2;
                    }
                }

                if (tilesFlipped.Contains(tile))
                {
                    tilesFlipped.Remove(tile);
                }
                else
                {
                    tilesFlipped.Add(tile);
                }
            }

            return tilesFlipped;
        }

        private List<(int n, int e)> RunStep(List<(int n, int e)> currentBlackTiles)
        {
            var blackTilesAfterStep = new List<(int n, int e)>();
            var tilesToConsider = currentBlackTiles.SelectMany(GetNeighbours).Distinct();

            foreach (var tile in tilesToConsider)
            {
                var currentBlackNeighbourCount = GetNeighbours(tile).Count(currentBlackTiles.Contains);
                if ((currentBlackTiles.Contains(tile) && currentBlackNeighbourCount == 1) || currentBlackNeighbourCount == 2)
                {
                    blackTilesAfterStep.Add(tile);
                }
            }

            return blackTilesAfterStep;
        }

        private List<(int n, int e)> GetNeighbours((int n, int e) tile)
        {
            return new List<(int n, int e)>
            {
                (tile.n, tile.e + 2), (tile.n, tile.e - 2), (tile.n + 1, tile.e + 1), (tile.n + 1, tile.e - 1), (tile.n - 1, tile.e + 1),
                (tile.n - 1, tile.e - 1)
            };
        }
    }
}
