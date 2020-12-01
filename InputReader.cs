using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020
{
    public class InputReader<T>
    {
        private readonly string inputFilePath;

        public InputReader(int day)
        {
            inputFilePath = $"../../../day{day}/input.txt";
        }

        public IEnumerable<T> ReadInput()
        {
            return ReadInputFromFile(inputFilePath);
        }

        private IEnumerable<T> ReadInputFromFile(string filePath)
        {
            var reader = new StreamReader(filePath);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                yield return ParseLine(line);
            }

            reader.Close();
        }

        private T ParseLine(string line)
        {
            var parse = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
            return (T) parse.Invoke(this, new[] { line });
        }
    }
}
