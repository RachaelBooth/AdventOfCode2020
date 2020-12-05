using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day3
{
    public class Map
    {
        public Dictionary<(int x, int y), bool> map;
        public int maxX;
        public int maxY;

        public Map(IEnumerable<MapLine> mapLines)
        {
            map = new Dictionary<(int x, int y), bool>();
            var y = 0;
            foreach (var mapLine in mapLines)
            {
                var x = 0;
                foreach (var point in mapLine.lineTrees)
                {
                    map.Add((x, y), point);
                    x++;
                }

                maxX = x - 1;
                y++;
            }

            maxY = y - 1;
        }
    }

    public class MapLine
    {
        public List<bool> lineTrees;

        public MapLine(List<bool> lineTrees)
        {
            this.lineTrees = lineTrees;
        }

        public static MapLine Parse(string line)
        {
            return new MapLine(line.ToCharArray().Select(c => c == '#').ToList());
        }
    }
}
