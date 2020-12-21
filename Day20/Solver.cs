using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2020.GridHelpers;

namespace AdventOfCode2020.Day20
{
    public class Solver : ISolver
    {
        private Dictionary<long, Tile> Tiles;
        private int SquareSize;
        private int xOffset;
        private int yOffset;
        private int tileContentSize;

        public Solver()
        {
            Tiles = new InputReader<Tile>(20, useTestFilePath: false).ReadInputAsLineGroups().ToDictionary(t => t.id);
            // We do know that the input should have a square number of tiles...
            SquareSize = (int) Math.Sqrt(Tiles.Count);
        }

        public void SolvePartOne()
        {
            var square = GetSquare();
            var minX = square.Keys.Min(k => k.x);
            var minY = square.Keys.Min(k => k.y);
            var maxX = square.Keys.Max(k => k.x);
            var maxY = square.Keys.Max(k => k.y);
            var result = square[(minX, minY)].id * square[(minX, maxY)].id * square[(maxX, minY)].id *
                         square[(maxX, maxY)].id;
            Console.WriteLine(result);
        }

        public void SolvePartTwo()
        {
            foreach (var initialTransform in Enumerable.Range(0, 4)
                .SelectMany(r => new List<(bool flip, int rotation)> {(true, r), (false, r)}))
         //   foreach (var initialTransform in new List<(bool flip, int rotation)> { (false, 0), (true, 0), (true, 1), (false, 1), (false, 3) })
            {
                Console.WriteLine($"Initial tile transform: {initialTransform.flip}, {initialTransform.rotation}");
                var square = GetSquare(initialTransform.flip, initialTransform.rotation);

                xOffset = square.Keys.Min(k => k.x);
                yOffset = square.Keys.Min(k => k.y);
                tileContentSize = Tiles.Values.First().GetEdge(1, false, 0).Length - 2;

               /* var y = 0;
                while (y < tileContentSize * SquareSize)
                {
                    var x = 0;
                    while (x < tileContentSize * SquareSize)
                    {
                        var tileLocation = ((x / tileContentSize) + xOffset, (y / tileContentSize) + yOffset);
                        var locationInTile = (x % tileContentSize, y % tileContentSize);
                        var tileDetails = square[tileLocation];
                        Console.Write(Tiles[tileDetails.id]
                            .GetContentAt(locationInTile, tileDetails.flip, tileDetails.rotation));

                        x++;
                    }
                    Console.Write("\r\n");
                    y++;
                } */

                //                   # 
                // #    ##    ##    ###
                //  #  #  #  #  #  #   

                var seaMonster = new List<(int x, int y)>
                {
                    (0, 0), (1, 1), (4, 1), (5, 0), (6, 0), (7, 1), (10, 1), (11, 0), (12, 0), (13, 1), (16, 1),
                    (17, 0), (18, 0), (19, 0), (18, -1)
                };
                foreach (var monsterOrientation in Enumerable.Range(0, 4)
                    .SelectMany(r => new List<(bool flip, int rotation)> {(true, r), (false, r)}))
                {
                    var pattern = TransformPattern(seaMonster, monsterOrientation.flip, monsterOrientation.rotation);
                    var matchStarts = FindPatternStarts(pattern, square);
                    if (matchStarts.Any())
                    {
                        var monsterParts = matchStarts.SelectMany(matchStart => pattern.Select(p => matchStart.Plus(p)))
                            .ToList();
                        var roughness = Enumerable.Range(0, SquareSize * tileContentSize)
                            .SelectMany(x => Enumerable.Range(0, SquareSize * tileContentSize).Select(y => (x, y)))
                            .Where(l => !monsterParts.Contains(l))
                            .Count(l => ContentIsHash(l, square));
                        Console.WriteLine($"Monster transform: {monsterOrientation.flip}, {monsterOrientation.rotation}");
                        Console.WriteLine($"{roughness} ({matchStarts.Count} monsters)");
                    }
                }
            }
        }

        private List<(int x, int y)> TransformPattern(List<(int x, int y)> pattern, bool flip, int rotation)
        {
            return pattern.Select(l =>
            {
                var newX = l.x;
                var newY = l.y;
                if (flip)
                {
                    newX = -newX;
                }

                switch (rotation)
                {
                    case 0:
                        break;
                    case 1:
                        var tmp1 = newX;
                        newX = newY;
                        newY = -tmp1;
                        break;
                    case 2:
                        newX = -newX;
                        newY = -newY;
                        break;
                    case 3:
                        var tmp2 = newX;
                        newX = -newY;
                        newY = tmp2;
                        break;
                    default:
                        throw new Exception("Unexpected rotation");
                }

                return (newX, newY);
            }).ToList();
        }

        private List<(int x, int y)> FindPatternStarts(List<(int x, int y)> pattern, Dictionary<(int x, int y), (long id, bool flip, int rotation)> tileSquare)
        {
            return Enumerable.Range(0, SquareSize * tileContentSize).SelectMany(x =>
                    Enumerable.Range(0, SquareSize * tileContentSize).Select(y => (x, y)))
                .Where(l => IsPatternStart(l, pattern, tileSquare)).ToList();
        }

        private bool IsPatternStart((int x, int y) location, List<(int x, int y)> pattern, Dictionary<(int x, int y), (long id, bool flip, int rotation)> tileSquare)
        {
            return pattern.All(p => ContentIsHash(location.Plus(p), tileSquare));
        }

        // QQ - possibly dodgy? (i.e. sometimes not finding # when it should do?)
        private bool ContentIsHash((int x, int y) location, Dictionary<(int x, int y), (long id, bool flip, int rotation)> tileSquare)
        {
            if (location.x < 0 || location.y < 0 || location.x >= SquareSize * tileContentSize || location.y >= SquareSize * tileContentSize)
            {
                return false;
            }

            var tileLocation = ((location.x / tileContentSize) + xOffset, (location.y / tileContentSize) + yOffset);
            if (!tileSquare.ContainsKey(tileLocation))
            {
                return false;
            }

            var locationInTile = (location.x % tileContentSize, location.y % tileContentSize);
            var tileDetails = tileSquare[tileLocation];
            return Tiles[tileDetails.id].GetContentAt(locationInTile, tileDetails.flip, tileDetails.rotation) == '#';
        }

        private Dictionary<(int x, int y), (long id, bool flip, int rotation)> GetSquare(bool initialFlip = false, int initialRotation = 0)
        {
            var assembledTilesOptions = new List<Dictionary<(int x, int y), (long id, bool flip, int rotation)>>();

            // Pick initial tile - wlog this is at (0, 0) and no transformation (and we allow this point to be anywhere in the square)
            var initial = Tiles[Tiles.Keys.First()];
            // Try this for ease of testing with the test input?
            var initialDict = new Dictionary<(int x, int y), (long id, bool flip, int rotation)> { { (0, 0), (initial.id, initialFlip, initialRotation) } };

            assembledTilesOptions.Add(initialDict);

            while (assembledTilesOptions[0].Count < Tiles.Count)
            {
                // Eww, naming
                var newOptions = new List<Dictionary<(int x, int y), (long id, bool flip, int rotation)>>();
                foreach (var currentOption in assembledTilesOptions)
                {
                    var nextOptions = FindPotentialNext(currentOption);
                    foreach (var n in nextOptions)
                    {
                        if (!newOptions.Any(no => IsEqual(no, n)))
                        {
                            newOptions.Add(n);
                        }
                    }
                }

                assembledTilesOptions = newOptions;
            }

            return assembledTilesOptions[0];
        }

        private bool IsEqual(Dictionary<(int x, int y), (long id, bool flip, int rotation)> first,
            Dictionary<(int x, int y), (long id, bool flip, int rotation)> second)
        {
            if (first.Count != second.Count)
            {
                return false;
            }

            if (first.Keys.Any(k => !second.ContainsKey(k)))
            {
                return false;
            }

            return first.Keys.All(k => first[k] == second[k]);
        }

        private List<Dictionary<(int x, int y), (long id, bool flip, int rotation)>> FindPotentialNext(Dictionary<(int x, int y), (long id, bool flip, int rotation)> current)
        {
            var unusedTiles = Tiles.Keys.Where(k => current.Values.All(v => v.id != k));
            var options = new List<Dictionary<(int x, int y), (long id, bool flip, int rotation)>>();
            var zeroRowMin = current.Keys.Where(k => k.y == 0).Min(k => k.x);
            var zeroRowMax = current.Keys.Where(k => k.y == 0).Max(k => k.x);
            var zeroRowLength = zeroRowMax - zeroRowMin + 1;
            if (zeroRowLength < SquareSize)
            {
                foreach (var tileId in unusedTiles)
                {
                    var tile = Tiles[tileId];
                    var validWaysToFitRight = ValidTransformationsToFit(tile, (zeroRowMax + 1, 0), current);
                    foreach (var transformation in validWaysToFitRight)
                    {
                        var updated = new Dictionary<(int x, int y), (long id, bool flip, int rotation)>(current);
                        updated.Add((zeroRowMax + 1, 0), (tileId, transformation.flip, transformation.rotation));
                        options.Add(updated);
                    }

                    var validWaysToFitLeft = ValidTransformationsToFit(tile, (zeroRowMin - 1, 0), current);
                    foreach (var transformation in validWaysToFitLeft)
                    {
                        var updated = new Dictionary<(int x, int y), (long id, bool flip, int rotation)>(current);
                        updated.Add((zeroRowMin - 1, 0), (tileId, transformation.flip, transformation.rotation));
                        options.Add(updated);
                    }
                }

                return options;
            }

            var rowsToFill = current.Keys.Select(k => k.y)
                .Where(y => current.Keys.Count(k => k.y == y) < zeroRowLength).ToList();
            if (rowsToFill.Any())
            {
                var y = rowsToFill.First();
                var x = current.Keys.Where(k => k.y == y).Max(k => k.x) + 1;
                var locationToFill = (x, y);
                foreach (var tileId in unusedTiles)
                {
                    foreach (var transformation in ValidTransformationsToFit(Tiles[tileId], locationToFill, current))
                    {
                        var updated = new Dictionary<(int x, int y), (long id, bool flip, int rotation)>(current);
                        updated.Add(locationToFill, (tileId, transformation.flip, transformation.rotation));
                        options.Add(updated);
                    }
                }

                return options;
            }

            var minY = current.Keys.Min(k => k.y);
            var maxY = current.Keys.Max(k => k.y);

            foreach (var tileId in unusedTiles)
            {
                foreach (var transformation in ValidTransformationsToFit(Tiles[tileId], (zeroRowMin, maxY + 1), current))
                {
                    var updated = new Dictionary<(int x, int y), (long id, bool flip, int rotation)>(current);
                    updated.Add((zeroRowMin, maxY + 1), (tileId, transformation.flip, transformation.rotation));
                    options.Add(updated);
                }

                foreach (var transformation in ValidTransformationsToFit(Tiles[tileId], (zeroRowMin, minY - 1), current))
                {
                    var updated = new Dictionary<(int x, int y), (long id, bool flip, int rotation)>(current);
                    updated.Add((zeroRowMin, minY - 1), (tileId, transformation.flip, transformation.rotation));
                    options.Add(updated);
                }
            }

            return options;
        }

        private List<(bool flip, int rotation)> ValidTransformationsToFit(Tile tile, (int x, int y) location, Dictionary<(int x, int y), (long id, bool flip, int rotation)> current)
        {
            var fixedEdges = new List<(int edgeKey, string edge)>();
            if (current.ContainsKey((location.x, location.y - 1)))
            {
                var detail = current[(location.x, location.y - 1)];
                var edgeAbove = Tiles[detail.id].GetEdge(3, detail.flip, detail.rotation);
                fixedEdges.Add((1, edgeAbove));
            }

            if (current.ContainsKey((location.x, location.y + 1)))
            {
                var detail = current[(location.x, location.y + 1)];
                var edgeBelow = Tiles[detail.id].GetEdge(1, detail.flip, detail.rotation);
                fixedEdges.Add((3, edgeBelow));
            }

            if (current.ContainsKey((location.x + 1, location.y)))
            {
                var detail = current[(location.x + 1, location.y)];
                var edgeRight = Tiles[detail.id].GetEdge(4, detail.flip, detail.rotation);
                fixedEdges.Add((2, edgeRight));
            }

            if (current.ContainsKey((location.x - 1, location.y)))
            {
                var detail = current[(location.x - 1, location.y)];
                var edgeLeft = Tiles[detail.id].GetEdge(2, detail.flip, detail.rotation);
                fixedEdges.Add((4, edgeLeft));
            }

            return Enumerable.Range(0, 4).SelectMany(r => new List<(bool flip, int rotation)> { (false, r), (true, r) })
                .Where(transform => fixedEdges.All(e => tile.GetEdge(e.edgeKey, transform.flip, transform.rotation) == string.Join("", e.edge.Reverse()))).ToList();
        }
    }

    public class Tile
    {
        public long id;
        private Dictionary<int, string> edges;
        private Dictionary<int, string> flippedEdges;
        private Dictionary<(int x, int y), char> content;
        private int contentSize;

        public Tile(long id, List<string> lines)
        {
            this.id = id;
            var edge1 = lines[0];
            var edge3 = string.Join("", lines[^1].Reverse());
            var edge2 = string.Join("", lines.Select(l => l[^1]));
            var edge4 = string.Join("", lines.Select(l => l[0]).Reverse());
            // edges go clockwise 1, 2, 3, 4
            edges = new Dictionary<int, string> {{1, edge1}, {2, edge2}, {3, edge3}, {4, edge4}};
            flippedEdges = new Dictionary<int, string>();
            flippedEdges.Add(1, string.Join("", edge1.Reverse()));
            flippedEdges.Add(2, string.Join("", edge4.Reverse()));
            flippedEdges.Add(3, string.Join("", edge3.Reverse()));
            flippedEdges.Add(4, string.Join("", edge2.Reverse()));

            content = new Dictionary<(int x, int y), char>();
            var y = 0;
            while (y < lines.Count - 2)
            {
                var line = lines[y + 1];
                var x = 0;
                while (x < line.Length - 2)
                {
                    content.Add((x, y), line[x + 1]);
                    x++;
                }

                y++;
            }

            contentSize = y;
        }

        public string GetEdge(int edgeKey, bool flip, int rotation)
        {
            // Assume edgeKey is at least 1, so don't have to think about negative/0
             var key = edgeKey + rotation;
             while (key > 4)
             {
                 key = key - 4;
             }

             return flip ? flippedEdges[key] : edges[key];
        }

        public char GetContentAt((int x, int y) location, bool flip, int rotation)
        {
            var transformedX = location.x;
            var transformedY = location.y;

            switch (rotation)
            {
                case 0:
                    break;
                case 3:
                    var temp = transformedX;
                    transformedX = transformedY;
                    transformedY = contentSize - 1 - temp;
                    break;
                case 2:
                    transformedY = contentSize - 1 - transformedY;
                    transformedX = contentSize - 1 - transformedX;
                    break;
                case 1:
                    var temp2 = transformedY;
                    transformedY = transformedX;
                    transformedX = contentSize - 1 - temp2;
                    break;
                default:
                    throw new Exception("Unexpected rotation");
            }

            if (flip)
            {
                transformedX = contentSize - 1 - transformedX;
            }

            return content[(transformedX, transformedY)];
        }

        public static Tile Parse(List<string> lines)
        {
            var id = long.Parse(lines[0].Substring(5).TrimEnd(':'));
            return new Tile(id, lines.Skip(1).ToList());
        }
    }
}
