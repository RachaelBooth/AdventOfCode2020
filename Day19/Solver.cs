using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Day19
{
    public class Solver : ISolver
    {
        private Dictionary<string, Rule> Rules = new Dictionary<string, Rule>();
        private List<string> Messages = new List<string>();

        public Solver()
        {
            var lines = new InputReader(19).ReadInputAsLines();

            var inRulesSection = true;
            foreach (var line in lines)
            {
                if (inRulesSection && line == "")
                {
                    inRulesSection = false;
                }
                else if (inRulesSection)
                {
                    var rule = new Rule(line);
                    Rules.Add(rule.key, rule);
                }
                else
                {
                    Messages.Add(line);
                }
            }
        }

        public void SolvePartOne()
        {
            var ruleZeroRegex = "^" + ReduceToRegex("0") + "$";
            var result = Messages.Count(message => Regex.IsMatch(message, ruleZeroRegex));
            Console.WriteLine(result);
        }

        public void SolvePartTwo()
        {
            Rules["8"] = new Rule("8: 42 | 42 8");
            Rules["11"] = new Rule("11: 42 31 | 42 11 31");
            var ruleZeroRegex = "^" + ReduceToRegex("0") + "$";
            var result = Messages.Count(message => Regex.IsMatch(message, ruleZeroRegex));
            Console.WriteLine(result);
        }

        private string ReduceToRegex(List<string> ruleKeys)
        {
            return string.Join("", ruleKeys.Select(ReduceToRegex));
        }

        private string ReduceToRegex(string ruleKey)
        {
            var rule = Rules[ruleKey];
            if (rule.isCharacterRule)
            {
                return rule.character;
            }

            // Eww
            if (ruleKey == "8" && rule.subRules.Count > 1)
            {
                // i.e. 8 in part 2
                return "(" + ReduceToRegex("42") + ")+";
            }

            if (ruleKey == "11" && rule.subRules.Count > 1)
            {
                return "(?'open'(" + ReduceToRegex("42") + "))+(?'-open'(" + ReduceToRegex("31") + "))+(?(open)(?!))";
            }

            return "(" + string.Join("|", rule.subRules.Select(ReduceToRegex)) + ")";
        }
    }

    public class Rule
    {
        public string key;
        public bool isCharacterRule;
        public string character;
        public List<List<string>> subRules;

        public Rule(string line)
        {
            var parts = line.Split(": ");
            key = parts[0];
            // What can be here is sufficiently restricted to do this, even if I ache to be more generic
            if (parts[1] == "\"a\"")
            {
                isCharacterRule = true;
                character = "a";
            }
            else if (parts[1] == "\"b\"")
            {
                isCharacterRule = true;
                character = "b";
            }
            else
            {
                isCharacterRule = false;
                subRules = parts[1].Split(" | ").Select(r => r.Split(" ").ToList()).ToList();
            }
        }
    }
}
