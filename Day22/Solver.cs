using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Base;

namespace AdventOfCode2020.Day22
{
    public class Solver : ISolver
    {
        private Dictionary<string, List<int>> initialDeal;
        private const string P1 = "Player 1:";
        private const string P2 = "Player 2:";

        public Solver()
        {
            initialDeal = new InputReader<int>(22).ReadInputAsGroupedLinesByHeader();
        }

        public void SolvePartOne()
        {
            var game = new CombatGame(initialDeal[P1], initialDeal[P2]);
            game.PlayUntilFinished();
            var winningScore = game.GetScore();
            Console.WriteLine(winningScore);
        }

        public void SolvePartTwo()
        {
            var game = new RecursiveCombatGame(initialDeal[P1], initialDeal[P2]);
            game.PlayUntilFinished();
            var winningScore = game.GetWinnersScore();
            Console.WriteLine(winningScore);
        }
    }

    public class CombatGame
    {
        private List<int> P1Cards;
        private List<int> P2Cards;

        public CombatGame(List<int> p1Cards, List<int> p2Cards)
        {
            P1Cards = p1Cards;
            P2Cards = p2Cards;
        }

        public void PlayUntilFinished()
        {
            while (!GameIsOver())
            {
                PlayRound();
            }
        }

        public int GetScore()
        {
            var deckToCount = P1Cards.Any() ? P1Cards : P2Cards;
            var score = 0;
            var i = 1;
            while (i <= deckToCount.Count)
            {
                score = score + i * deckToCount[^i];
                i++;
            }

            return score;
        }

        public bool GameIsOver()
        {
            return !P1Cards.Any() || !P2Cards.Any();
        }

        public void PlayRound()
        {
            var p1Card = P1Cards[0];
            var p2Card = P2Cards[0];
            P1Cards.RemoveAt(0);
            P2Cards.RemoveAt(0);

            if (p1Card > p2Card)
            {
                P1Cards.Add(p1Card);
                P1Cards.Add(p2Card);
            }
            else
            {
                P2Cards.Add(p2Card);
                P2Cards.Add(p1Card);
            }
        }
    }

    public class RecursiveCombatGame
    {
        private List<int> P1Cards;
        private List<int> P2Cards;

        private List<(List<int> p1, List<int> p2)> previousStates;

        public int? Winner;

        public RecursiveCombatGame(List<int> p1Cards, List<int> p2Cards)
        {
            P1Cards = p1Cards;
            P2Cards = p2Cards;
            previousStates = new List<(List<int> p1, List<int> p2)>();
        }

        public int PlayUntilFinished()
        {
            while (!Winner.HasValue)
            {
                PlayRound();
            }

            return Winner.Value;
        }

        public int GetWinnersScore()
        {
            // Assume sensible use...
            var deckToCount = Winner == 1 ? P1Cards : P2Cards;
            var score = 0;
            var i = 1;
            while (i <= deckToCount.Count)
            {
                score = score + i * deckToCount[^i];
                i++;
            }

            return score;
        }

        public void PlayRound()
        {
            var p1Card = P1Cards[0];
            var p2Card = P2Cards[0];
            P1Cards.RemoveAt(0);
            P2Cards.RemoveAt(0);

            int roundWinner;
            if (P1Cards.Count >= p1Card && P2Cards.Count >= p2Card)
            {
                roundWinner = new RecursiveCombatGame(P1Cards.Take(p1Card).ToList(), P2Cards.Take(p2Card).ToList()).PlayUntilFinished();
            }
            else
            {
                roundWinner = p1Card > p2Card ? 1 : 2;
            }

            if (roundWinner == 1)
            {
                P1Cards.Add(p1Card);
                P1Cards.Add(p2Card);
            }
            else
            {
                P2Cards.Add(p2Card);
                P2Cards.Add(p1Card);
            }

            if (!P1Cards.Any())
            {
                Winner = 2;
            }
            else if (StateIsRepeated() || !P2Cards.Any())
            {
                Winner = 1;
            }
            else
            {
                previousStates.Add((P1Cards.ToList(), P2Cards.ToList()));
            }
        }

        private bool StateIsRepeated()
        {
            return previousStates.Any(StateMatches);
        }

        private bool StateMatches((List<int> p1, List<int> p2) previousState)
        {
            if (P1Cards.Count != previousState.p1.Count || P2Cards.Count != previousState.p2.Count)
            {
                return false;
            }

            if (Enumerable.Range(0, P1Cards.Count).Any(i => P1Cards[i] != previousState.p1[i]))
            {
                return false;
            }

            if (Enumerable.Range(0, P2Cards.Count).Any(i => P2Cards[i] != previousState.p2[i]))
            {
                return false;
            }

            return true;
        }
    }
}
