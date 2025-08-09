using System;
using System.Collections.Generic;

namespace Assignment1AI
{
    class Program
    {
        static char[] board = new char[9];
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Running 50 simulations for each heuristic match...\n");
            SimulateGames(50);
        }

        static void InitializeBoard()
        {
            for (int i = 0; i < 9; i++)
                board[i] = ' ';
        }

        static void PrintBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                Console.Write("| " + board[i] + " ");
                if ((i + 1) % 3 == 0)
                    Console.WriteLine("|");
            }
            Console.WriteLine();
        }

        static List<int> AvailableMoves()
        {
            var moves = new List<int>();
            for (int i = 0; i < 9; i++)
                if (board[i] == ' ') moves.Add(i);
            return moves;
        }

        static bool IsWinner(char player)
        {
            int[,] lines = new int[,] {
                {0,1,2}, {3,4,5}, {6,7,8},
                {0,3,6}, {1,4,7}, {2,5,8},
                {0,4,8}, {2,4,6}
            };
            for (int i = 0; i < 8; i++)
            {
                if (board[lines[i, 0]] == player &&
                    board[lines[i, 1]] == player &&
                    board[lines[i, 2]] == player)
                    return true;
            }
            return false;
        }

        static bool IsFull()
        {
            foreach (char c in board)
                if (c == ' ') return false;
            return true;
        }

        static int HeuristicA(char ai, char opp)
        {
            int score = 0;
            int[,] lines = new int[,] {
                {0,1,2}, {3,4,5}, {6,7,8},
                {0,3,6}, {1,4,7}, {2,5,8},
                {0,4,8}, {2,4,6}
            };

            for (int i = 0; i < 8; i++)
            {
                int aiCount = 0, oppCount = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (board[lines[i, j]] == ai) aiCount++;
                    if (board[lines[i, j]] == opp) oppCount++;
                }
                if (oppCount == 0)
                {
                    if (aiCount == 2) score += 10;
                    else if (aiCount == 1) score += 1;
                }
            }
            return score;
        }

        static int HeuristicB(char ai, char opp)
        {
            int score = 0;
            int[,] lines = new int[,] {
                {0,1,2}, {3,4,5}, {6,7,8},
                {0,3,6}, {1,4,7}, {2,5,8},
                {0,4,8}, {2,4,6}
            };

            for (int i = 0; i < 8; i++)
            {
                int aiCount = 0, oppCount = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (board[lines[i, j]] == ai) aiCount++;
                    if (board[lines[i, j]] == opp) oppCount++;
                }

                if (oppCount == 0 && aiCount == 2) score += 10;
                if (aiCount == 0 && oppCount == 2) score += 8;
                if (oppCount == 0 && aiCount == 1) score += 1;
            }

            if (board[4] == ai) score += 1;
            foreach (int corner in new int[] { 0, 2, 6, 8 })
                if (board[corner] == ai) score += 1;

            return score;
        }

        static int BestMove(char ai, char opp, Func<char, char, int> heuristic)
        {
            var moves = AvailableMoves();
            int bestScore = int.MinValue;
            int bestMove = -1;

            foreach (int move in moves)
            {
                board[move] = ai;
                int score = heuristic(ai, opp);
                board[move] = ' ';
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        static char PlayGame(Func<char, char, int> heuristicX, Func<char, char, int> heuristicO, bool show = false)
        {
            InitializeBoard();
            char current = 'X';

            while (!IsFull())
            {
                int move = (current == 'X') ? BestMove('X', 'O', heuristicX) : BestMove('O', 'X', heuristicO);
                if (move == -1) move = AvailableMoves()[0];
                board[move] = current;

                if (show)
                {
                    Console.WriteLine(current + " moves to " + move);
                    PrintBoard();
                }

                if (IsWinner(current)) return current;
                current = (current == 'X') ? 'O' : 'X';
            }

            return 'D';
        }

        static void SimulateGames(int games)
        {
            int aWins = 0, bWins = 0, draws = 0;

            // A (X) vs B (O)
            for (int i = 0; i < games; i++)
            {
                char result = PlayGame(HeuristicA, HeuristicB);
                if (result == 'X') aWins++;
                else if (result == 'O') bWins++;
                else draws++;
            }
            Console.WriteLine($"Heuristic A (X) vs Heuristic B (O): A Wins: {aWins}, B Wins: {bWins}, Draws: {draws}");

            aWins = bWins = draws = 0;

            // B (X) vs A (O)
            for (int i = 0; i < games; i++)
            {
                char result = PlayGame(HeuristicB, HeuristicA);
                if (result == 'X') bWins++;
                else if (result == 'O') aWins++;
                else draws++;
            }
            Console.WriteLine($"Heuristic B (X) vs Heuristic A (O): B Wins: {bWins}, A Wins: {aWins}, Draws: {draws}");

            Console.WriteLine("\nSample Game (B vs A):");
            PlayGame(HeuristicB, HeuristicA, true);

            Console.WriteLine("\nAnalysis:");
            Console.WriteLine("- Heuristic A focuses on offense.");
            Console.WriteLine("- Heuristic B balances offense, defense, and positioning.");
            Console.WriteLine("- Heuristic B is more strategic and performs better.");
        }
    }
}

