using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Base;

namespace AdventOfCode2020.Computer
{
    public class Computer
    {
        private int accumulator;
        private bool hasHalted = false;

        private int index;

        // qq make this a value tuple if needing more data later
        private List<int> previousStates = new List<int> { 0 };
        private readonly List<Instruction> programme;

        public Computer(List<Instruction> programme)
        {
            this.programme = programme;
        }

        public Computer(int day)
        {
            programme = new InputReader<Instruction>(day).ReadInputAsLines().ToList();
        }

        public int RunProgramme(int? steps)
        {
            var s = 0;
            while (!hasHalted && (!steps.HasValue || s < steps.Value))
            {
                RunStep();
                s += 1;
            }

            return accumulator;
        }

        private void RunStep()
        {
            if (index == programme.Count)
            {
                hasHalted = true;
                return;
            }

            if (index > programme.Count)
            {
                hasHalted = true;
                Console.WriteLine("Potential error: Attempted to run step after end, but not immediately after the last instruction. Halting anyway.");
                return;
            }

            var instruction = programme[index];
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
                    throw new NotImplementedException("Unrecognised operation");
            }
            EnsureNotInInfiniteLoop();
        }

        private void EnsureNotInInfiniteLoop()
        {
            var currentState = index;
            if (previousStates.Contains(currentState))
            {
                throw new InfiniteLoopException(currentState);
            }
            previousStates.Add(currentState);
        }
    }
}