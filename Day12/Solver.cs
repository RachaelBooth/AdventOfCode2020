using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode.Base;

namespace AdventOfCode2020.Day12
{
    public class Solver : ISolver
    {
        private IEnumerable<Direction> directions;

        public Solver()
        {
            directions = new InputReader<Direction>(12).ReadInputAsLines();
        }

        public void SolvePartOne()
        {
            var north = 0;
            var east = 0;
            // We'll define angle 0 to be east, and positive numbers clockwise
            var facing = 0;
            foreach (var direction in directions)
            {
                switch (direction.action)
                {
                    case 'N':
                        north = north + direction.value;
                        break;
                    case 'S':
                        north = north - direction.value;
                        break;
                    case 'E':
                        east = east + direction.value;
                        break;
                    case 'W':
                        east = east - direction.value;
                        break;
                    case 'L':
                        // Eww, degrees
                        facing = facing - direction.value;
                        break;
                    case 'R':
                        facing = facing + direction.value;
                        break;
                    case 'F':
                        while (facing < 0)
                        {
                            facing = facing + 360;
                        }

                        while (facing >= 360)
                        {
                            facing = facing - 360;
                        }

                        switch (facing)
                        {
                            case 0:
                                east = east + direction.value;
                                break;
                            case 90:
                                north = north - direction.value;
                                break;
                            case 180:
                                east = east - direction.value;
                                break;
                            case 270:
                                north = north + direction.value;
                                break;
                            default:
                                throw new Exception("Sorry, gotta handle more complicated facing directions");
                        }

                        break;
                    default:
                        throw new Exception("Unrecognised action");
                }
            }

            var result = Math.Abs(north) + Math.Abs(east);
            Console.WriteLine(result);
        }

        public void SolvePartTwo()
        {
            var waypointNorth = 1;
            var waypointEast = 10;
            var shipNorth = 0;
            var shipEast = 0;
            foreach (var direction in directions)
            {
                switch (direction.action)
                {
                    case 'N':
                        waypointNorth = waypointNorth + direction.value;
                        break;
                    case 'S':
                        waypointNorth = waypointNorth - direction.value;
                        break;
                    case 'E':
                        waypointEast = waypointEast + direction.value;
                        break;
                    case 'W':
                        waypointEast = waypointEast - direction.value;
                        break;
                    case 'L':
                        // Eww, degrees
                        var degreesLeft = direction.value % 360;
                        switch (degreesLeft)
                        {
                            case 0:
                                break;
                            case 90:
                                var newNorth = waypointEast;
                                var newEast = -waypointNorth;
                                waypointNorth = newNorth;
                                waypointEast = newEast;
                                break;
                            case 180:
                                waypointNorth = -waypointNorth;
                                waypointEast = -waypointEast;
                                break;
                            case 270:
                                var newNorth2 = -waypointEast;
                                var newEast2 = waypointNorth;
                                waypointNorth = newNorth2;
                                waypointEast = newEast2;
                                break;
                            default:
                                throw new Exception("Sorry, more complicated rotations...");
                        }
                        break;
                    case 'R':
                        // Bleugh should sort out reusing. Ah well, just swap 90 and 270.
                        var degreesRight = direction.value % 360;
                        switch (degreesRight)
                        {
                            case 0:
                                break;
                            case 270:
                                var newNorth = waypointEast;
                                var newEast = -waypointNorth;
                                waypointNorth = newNorth;
                                waypointEast = newEast;
                                break;
                            case 180:
                                waypointNorth = -waypointNorth;
                                waypointEast = -waypointEast;
                                break;
                            case 90:
                                var newNorth2 = -waypointEast;
                                var newEast2 = waypointNorth;
                                waypointNorth = newNorth2;
                                waypointEast = newEast2;
                                break;
                            default:
                                throw new Exception("Sorry, more complicated rotations...");
                        }
                        break;
                    case 'F':
                        shipNorth = shipNorth + direction.value * waypointNorth;
                        shipEast = shipEast + direction.value * waypointEast;
                        break;
                    default:
                        throw new Exception("Unrecognised action");
                }
            }

            var result = Math.Abs(shipNorth) + Math.Abs(shipEast);
            Console.WriteLine(result);
        }
    }

    public class Direction
    {
        public int value;
        public char action;

        public Direction(char action, int value)
        {
            this.action = action;
            this.value = value;
        }

        public static Direction Parse(string line)
        {
            var action = line[0];
            var value = int.Parse(line.Remove(0, 1));
            return new Direction(action, value);
        }
    }
}
