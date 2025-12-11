using System;

namespace BLL
{
    public class GameBrain
    {
        private EBoardState[,] GameBoard { get; set; }
        public GameConfiguration GameConfiguration { get; set; }
        private string Player1Name { get; set; }
        private string Player2Name { get; set; }

        private bool NextMoveByX { get; set; } = true;

        private readonly Random _random = new Random();

        public GameBrain(GameConfiguration configuration, string player1Name, string player2Name, int width, int height)
        {
            GameConfiguration = configuration;
            Player1Name = player1Name;
            Player2Name = player2Name;
            
            if (width > 0) GameConfiguration.BoardWidth = width;
            if (height > 0) GameConfiguration.BoardHeight = height;
            GameBoard = new EBoardState[configuration.BoardWidth, configuration.BoardHeight];
        }
        
        public GameBrain(GameConfiguration configuration, string player1Name, string player2Name)
        {
            GameConfiguration = configuration;
            Player1Name = player1Name;
            Player2Name = player2Name;
            GameBoard = new EBoardState[configuration.BoardWidth, configuration.BoardHeight];
        }

        public EBoardState[,] GetBoard()
        {
            var gameBoardCopy = new EBoardState[GameConfiguration.BoardWidth, GameConfiguration.BoardHeight];
            Array.Copy(GameBoard, gameBoardCopy, GameBoard.Length);
            return gameBoardCopy;
        }

        public bool IsNextPlayerX() => NextMoveByX;

        public void ProcessMove(int x)
        {
            if (x < 0 || x >= GameConfiguration.BoardWidth) return;
            
            for (int row = GameConfiguration.BoardHeight - 1; row >= 0; row--)
            {
                if (GameBoard[x, row] == EBoardState.Empty)
                {
                    GameBoard[x, row] = NextMoveByX ? EBoardState.X : EBoardState.O;
                    NextMoveByX = !NextMoveByX;
                    break;
                }
            }
        }

        private (int dirX, int dirY) GetDirection(int directionIndex) =>
            directionIndex switch
            {
                0 => (-1, -1), // Diagonal up-left
                1 => (0, -1),  // Vertical up
                2 => (1, -1),  // Diagonal up-right
                3 => (1, 0),   // Horizontal
                _ => (0, 0)
            };

        private (int dirX, int dirY) FlipDirection((int dirX, int dirY) direction) =>
            (-direction.dirX, -direction.dirY);

        public EBoardState GetWinner(int x, int y)
        {
            if (GameBoard[x, y] == EBoardState.Empty) 
                return EBoardState.Empty;

            var startCell = GameBoard[x, y];

            for (int directionIndex = 0; directionIndex < 4; directionIndex++)
            {
                var (dirX, dirY) = GetDirection(directionIndex);
                int count = 1;
                
                int nextX = x + dirX;
                int nextY = y + dirY;

                while (nextY >= 0 &&
                       nextY < GameConfiguration.BoardHeight &&
                       count < GameConfiguration.WinCondition)
                {
                    nextX = (nextX + GameConfiguration.BoardWidth) % GameConfiguration.BoardWidth;

                    if (GameBoard[nextX, nextY] != startCell)
                        break;

                    count++;
                    nextX += dirX;
                    nextY += dirY;
                }
                
                (dirX, dirY) = FlipDirection((dirX, dirY));

                nextX = x + dirX;
                nextY = y + dirY;

                while (nextY >= 0 &&
                       nextY < GameConfiguration.BoardHeight &&
                       count < GameConfiguration.WinCondition)
                {
                    nextX = (nextX + GameConfiguration.BoardWidth) % GameConfiguration.BoardWidth;

                    if (GameBoard[nextX, nextY] != startCell)
                        break;

                    count++;
                    nextX += dirX;
                    nextY += dirY;
                }

                if (count >= GameConfiguration.WinCondition)
                    return startCell == EBoardState.X ? EBoardState.XWin : EBoardState.OWin;
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
                {
                    row.Add(GameBoard[x, y]);
                }
                list.Add(row);
            }
            return list;
        }

        public void SetBoardFromList(List<List<EBoardState>>? list)
        {
            if (list == null) return;

            int xCount = 0;
            int oCount = 0;

            for (int y = 0; y < GameConfiguration.BoardHeight && y < list.Count; y++)
            {
                for (int x = 0; x < GameConfiguration.BoardWidth && x < list[y].Count; x++)
                {
                    GameBoard[x, y] = list[y][x];

                    if (list[y][x] == EBoardState.X || list[y][x] == EBoardState.XWin)
                        xCount++;

                    if (list[y][x] == EBoardState.O || list[y][x] == EBoardState.OWin)
                        oCount++;
                }
            }
            NextMoveByX = xCount <= oCount;
        }

        public GameConfiguration GetConfiguration()
        {
            return GameConfiguration;
        }
        
        public void MakeAiMove()
        {
            if (!GameConfiguration.IsVsAi) return;
            if (IsNextPlayerX()) return;

            switch (GameConfiguration.AiDifficulty)
            {
                case "Easy":
                    MakeRandomMove();
                    break;
                case "Normal":
                    MakeMediumMove();
                    break;
                case "Hard":
                    MakeHardMove();
                    break;
                default:
                    MakeRandomMove();
                    break;
            }
        }

        private int GetFirstFreeRow(int x)
        {
            if (x < 0 || x >= GameConfiguration.BoardWidth) return -1;

            for (int row = GameConfiguration.BoardHeight - 1; row >= 0; row--)
            {
                if (GameBoard[x, row] == EBoardState.Empty)
                    return row;
            }
            return -1;
        }

        private bool WouldMoveWin(int x, EBoardState player)
        {
            var row = GetFirstFreeRow(x);
            if (row == -1) return false;

            var original = GameBoard[x, row];
            GameBoard[x, row] = player;

            var res = GetWinner(x, row);

            GameBoard[x, row] = original;

            return (player == EBoardState.X && res == EBoardState.XWin)
                   || (player == EBoardState.O && res == EBoardState.OWin);
        }

        private void MakeRandomMove()
        {
            var freeColumns = new List<int>();
            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
            {
                if (GetFirstFreeRow(x) != -1)
                    freeColumns.Add(x);
            }

            if (freeColumns.Count == 0) return;

            int choice = freeColumns[_random.Next(freeColumns.Count)];
            ProcessMove(choice);
        }

        private void MakeMediumMove()
        {
            var aiPiece = EBoardState.O;
            var humanPiece = EBoardState.X;

            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
            {
                if (GetFirstFreeRow(x) == -1) continue;
                if (WouldMoveWin(x, aiPiece))
                {
                    ProcessMove(x);
                    return;
                }
            }
            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
            {
                if (GetFirstFreeRow(x) == -1) continue;
                if (WouldMoveWin(x, humanPiece))
                {
                    ProcessMove(x);
                    return;
                }
            }

            var freeColumns = new List<int>();
            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
            {
                if (GetFirstFreeRow(x) != -1)
                    freeColumns.Add(x);
            }
            if (freeColumns.Count == 0) return;

            int center = GameConfiguration.BoardWidth / 2;
            freeColumns.Sort((a, b) =>
                Math.Abs(a - center).CompareTo(Math.Abs(b - center)));

            int take = Math.Min(3, freeColumns.Count);
            int choice = freeColumns[_random.Next(take)];
            ProcessMove(choice);
        }
        
        private int EvaluateMoveScore(int x, EBoardState aiPiece, EBoardState humanPiece)
        {
            int row = GetFirstFreeRow(x);
            if (row == -1) return int.MinValue;
            
            GameBoard[x, row] = aiPiece;
            
            if (WouldMoveWin(x, aiPiece))
            {
                GameBoard[x, row] = EBoardState.Empty;
                return 100000;
            }

            int losingReplies = 0;
            int aiThreats = 0;
            
            for (int hx = 0; hx < GameConfiguration.BoardWidth; hx++)
            {
                if (GetFirstFreeRow(hx) == -1) continue;

                if (WouldMoveWin(hx, humanPiece))
                {
                    losingReplies++;
                }
            }
            
            for (int ax = 0; ax < GameConfiguration.BoardWidth; ax++)
            {
                if (GetFirstFreeRow(ax) == -1) continue;

                if (WouldMoveWin(ax, aiPiece))
                {
                    aiThreats++;
                }
            }
            
            int center = GameConfiguration.BoardWidth / 2;
            int centerDistance = Math.Abs(x - center);
            
            GameBoard[x, row] = EBoardState.Empty;
            int score = 0;
            score += aiThreats * 10;
            score -= losingReplies * 50;
            score -= centerDistance;

            return score;
        }


        private void MakeHardMove()
        {
            var aiPiece = EBoardState.O;
            var humanPiece = EBoardState.X;

            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
            {
                if (GetFirstFreeRow(x) == -1) continue;
                if (WouldMoveWin(x, aiPiece))
                {
                    ProcessMove(x);
                    return;
                }
            }
            
            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
            {
                if (GetFirstFreeRow(x) == -1) continue;
                if (WouldMoveWin(x, humanPiece))
                {
                    ProcessMove(x);
                    return;
                }
            }

            var candidateColumns = new List<int>();
            for (int x = 0; x < GameConfiguration.BoardWidth; x++)
            {
                if (GetFirstFreeRow(x) != -1)
                    candidateColumns.Add(x);
            }

            if (candidateColumns.Count == 0) return;

            int bestScore = int.MinValue;
            var bestColumns = new List<int>();

            foreach (var col in candidateColumns)
            {
                int score = EvaluateMoveScore(col, aiPiece, humanPiece);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestColumns.Clear();
                    bestColumns.Add(col);
                }
                else if (score == bestScore)
                {
                    bestColumns.Add(col);
                }
            }
            int choice = bestColumns[_random.Next(bestColumns.Count)];
            ProcessMove(choice);
        }


    }
}
