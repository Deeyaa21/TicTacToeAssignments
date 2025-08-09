using System;
using System.Collections.Generic;

namespace Assignment2AI
{
    class Program
    {
        static char[] board = new char[9];
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Rule-Based Tic Tac Toe AI\n");
            PlayGame(show: true);

            Console.WriteLine("\nSimulation of 50 games against random player:\n");
            SimulateGames(50);

            Console.WriteLine("\nAnalysis:");
            Console.WriteLine("The rule engine follows a fixed priority:");
            Console.WriteLine("1. Win if possible");
            Console.WriteLine("2. Block opponent's win");
            Console.WriteLine("3. Take center");
            Console.WriteLine("4. Take a corner");
            Console.WriteLine("5. Take a side");
            Console.WriteLine("It performs well against random players but lacks learning or adaptation.");
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

        static bool IsWinnerBoard(char[] testBoard, int square, char player)
        {
            int[,] lines = new int[,] {
                {0,1,2}, {3,4,5}, {6,7,8},
                {0,3,6}, {1,4,7}, {2,5,8},
                {0,4,8}, {2,4,6}
            };
            for (int i = 0; i < 8; i++)
            {
                if (testBoard[lines[i, 0]] == player &&
                    testBoard[lines[i, 1]] == player &&
                    testBoard[lines[i, 2]] == player)
                    return true;
            }
            return false;
        }

        static int RuleBasedMove(char ai, char opp)
        {
            var moves = AvailableMoves();

            // 1. Win if possible
            foreach (int move in moves)
            {
                char[] copy = (char[])board.Clone();
                copy[move] = ai;
                if (IsWinnerBoard(copy, move, ai)) return move;
            }

            // 2. Block opponent's win
            foreach (int move in moves)
            {
                char[] copy = (char[])board.Clone();
                copy[move] = opp;
                if (IsWinnerBoard(copy, move, opp)) return move;
            }

            // 3. Take center
            if (moves.Contains(4)) return 4;

            // 4. Take corner
            foreach (int i in new int[] { 0, 2, 6, 8 })
                if (moves.Contains(i)) return i;

            // 5. Take side
            foreach (int i in new int[] { 1, 3, 5, 7 })
                if (moves.Contains(i)) return i;

            // Fallback
            return moves.Count > 0 ? moves[0] : -1;
        }

        static char PlayGame(bool show = false)
        {
            InitializeBoard();
            char current = 'X';
            char ai = 'X';
            char opp = 'O';

            while (AvailableMoves().Count > 0)
            {
                int move;
                if (current == ai)
                    move = RuleBasedMove(ai, opp);
                else
                    move = rnd.Next(AvailableMoves().Count) >= 0 ? AvailableMoves()[rnd.Next(AvailableMoves().Count)] : -1;

                if (move == -1) break;

                board[move] = current;

                if (show)
                {
                    Console.WriteLine($"{current} moves to {move}");
                    PrintBoard();
                    Console.WriteLine();
                }

                if (IsWinner(current)) return current;
                current = current == 'X' ? 'O' : 'X';
            }

            return 'D';
        }

        static void SimulateGames(int count)
        {
            int wins = 0, losses = 0, draws = 0;
            for (int i = 0; i < count; i++)
            {
                char result = PlayGame(show: false);
                if (result == 'X') wins++;
                else if (result == 'O') losses++;
                else draws++;
            }

            Console.WriteLine($"AI Wins: {wins}");
            Console.WriteLine($"AI Losses: {losses}");
            Console.WriteLine($"Draws: {draws}");
        }
    }
}
