using System;
using System.Collections.Generic;

namespace BLL
{
    public class GameBrain
    {
        private EBoardState[,] GameBoard { get; set; }
        public GameConfiguration GameConfiguration { get; set; }

        private bool NextMoveByX { get; set; } = true;

        private readonly Random _random = new Random();
        
        public GameBrain(GameConfiguration configuration, string player1Name, string player2Name)
        {
            GameConfiguration = configuration;
            GameBoard = new EBoardState[configuration.BoardWidth, configuration.BoardHeight];
        }

        public EBoardState[,] GetBoard()
        {
            var copy = new EBoardState[GameConfiguration.BoardWidth, GameConfiguration.BoardHeight];
            Array.Copy(GameBoard, copy, GameBoard.Length);
            return copy;
        }

        public bool IsNextPlayerX() => NextMoveByX;

        public void ProcessMove(int x)
        {
            if (x < 0 || x >= GameConfiguration.BoardWidth) return;

            for (int y = GameConfiguration.BoardHeight - 1; y >= 0; y--)
            {
                if (GameBoard[x, y] == EBoardState.Empty)
                {
                    GameBoard[x, y] = NextMoveByX ? EBoardState.X : EBoardState.O;
                    NextMoveByX = !NextMoveByX;
                    return;
                }
            }
        }

        private int WrapX(int x)
        {
            if (!GameConfiguration.IsCylinder)
                return x;

            return (x + GameConfiguration.BoardWidth) % GameConfiguration.BoardWidth;
        }

        private int GetFirstFreeRow(int x)
        {
            for (int y = GameConfiguration.BoardHeight - 1; y >= 0; y--)
            {
                if (GameBoard[x, y] == EBoardState.Empty)
                    return y;
            }
            return -1;
        }

        public bool IsBoardFull()
        {
            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
                if (GetFirstFreeRow(x) != -1)
                    return false;

            return true;
        }
        
        public EBoardState GetWinner(int x, int y)
        {
            var cell = GameBoard[x, y];
            if (cell == EBoardState.Empty) return EBoardState.Empty;

            int[][] dirs =
            {
                new[] {1, 0},  // horizontal
                new[] {0, 1},  // vertical
                new[] {1, 1},  // diag down-right
                new[] {1, -1}  // diag up-right
            };

            foreach (var d in dirs)
            {
                int count = 1;
                
                count += CountDirection(x, y, d[0], d[1], cell);
                count += CountDirection(x, y, -d[0], -d[1], cell);

                if (count >= GameConfiguration.WinCondition)
                    return cell == EBoardState.X ? EBoardState.XWin : EBoardState.OWin;
            }

            return EBoardState.Empty;
        }

        private int CountDirection(int startX, int startY, int dirX, int dirY, EBoardState cell)
        {
            int count = 0;
            int x = startX + dirX;
            int y = startY + dirY;

            while (y >= 0 && y < GameConfiguration.BoardHeight)
            {
                x = GameConfiguration.IsCylinder ? WrapX(x) : x;

                if (x < 0 || x >= GameConfiguration.BoardWidth)
                    break;

                if (GameBoard[x, y] != cell)
                    break;

                count++;
                x += dirX;
                y += dirY;
            }

            return count;
        }

        public EBoardState GetAnyWinner()
        {
            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
            {
                for (int y = 0; y < GameConfiguration.BoardHeight; y++)
                {
                    var w = GetWinner(x, y);
                    if (w == EBoardState.XWin || w == EBoardState.OWin)
                        return w;
                }
            }
            return EBoardState.Empty;
        }

        public List<List<EBoardState>> GetBoardAsList()
        {
            var list = new List<List<EBoardState>>();

            for (int y = 0; y < GameConfiguration.BoardHeight; y++)
            {
                var row = new List<EBoardState>();
                for (int x = 0; x < GameConfiguration.BoardWidth; x++)
                    row.Add(GameBoard[x, y]);
                list.Add(row);
            }
            return list;
        }

        public void SetBoardFromList(List<List<EBoardState>> board)
        {
            int xCnt = 0, oCnt = 0;

            for (int y = 0; y < GameConfiguration.BoardHeight; y++)
            {
                for (int x = 0; x < GameConfiguration.BoardWidth; x++)
                {
                    GameBoard[x, y] = board[y][x];

                    if (board[y][x] == EBoardState.X) xCnt++;
                    if (board[y][x] == EBoardState.O) oCnt++;
                }
            }

            NextMoveByX = xCnt <= oCnt;
        }
        
        public GameConfiguration GetConfiguration() { return GameConfiguration; }

        public void MakeAiMove()
        {
            var aiPiece = IsNextPlayerX() ? EBoardState.X : EBoardState.O;
            var humanPiece = aiPiece == EBoardState.X ? EBoardState.O : EBoardState.X;

            int depth = GameConfiguration.AiDifficulty switch
            {
                "Easy" => 2,
                "Normal" => 4,
                "Hard" => 6,
                _ => 3
            };

            int bestScore = int.MinValue;
            int bestMove = -1;

            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
            {
                int row = GetFirstFreeRow(x);
                if (row == -1) continue;

                GameBoard[x, row] = aiPiece;

                int score =
                    Minimax(depth - 1, int.MinValue + 1, int.MaxValue - 1,
                        false, aiPiece, humanPiece);

                GameBoard[x, row] = EBoardState.Empty;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = x;
                }
            }

            if (bestMove != -1)
                ProcessMove(bestMove);
        }


        private int EvaluateBoard(EBoardState aiPiece, EBoardState humanPiece)
        {
            int score = 0;
            int center = GameConfiguration.BoardWidth / 2;
            for (int y = 0; y < GameConfiguration.BoardHeight; y++)
            {
                if (GameBoard[center, y] == aiPiece) score += 3;
                if (GameBoard[center, y] == humanPiece) score -= 3;
            }

            // Immediate win/loss potential
            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
            {
                int row = GetFirstFreeRow(x);
                if (row == -1) continue;

                // Simulate AI win
                GameBoard[x, row] = aiPiece;
                if (GetWinner(x, row) == (aiPiece == EBoardState.X ? EBoardState.XWin : EBoardState.OWin))
                    score += 50;

                // Simulate human win
                GameBoard[x, row] = humanPiece;
                if (GetWinner(x, row) == (humanPiece == EBoardState.X ? EBoardState.XWin : EBoardState.OWin))
                    score -= 60;

                GameBoard[x, row] = EBoardState.Empty;
            }

            return score;
        }

        private int Minimax(int depth, int alpha, int beta, bool maximizing,
            EBoardState aiPiece, EBoardState humanPiece)
        {
            var winner = GetAnyWinner();

            if (winner == EBoardState.XWin || winner == EBoardState.OWin ||
                depth == 0 || IsBoardFull())
            {
                if ((winner == EBoardState.XWin && aiPiece == EBoardState.X) ||
                    (winner == EBoardState.OWin && aiPiece == EBoardState.O))
                    return 1000000 + depth;

                if ((winner == EBoardState.XWin && humanPiece == EBoardState.X) ||
                    (winner == EBoardState.OWin && humanPiece == EBoardState.O))
                    return -1000000 - depth;

                return EvaluateBoard(aiPiece, humanPiece);
            }

            if (maximizing)
            {
                int best = int.MinValue;

                for (int x = 0; x < GameConfiguration.BoardWidth; x++)
                {
                    int row = GetFirstFreeRow(x);
                    if (row == -1) continue;

                    GameBoard[x, row] = aiPiece;

                    int eval = Minimax(depth - 1, alpha, beta, false, aiPiece, humanPiece);

                    GameBoard[x, row] = EBoardState.Empty;

                    if (eval > best) best = eval;
                    if (eval > alpha) alpha = eval;

                    if (beta <= alpha) break;
                }

                return best;
            }
            else
            {
                int best = int.MaxValue;

                for (int x = 0; x < GameConfiguration.BoardWidth; x++)
                {
                    int row = GetFirstFreeRow(x);
                    if (row == -1) continue;

                    GameBoard[x, row] = humanPiece;

                    int eval = Minimax(depth - 1, alpha, beta, true, aiPiece, humanPiece);

                    GameBoard[x, row] = EBoardState.Empty;

                    if (eval < best) best = eval;
                    if (eval < beta) beta = eval;

                    if (beta <= alpha) break;
                }

                return best;
            }
        }
    }
}
