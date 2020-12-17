using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace AdventOfCode2020.Day14
{
    public class Solver : ISolver
    {
        private IEnumerable<InitializationProgrammeLine> initializationProgramme;

        public Solver()
        {
            initializationProgramme = new InputReader<InitializationProgrammeLine>(14).ReadInputAsLines();
        }

        public void SolvePartOne()
        {
            var mem = new Dictionary<string, string>();
            var maskZeros = new List<int>();
            var maskOnes = new List<int>();
            foreach (var line in initializationProgramme)
            {
                if (line.IsMaskLine)
                {
                    maskZeros = new List<int>();
                    maskOnes = new List<int>();
                    var i = 0;
                    while (i < line.bitmask.Length)
                    {
                        if (line.bitmask[i] == '0')
                        {
                            maskZeros.Add(i);
                        }
                        else if (line.bitmask[i] == '1')
                        {
                            maskOnes.Add(i);
                        }
                        i++;
                    }
                }
                else
                {
                    var binaryValue = Convert.ToString(line.value, 2).PadLeft(36, '0').ToCharArray();
                    foreach (var val in maskZeros)
                    {
                        binaryValue[val] = '0';
                    }
                    foreach (var val in maskOnes)
                    {
                        binaryValue[val] = '1';
                    }
                    mem[line.memToWrite] = string.Join("", binaryValue);
                }
            }

            UInt64 sum = 0;
            foreach (var value in mem.Values)
            {
                sum = sum + Convert.ToUInt64(value, 2);
            }
            Console.WriteLine(sum);
        }

        public void SolvePartTwo()
        {
            var mem = new Dictionary<string, UInt64>();
            // Will always be overwritten before use
            var mask = "";
            foreach (var line in initializationProgramme)
            {
                if (line.IsMaskLine)
                {
                    mask = line.bitmask;
                }
                else
                {
                    var memBinary = Convert.ToString(int.Parse(line.memToWrite), 2);
                    var addresses = GetMaskResults(memBinary, mask);
                    foreach (var address in addresses)
                    {
                        mem[address] = Convert.ToUInt64(line.value);
                    }
                }
            }
            var result = mem.Values.Aggregate((current, next) => current + next);
            Console.WriteLine(result);
        }

        // Valid for version 2
        private List<string> GetMaskResults(string input, string mask)
        {
            var i = 1;
            // Mask is always full length, can assume with this input that input is shorter
            var results = new List<string> { "" };
            while (i <= mask.Length)
            {
                if (mask[^i] == 'X')
                {
                    results = results.SelectMany(r => new List<string> { '0' + r, '1' + r }).ToList();
                }
                else
                {
                    char inputValue;
                    if (i <= input.Length)
                    {
                        inputValue = input[^i];
                    }
                    else
                    {
                        inputValue = '0';
                    }
                    var maskedInputValue = (char) (inputValue | mask[^i]);
                    results = results.Select(r => maskedInputValue + r).ToList();
                }
                i++;
            }
            return results;
        }
    }

    public class InitializationProgrammeLine
    {
        public string bitmask;
        public string memToWrite;
        public int value;
        public bool IsMaskLine;

        public InitializationProgrammeLine(string bitmask)
        {
            this.bitmask = bitmask;
            IsMaskLine = true;
        }

        public InitializationProgrammeLine(string memToWrite, string value)
        {
            this.memToWrite = memToWrite;
            this.value = int.Parse(value);
            IsMaskLine = false;
        }

        public static InitializationProgrammeLine Parse(string line)
        {
            if (line.StartsWith("mask"))
            {
                return new InitializationProgrammeLine(line.Split(" = ")[1]);
            }

            var parts = line.Split(" = ");
            var address = parts[0].Split('[', ']')[1];
            return new InitializationProgrammeLine(address, parts[1]);
        }
    }
}
