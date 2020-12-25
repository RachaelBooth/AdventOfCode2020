using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using AdventOfCode.Base;

namespace AdventOfCode2020.Day11
{
    public class Solver : ISolver
    {
        private Dictionary<(int x, int y), bool> SeatOccuptionMap = new Dictionary<(int x, int y), bool>();
        private Dictionary<(int x, int y), List<(int x, int y)>> VisibleSeats;

        public Solver()
        {
            var lines = new InputReader<string>(11).ReadInputAsLines();
            var y = 0;
            foreach (var line in lines)
            {
                var x = 0;
                while (x < line.Length)
                {
                    if (line[x] == 'L')
                    {
                        SeatOccuptionMap.Add((x, y), false);
                    }
                    x++;
                }

                y++;
            }
        }

        public void SolvePartOne()
        {
            var currentMap = SeatOccuptionMap;
            while (true)
            {
                var nextMap = RunStepVersionOne(currentMap);
                if (nextMap.Keys.All(k => currentMap[k] == nextMap[k]))
                {
                    // Done (keys should remain constant)
                    var result = nextMap.Count(p => p.Value);
                    Console.WriteLine(result);
                    return;
                }

                currentMap = nextMap;
            }
        }

        public void SolvePartTwo()
        {
            PopulateVisibleSeatsDictionary();
            var currentMap = SeatOccuptionMap;
            while (true)
            {
                var nextMap = RunStepVersionTwo(currentMap);
                if (nextMap.Keys.All(k => currentMap[k] == nextMap[k]))
                {
                    // Done (keys should remain constant)
                    var result = nextMap.Count(p => p.Value);
                    Console.WriteLine(result);
                    return;
                }

                currentMap = nextMap;
            }
        }

        private void PopulateVisibleSeatsDictionary()
        {
            VisibleSeats = new Dictionary<(int x, int y), List<(int x, int y)>>();
            var seats = SeatOccuptionMap.Keys;
            var maxX = seats.Max(s => s.x);
            var maxY = seats.Max(s => s.y);
            foreach (var seat in seats)
            {
                var visible = new List<(int x, int y)>();

                var left = 1;
                while (seat.x - left >= 0)
                {
                    if (seats.Contains((seat.x - left, seat.y)))
                    {
                        visible.Add((seat.x - left, seat.y));
                        break;
                    }

                    left++;
                }

                var right = 1;
                while (seat.x + right <= maxX)
                {
                    if (seats.Contains((seat.x + right, seat.y)))
                    {
                        visible.Add((seat.x + right, seat.y));
                        break;
                    }

                    right++;
                }

                var up = 1;
                // yes that is up because of the way I did directions on reading this grid. Boo...
                while (seat.y - up >= 0)
                {
                    if (seats.Contains((seat.x, seat.y - up)))
                    {
                        visible.Add((seat.x, seat.y - up));
                        break;
                    }

                    up++;
                }

                var down = 1;
                while (seat.y + down <= maxY)
                {
                    if (seats.Contains((seat.x, seat.y + down)))
                    {
                        visible.Add((seat.x, seat.y + down));
                        break;
                    }

                    down++;
                }

                var ne = 1;
                while (seat.x + ne <= maxX && seat.y - ne >= 0)
                {
                    if (seats.Contains((seat.x + ne, seat.y - ne)))
                    {
                        visible.Add((seat.x + ne, seat.y - ne));
                        break;
                    }

                    ne++;
                }

                var nw = 1;
                while (seat.x - nw >= 0 && seat.y - nw >= 0)
                {
                    if (seats.Contains((seat.x - nw, seat.y - nw)))
                    {
                        visible.Add((seat.x - nw, seat.y - nw));
                        break;
                    }

                    nw++;
                }

                var se = 1;
                while (seat.x + se <= maxX && seat.y + se <= maxY)
                {
                    if (seats.Contains((seat.x + se, seat.y + se)))
                    {
                        visible.Add((seat.x + se, seat.y + se));
                        break;
                    }

                    se++;
                }

                var sw = 1;
                while (seat.x - sw >= 0 && seat.y + sw <= maxY)
                {
                    if (seats.Contains((seat.x - sw, seat.y + sw)))
                    {
                        visible.Add((seat.x - sw, seat.y + sw));
                        break;
                    }

                    sw++;
                }

                VisibleSeats.Add(seat, visible);
            }
        }

        private Dictionary<(int x, int y), bool> RunStepVersionOne(Dictionary<(int x, int y), bool> occupationMap)
        {
            var updatedMap = new Dictionary<(int x, int y), bool>();
            foreach (var location in occupationMap.Keys)
            {
                var adjacents = occupationMap.Keys.Where(k =>
                    k != location && Math.Abs(k.x - location.x) <= 1 && Math.Abs(k.y - location.y) <= 1);
                var occupiedAdjacents = adjacents.Count(a => occupationMap[a]);
                if (occupationMap[location])
                {
                    if (occupiedAdjacents >= 4)
                    {
                        updatedMap.Add(location, false);
                    }
                    else
                    {
                        updatedMap.Add(location, true);
                    }
                }
                else
                {
                    if (occupiedAdjacents == 0)
                    {
                        updatedMap.Add(location, true);
                    }
                    else
                    {
                        updatedMap.Add(location, false);
                    }
                }
            }

            return updatedMap;
        }

        private Dictionary<(int x, int y), bool> RunStepVersionTwo(Dictionary<(int x, int y), bool> occupationMap)
        {
            var updatedMap = new Dictionary<(int x, int y), bool>();
            foreach (var seat in occupationMap.Keys)
            {
                var visibleOccupiedCount = VisibleSeats[seat].Count(s => occupationMap[s]);
                if (occupationMap[seat])
                {
                    if (visibleOccupiedCount >= 5)
                    {
                        updatedMap.Add(seat, false);
                    }
                    else
                    {
                        updatedMap.Add(seat, true);
                    }
                }
                else
                {
                    if (visibleOccupiedCount == 0)
                    {
                        updatedMap.Add(seat, true);
                    }
                    else
                    {
                        updatedMap.Add(seat, false);
                    }
                }
            }

            return updatedMap;
        }
    }
}
