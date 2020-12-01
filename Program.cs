using EasyConsoleCore;
using System.Reflection;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = Input.ReadInt("Select a day: ", min: 1, max: 25);
            var part = Input.ReadInt("Which part?", min: 1, max: 2);
            var solver = Assembly.GetExecutingAssembly().CreateInstance($"AdventOfCode2020.Day{day}.Solver") as ISolver;
            switch (part) {
                case 1:
                    solver.SolvePartOne();
                    break;
                case 2:
                    solver.SolvePartTwo();
                    break;
            }
        }
    }
}
