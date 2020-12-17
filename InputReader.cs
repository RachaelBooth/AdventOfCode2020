using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020
{
    public class InputReader : InputReader<string>
    {
        public InputReader(int day, bool useTestFilePath = false) : base(day, useTestFilePath) {}

        public Dictionary<T, U> ParseGrid<T, U>(Func<(int x, int y), T> locationMap, Func<char, U> characterMap)
        {
            var grid = new Dictionary<T, U>();
            var y = 0;
            foreach (var line in ReadInputAsLines())
            {
                var x = 0;
                while (x < line.Length)
                {
                    grid.Add(locationMap((x, y)), characterMap(line[x]));
                    x++;
                }

                y++;
            }

            return grid;
        }
    }

    public class InputReader<T>
    {
        private readonly string inputFilePath;

        public InputReader(int day, bool useTestFilePath = false)
        {
            inputFilePath = $"../../../day{day}/{(useTestFilePath ? "Test" : "")}Input.txt";
        }

        public IEnumerable<T> ReadInputAsLines()
        {
            var reader = new StreamReader(inputFilePath);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                yield return ParseLine(line);
            }

            reader.Close();
        }

        public IEnumerable<T> ReadInputAsLineGroups()
        {
            var reader = new StreamReader(inputFilePath);
            string line;

            var currentLines = new List<string>();
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "")
                {
                    yield return ParseLineGroup(currentLines);
                    currentLines = new List<string>();
                }
                else
                {
                    currentLines.Add(line);
                }
            }
            // Because bleugh. (Will get null for end of file rather than final blank line)
            yield return ParseLineGroup(currentLines);

            reader.Close();
        }

        private T ParseLine(string line)
        {
            if (typeof(T) == typeof(string))
            {
                // Ewww
                return (T) Convert.ChangeType(line, typeof(T));
            }
            var parse = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
            return (T) parse.Invoke(this, new[] { line });
        }

        private T ParseLineGroup(List<string> lines)
        {
            var parse = typeof(T).GetMethod("Parse", new Type[] { typeof(List<string>) });
            return (T) parse.Invoke(this, new[] { lines });
        }
    }
}
