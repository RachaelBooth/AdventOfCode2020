using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2020.Computer;

namespace AdventOfCode2020.Day8
{
    public class Solver : ISolver
    {
        private readonly List<Instruction> instructions;

        public Solver()
        {
           instructions = new InputReader<Instruction>(8).ReadInputAsLines().ToList();
        }

        public void SolvePartOne()
        {
            var accumulator = 0;
            var index = 0;
            var indicesRun = new List<int>();

            while (!indicesRun.Contains(index))
            {
                indicesRun.Add(index);
                var instruction = instructions[index];
                switch (instruction.operation)
                {
                    case Operation.acc:
                        accumulator = accumulator + instruction.argument;
                        index = index + 1;
                        break;
                    case Operation.jmp:
                        index = index + instruction.argument;
                        break;
                    case Operation.nop:
                        index = index + 1;
                        break;
                    default:
                        throw new Exception("Unrecognised operation");
                }
            }

            Console.WriteLine(accumulator);
        }

        public void SolvePartTwo()
        {
            var swapsAttempted = new List<int>();

            while (true)
            {
                var swapped = false;
                var index = 0;
                var indicesRun = new List<int>();
                var accumulator = 0;

                while (!indicesRun.Contains(index))
                {
                    if (index == instructions.Count)
                    {
                        // Terminates
                        Console.WriteLine($"swapped instruction at index {swapsAttempted.Last()}");
                        Console.WriteLine($"Accumulator: {accumulator}");
                        return;
                    }
                    
                    indicesRun.Add(index);
                    var instruction = instructions[index];

                    if (!swapped && instruction.operation != Operation.acc && !swapsAttempted.Contains(index))
                    {
                        swapped = true;
                        swapsAttempted.Add(index);
                        switch (instruction.operation)
                        {
                            case Operation.jmp:
                                instruction = new Instruction(Operation.nop, instruction.argument);
                                break;
                            case Operation.nop:
                                instruction = new Instruction(Operation.jmp, instruction.argument);
                                break;
                        }
                    }

                    switch (instruction.operation)
                    {
                        case Operation.acc:
                            accumulator = accumulator + instruction.argument;
                            index = index + 1;
                            break;
                        case Operation.jmp:
                            index = index + instruction.argument;
                            break;
                        case Operation.nop:
                            index = index + 1;
                            break;
                        default:
                            throw new Exception("Unrecognised operation");
                    }
                }
                // In infinite loop =( - try again
            }
        }
    }
}
