using System;

namespace AdventOfCode2020.Day25
{
    public class Solver : ISolver
    {
        // w.l.o.g.
        private int cardPublicKey = 13233401;
        private int doorPublicKey = 6552760;

        private int modulus = 20201227;

        public void SolvePartOne()
        {
            // Should end up in int range but intermediate multiplications could be big
            long value = 1;
            var subjectNumber = 7;
            var cardLoopSize = 0;
            while (value != cardPublicKey)
            {
                value = (value * subjectNumber) % modulus;
                cardLoopSize++;
            }

            var c = 0;
            long encryptionKey = 1;
            while (c < cardLoopSize)
            {
                encryptionKey = (encryptionKey * doorPublicKey) % modulus;
                c++;
            }

            Console.WriteLine(encryptionKey);
        }
        
        public void SolvePartTwo()
        {
            throw new NotImplementedException("Merry Christmas!");
        }
}
}
