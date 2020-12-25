using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using AdventOfCode.Base;

namespace AdventOfCode2020.Day18
{
    public class Solver : ISolver
    {
        public void SolvePartOne()
        {
            var results = new InputReader<SamePrecedenceExpression>(18).ReadInputAsLines();
            var sum = results.Select(r => (BigInteger) r.expressionValue).Aggregate((current, next) => current + next);
            Console.WriteLine(sum);
        }

        public void SolvePartTwo()
        {
            var results = new InputReader<AdditionPrecedenceExpression>(18).ReadInputAsLines();
            var sum = results.Select(r => (BigInteger)r.expressionValue).Aggregate((current, next) => current + next);
            Console.WriteLine(sum);
        }
    }

    public class SamePrecedenceExpression
    {
        public long expressionValue;

        public SamePrecedenceExpression(long expressionValue)
        {
            this.expressionValue = expressionValue;
        }

        // Not quite the naming I'd go for in this case but fits my input reader
        // It's not like I'll need to maintain this for years
        public static SamePrecedenceExpression Parse(string expression)
        {
            if (!expression.Contains(")"))
            {
                var parts = expression.Trim().Split(" ");
                var value = long.Parse(parts[0]);
                var i = 1;
                while (i < parts.Length)
                {
                    // Will also get an exception if parts.Length is the wrong parity - again this means something is wrong in input parsing
                    var op = parts[i];
                    if (op == "+")
                    {
                        value = value + long.Parse(parts[i + 1]);
                    }
                    else if (op == "*")
                    {
                        value = value * long.Parse(parts[i + 1]);
                    }
                    else
                    {
                        throw new Exception("Unexpected operator - something's going wrong...");
                    }

                    i = i + 2;
                }
                return new SamePrecedenceExpression(value);
            }

            var rightIndex = 0;
            while (expression[rightIndex] != ')')
            {
                rightIndex++;
            }

            var leftIndex = rightIndex;
            while (expression[leftIndex] != '(')
            {
                leftIndex--;
            }

            var bracketedValue = Parse(expression.Substring(leftIndex + 1, rightIndex - leftIndex - 1)).expressionValue;
            var earlierPart = expression.Substring(0, leftIndex).TrimEnd();
            var laterPart = expression.Substring(rightIndex + 1).TrimStart();
            var expressionWithSubstitution = $"{earlierPart} {bracketedValue} {laterPart}";
            return Parse(expressionWithSubstitution.Trim());
        }
    }

    public class AdditionPrecedenceExpression
    {
        public long expressionValue;

        public AdditionPrecedenceExpression(long expressionValue)
        {
            this.expressionValue = expressionValue;
        }

        // This is icky as hell
        public static AdditionPrecedenceExpression Parse(string expression)
        {
            // Brackets still have precedence
            if (!expression.Contains(")"))
            {
                var parts = expression.Trim().Split(" ");
                if (!expression.Contains("+"))
                {
                    var value = long.Parse(parts[0]);
                    var i = 1;
                    while (i < parts.Length)
                    {
                        // Will also get an exception if parts.Length is the wrong parity - again this means something is wrong in input parsing
                        var op = parts[i];
                        if (op == "*")
                        {
                            value = value * long.Parse(parts[i + 1]);
                        }
                        else
                        {
                            throw new Exception("Unexpected operator - something's going wrong...");
                        }

                        i = i + 2;
                    }
                    return new AdditionPrecedenceExpression(value);
                }

                var additionOperatorIndex = 1;
                while (parts[additionOperatorIndex] != "+")
                {
                    additionOperatorIndex = additionOperatorIndex + 2;
                }

                var sum = long.Parse(parts[additionOperatorIndex - 1]) + long.Parse(parts[additionOperatorIndex + 1]);
                var substitutedParts = parts.Take(additionOperatorIndex - 1).Append(sum.ToString()).ToList();
                substitutedParts.AddRange(parts.Skip(additionOperatorIndex + 2));
                var substitutedExpression = string.Join(" ", substitutedParts);
                return Parse(substitutedExpression);
            }

            var rightIndex = 0;
            while (expression[rightIndex] != ')')
            {
                rightIndex++;
            }

            var leftIndex = rightIndex;
            while (expression[leftIndex] != '(')
            {
                leftIndex--;
            }

            var bracketedValue = Parse(expression.Substring(leftIndex + 1, rightIndex - leftIndex - 1)).expressionValue;
            var earlierPart = expression.Substring(0, leftIndex).TrimEnd();
            var laterPart = expression.Substring(rightIndex + 1).TrimStart();
            var expressionWithSubstitution = $"{earlierPart} {bracketedValue} {laterPart}";
            return Parse(expressionWithSubstitution.Trim());
        }
    }
}
