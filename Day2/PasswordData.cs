using System.Linq;

namespace AdventOfCode2020.Day2
{
    public class PasswordData
    {
        private readonly string password;
        private readonly char letter;
        private readonly (int min, int max) range;

        public PasswordData(string password, char letter, (int min, int max) range)
        {
            this.password = password;
            this.letter = letter;
            this.range = range;
        }

        public bool IsValidOldSystem()
        {
            var letterCount = password.ToCharArray().Count(c => c == letter);
            return letterCount >= range.min && letterCount <= range.max;
        }

        public bool IsValidNewSystem()
        {
            var relevantChars = new [] { password[range.min - 1], password[range.max - 1] };
            return relevantChars.Count(c => c == letter) == 1;
        }

        public static PasswordData Parse(string line)
        {
            var parts = line.Split(' ');
            var password = parts[2];
            var letter = parts[1][0];
            var range = parts[0].Split('-');
            var min = int.Parse(range[0]);
            var max = int.Parse(range[1]);
            return new PasswordData(password, letter, (min, max));
        }
    }
}
