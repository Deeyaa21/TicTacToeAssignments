using System;
using System.Collections.Generic;

namespace Assignment3AI
{
    class Program
    {
        static char[] board = new char[9];
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Pruning-Based Tic Tac Toe AI (Minimax with Alpha-Beta)\n");
            PlayGame(show: true);

            Console.WriteLine("\nAnalysis:");
            Console.WriteLine("This AI uses Minimax with Alpha-Beta Pruning.");
            Console.WriteLine("It explores optimal moves while cutting unnecessary branches.");
            Console.WriteLine("The AI is unbeatable and always chooses the best move.");
        }

        static void PlayGame(bool show)
        {
            InitializeBoard();
            char ai = 'X';
            char opponent = 'O';
            char current = ai;

            while (HasEmptySquares())
            {
                int move;
                if (current == ai)
                    move = GetBestMove(ai, opponent);
                else
                    move = rnd.Next(AvailableMoves().Count);

                MakeMove(move, current);

                if (show)
                {
                    Console.WriteLine($"{current} moves to position {move}");
                    PrintBoard();
                    Console.WriteLine();
                }

                if (IsWinner(current))
                {
                    Console.WriteLine($"{current} wins!");
                    return;
                }

                current = current == ai ? opponent : ai;
            }

            Console.WriteLine("It's a draw!");
        }

        static void InitializeBoard()
        {
            for (int i = 0; i < 9; i++)
                board[i] = ' ';
        }

        static void PrintBoard()
        {
            for (int i = 0; i < 9; i += 3)
                Console.WriteLine($"| {board[i]} | {board[i + 1]} | {board[i + 2]} |");
        }

        static bool HasEmptySquares() => Array.Exists(board, c => c == ' ');

        static List<int> AvailableMoves()
        {
            List<int> moves = new List<int>();
            for (int i = 0; i < 9; i++)
                if (board[i] == ' ')
                    moves.Add(i);
            return moves;
        }

        static void MakeMove(int index, char player)
        {
            board[index] = player;
        }

        static bool IsWinner(char player)
        {
            int[,] wins = {
                {0,1,2},{3,4,5},{6,7,8},
                {0,3,6},{1,4,7},{2,5,8},
                {0,4,8},{2,4,6}
            };

            for (int i = 0; i < wins.GetLength(0); i++)
            {
                if (board[wins[i, 0]] == player &&
                    board[wins[i, 1]] == player &&
                    board[wins[i, 2]] == player)
                    return true;
            }

            return false;
        }

        static int GetBestMove(char ai, char opponent)
        {
            int bestScore = int.MinValue;
            int bestMove = -1;

            foreach (int move in AvailableMoves())
            {
                board[move] = ai;
                int score = Minimax(0, false, ai, opponent, int.MinValue, int.MaxValue);
                board[move] = ' ';
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        static int Minimax(int depth, bool isMaximizing, char ai, char opponent, int alpha, int beta)
        {
            if (IsWinner(ai)) return 10 - depth;
            if (IsWinner(opponent)) return depth - 10;
            if (!HasEmptySquares()) return 0;

            if (isMaximizing)
            {
                int maxEval = int.MinValue;
                foreach (int move in AvailableMoves())
                {
                    board[move] = ai;
                    int eval = Minimax(depth + 1, false, ai, opponent, alpha, beta);
                    board[move] = ' ';
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha) break; // Prune
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (int move in AvailableMoves())
                {
                    board[move] = opponent;
                    int eval = Minimax(depth + 1, true, ai, opponent, alpha, beta);
                    board[move] = ' ';
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha) break; // Prune
                }
                return minEval;
            }
        }
    }
}

