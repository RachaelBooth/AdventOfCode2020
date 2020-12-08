using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Day4
{
    public class Passport
    {
        private Dictionary<string, string> data;

        public Passport(IEnumerable<string> data)
        {
            this.data = new Dictionary<string, string>();
            foreach (var item in data.SelectMany(d => d.Split(' ')))
            {
                var parts = item.Split((':'));
                this.data.Add(parts[0], parts[1]);
            }
        }

        public Passport(Dictionary<string, string> data)
        {
            this.data = data;
        }

        public static Passport Parse(List<string> dataLines)
        {
            var data = new Dictionary<string, string>();
            foreach (var item in dataLines.SelectMany(d => d.Split(' ')))
            {
                var parts = item.Split((':'));
                data.Add(parts[0], parts[1]);
            }
            return new Passport(data);
        }

        public bool IsValid()
        {
            if (!FieldsArePresent())
            {
                return false;
            }

            var birthYear = int.Parse(data["byr"]);
            if (birthYear < 1920 || birthYear > 2002)
            {
                return false;
            }

            var issueYear = int.Parse(data["iyr"]);
            if (issueYear < 2010 || issueYear > 2020)
            {
                return false;
            }

            var expirationYear = int.Parse(data["eyr"]);
            if (expirationYear < 2020 || expirationYear > 2030)
            {
                return false;
            }

            var height = data["hgt"];
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

            var hair = data["hcl"];
            if (!Regex.IsMatch(hair, "^#[0-9a-f]{6}$"))
            {
                return false;
            }

            var allowedEyes = new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};
            if (!allowedEyes.Contains(data["ecl"]))
            {
                return false;
            }

            if (!Regex.IsMatch(data["pid"], "^[0-9]{9}$"))
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
            return keys.All(key => data.ContainsKey(key));
        }
    }
}
