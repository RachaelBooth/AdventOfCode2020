using System;

namespace AdventOfCode2020.Computer
{
    public class InfiniteLoopException : Exception
    {
        public InfiniteLoopException(object state) : base($"Infinite loop detected: {state} previously visited") {}
    }
}
