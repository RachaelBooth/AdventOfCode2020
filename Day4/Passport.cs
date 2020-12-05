using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Day4
{
    public class Passport
    {
        public Dictionary<string, string> Data;

        public Passport(IEnumerable<string> data)
        {
            Data = new Dictionary<string, string>();
            foreach (var item in data.SelectMany(d => d.Split(' ')))
            {
                var parts = item.Split((':'));
                Data.Add(parts[0], parts[1]);
            }
        }

        public bool IsValid()
        {
            if (!FieldsArePresent())
            {
                return false;
            }

            var birthYear = int.Parse(Data["byr"]);
            if (birthYear < 1920 || birthYear > 2002)
            {
                return false;
            }

            var issueYear = int.Parse(Data["iyr"]);
            if (issueYear < 2010 || issueYear > 2020)
            {
                return false;
            }

            var expirationYear = int.Parse(Data["eyr"]);
            if (expirationYear < 2020 || expirationYear > 2030)
            {
                return false;
            }

            var height = Data["hgt"];
            if (height.EndsWith("cm"))
            {
                var cmHeight = int.Parse(height.TrimEnd('c', 'm'));
                if (cmHeight < 150 || cmHeight > 193)
                {
                    return false;
                }
            }
            else if (height.EndsWith("in"))
            {
                var inHeight = int.Parse(height.TrimEnd('i', 'n'));
                if (inHeight < 59 || inHeight > 76)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            var hair = Data["hcl"];
            if (!Regex.IsMatch(hair, "^#[0-9a-f]{6}$"))
            {
                return false;
            }

            var allowedEyes = new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};
            if (!allowedEyes.Contains(Data["ecl"]))
            {
                return false;
            }

            if (!Regex.IsMatch(Data["pid"], "^[0-9]{9}$"))
            {
                return false;
            }

            return true;
        }

        public bool FieldsArePresent()
        {
            return DataContainsKeys("byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid");
        }

        private bool DataContainsKeys(params string[] keys)
        {
            return keys.All(key => Data.ContainsKey(key));
        }
    }
}
