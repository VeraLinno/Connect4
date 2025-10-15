using System;

namespace BLL
{
    public class GameBrain
    {
        private EBoardState[,] GameBoard { get; set; }
        private GameConfiguration GameConfiguration { get; set; }
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

        public void ProcessMove(int x, int y)
        {
            if (x < 0 || x >= GameConfiguration.BoardWidth) return;
            if (y < 0 || y >= GameConfiguration.BoardHeight) return;

            if (GameBoard[x, y] == EBoardState.Empty)
            {
                GameBoard[x, y] = NextMoveByX ? EBoardState.X : EBoardState.O;
                NextMoveByX = !NextMoveByX;
            }
        }

        private (int dirX, int dirY) GetDirection(int directionIndex) =>
            directionIndex switch
            {
                0 => (-1, -1), // Diagonal up-left
                1 => (0, -1),  // Vertical
                2 => (1, -1),  // Diagonal up-right
                3 => (1, 0),   // Horizontal
                _ => (0, 0)
            };

        private (int dirX, int dirY) FlipDirection((int dirX, int dirY) direction) =>
            (-direction.dirX, -direction.dirY);

        private EBoardState GetBoardCell(int x, int y)
        {
            x = (x + GameConfiguration.BoardWidth) % GameConfiguration.BoardWidth;
            return GameBoard[x, y];
        }

        public bool BoardCoordinatesAreValid(int x, int y)
        {
            return x >= 0 && x < (GameConfiguration.BoardWidth) && 
                   y >= 0 && y < (GameConfiguration.BoardHeight);
        }

        public EBoardState GetWinner(int x, int y)
        {
            if (GameBoard[x, y] == EBoardState.Empty) return EBoardState.Empty;

            var player = GameBoard[x, y];

            for (int directionIndex = 0; directionIndex < 4; directionIndex++)
            {
                var (dirX, dirY) = GetDirection(directionIndex);

                int count = 0;
                
                int nextX = x;
                int nextY = y;
                while (BoardCoordinatesAreValid(nextX, nextY) && GetBoardCell(nextX, nextY) == player && count < GameConfiguration.WinCondition)
                {
                    count++;
                    nextX += dirX;
                    nextY += dirY;
                }
                
                (dirX, dirY) = FlipDirection((dirX, dirY));
                nextX = x + dirX;
                nextY = y + dirY;
                while (BoardCoordinatesAreValid(nextX, nextY) && GetBoardCell(nextX, nextY) == player && count < GameConfiguration.WinCondition)
                {
                    count++;
                    nextX += dirX;
                    nextY += dirY;
                }

                if (count >= GameConfiguration.WinCondition)
                    return player == EBoardState.X ? EBoardState.XWin : EBoardState.OWin;
            }

            return EBoardState.Empty;
        }
    }
}
