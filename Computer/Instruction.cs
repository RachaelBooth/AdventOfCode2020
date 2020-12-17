using System;

namespace AdventOfCode2020.Computer
{
    public class Instruction
    {
        public Operation operation;
        public int argument;

        public Instruction(Operation operation, int argument)
        {
            this.operation = operation;
            this.argument = argument;
        }

        public static Instruction Parse(string line)
        {
            var parts = line.Split(" ");
            var operation = Enum.Parse<Operation>(parts[0]);
            var argument = int.Parse(parts[1]);
            return new Instruction(operation, argument);
        }
    }

    public enum Operation
    {
        acc,
        jmp,
        nop
    }
}
