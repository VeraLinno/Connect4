namespace BLL;

public class GameBrain
{
    private EBoardState[,] GameBoard { get; set; }
    private GameConfiguration GameConfiguration { get; set; }
    private string Player1Name { get; set; }
    private string Player2Name { get; set; }

    private bool NextMoveByX { get; set; } = true;

    private Random rnd = new Random();

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
            1 => (0, -1), // Vertical
            2 => (1, -1), // Diagonal up-right
            3 => (1, 0), // horizontal
            _ => (0, 0)
        };

    private (int dirX, int dirY) FlipDirection((int dirX, int dirY) direction) =>
        (-direction.dirX, -direction.dirY);

    public bool BoardCoordinatesAreValid(int x, int y)
    {
        if (x < 0 || x >= GameConfiguration.BoardWidth) return false;
        if (y < 0 || y >= GameConfiguration.BoardHeight) return false;
        return true;
    }

    public EBoardState GetWinner(int x, int y)
    {
        if (GameBoard[x, y] == EBoardState.Empty) return EBoardState.Empty;

        

        for (int directionIndex = 0; directionIndex < 4; directionIndex++)
        {
            var (dirX, dirY) = GetDirection(directionIndex);

            var count = 0;
            
            var nextX = x;
            var nextY = y;
            while (BoardCoordinatesAreValid(nextX, nextY) && GameBoard[x, y] == GameBoard[nextX, nextY] &&
                   count <= GameConfiguration.WinCondition)
            {
                count++;
                nextX += dirX;
                nextY += dirY;
            }

            if (count < GameConfiguration.WinCondition)
            {
                (dirX, dirY) = FlipDirection((dirX, dirY));
                nextX = x + dirX;
                nextY = y + dirY;
                while (BoardCoordinatesAreValid(nextX, nextY) && GameBoard[x, y] == GameBoard[nextX, nextY] &&
                       count <= GameConfiguration.WinCondition)
                {
                    count++;
                    nextX += dirX;
                    nextY += dirY;
                }
            }
            
            if (count == GameConfiguration.WinCondition)
            {
                return GameBoard[x, y] == EBoardState.X ? EBoardState.XWin : EBoardState.OWin;
            }

        }
        return EBoardState.Empty;
    }
}