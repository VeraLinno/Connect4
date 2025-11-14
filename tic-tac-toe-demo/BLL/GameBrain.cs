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

        private Random random = new Random();

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

        public bool BoardCoordinatesAreValid(int x, int y)
        {
            return x >= 0 && x < GameConfiguration.BoardWidth &&
                   y >= 0 && y < GameConfiguration.BoardHeight;
        }

        public EBoardState GetWinner(int x, int y)
        {
            if (GameBoard[x, y] == EBoardState.Empty) return EBoardState.Empty;

            for (int directionIndex = 0; directionIndex < 4; directionIndex++)
            {
                var (dirX, dirY) = GetDirection(directionIndex);
                int count = 1;

                int nextX = x + dirX;
                int nextY = y + dirY;

                while (BoardCoordinatesAreValid(nextX, nextY) && GameBoard[nextX, nextY] == GameBoard[x, y] && count < GameConfiguration.WinCondition)
                {
                    count++;
                    nextX += dirX;
                    nextY += dirY;
                }

                (dirX, dirY) = FlipDirection((dirX, dirY));
                nextX = x + dirX;
                nextY = y + dirY;

                while (BoardCoordinatesAreValid(nextX, nextY) && GameBoard[nextX, nextY] == GameBoard[x, y] && count < GameConfiguration.WinCondition)
                {
                    count++;
                    nextX += dirX;
                    nextY += dirY;
                }

                if (count >= GameConfiguration.WinCondition)
                    return GameBoard[x, y] == EBoardState.X ? EBoardState.XWin : EBoardState.OWin;
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
            for (int y = 0; y < GameConfiguration.BoardHeight && y < list.Count; y++)
            {
                for (int x = 0; x < GameConfiguration.BoardWidth && x < list[y].Count; x++)
                {
                    GameBoard[x, y] = list[y][x];
                }
            }
        }
        public GameConfiguration GetConfiguration()
        {
            return GameConfiguration;
        }

    }
}
