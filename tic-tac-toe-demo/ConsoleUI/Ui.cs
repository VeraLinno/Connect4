using BLL;

namespace ConsoleUI;

public static class Ui
{
    public static void ShowNextPlayer(string nextPlayer)
    {
        Console.WriteLine("Next player: " + nextPlayer);
    }

    public static void LoadBoard(EBoardState[,] gameBoard, bool isCylinder)
    {
        if (isCylinder)
            DrawCylinderBoard(gameBoard);
        else
            DrawClassicalBoard(gameBoard);
    }

    private static void DrawClassicalBoard(EBoardState[,] board)
    {
        int w = board.GetLength(0);
        int h = board.GetLength(1);

        Console.Write("   ");
        for (int x = 0; x < w; x++)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("|" + GetNumberRepresentation(x + 1) + "|");
            Console.ResetColor();
        }
        Console.WriteLine();

        for (int y = 0; y < h; y++)
        {
            Console.Write("   ");
            for (int x = 0; x < w; x++)
                Console.Write("---+-");
            Console.WriteLine("---");

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(GetNumberRepresentation(y + 1));
            Console.ResetColor();

            for (int x = 0; x < w; x++)
            {
                Console.Write("|" + GetCellRepresentation(board[x, y]) + "|");
            }
            Console.WriteLine();
        }
    }

    private static void DrawCylinderBoard(EBoardState[,] gameBoard)
    {
        int width = gameBoard.GetLength(0);
        int height = gameBoard.GetLength(1);
        int half = (width + 1) / 2;

        Console.Write("   ");
        for (int x = -half; x < width + half; x++)
        {
            if (x < 0 || x >= width)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[" + GetNumberRepresentation(GetNewIndex(x, width) + 1) + "]");
            }
            else
            {
                Console.ResetColor();
                Console.Write("|");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(GetNumberRepresentation(x + 1));
                Console.ResetColor();
                Console.Write("|");
            }
        }
        Console.WriteLine();

        for (int y = 0; y < height; y++)
        {
            for (int x = -half; x < width + half; x++)
            {
                if (x < 1 || x > width)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("---+-");
                }
                else
                {
                    Console.ResetColor();
                    Console.Write("---+-");
                }
            }
            Console.WriteLine("---");

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(GetNumberRepresentation(y + 1));
            Console.ResetColor();

            for (int x = -half; x < width + half; x++)
            {
                if (x < 0 || x >= width)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("[" + GetCellRepresentation(gameBoard[GetNewIndex(x, width), y]) + "]");
                }
                else
                {
                    Console.ResetColor();
                    Console.Write("|" + GetCellRepresentation(gameBoard[x, y]) + "|");
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }


    private static int GetNewIndex(int index, int width)
        => ((index % width) + width) % width;

    private static string GetNumberRepresentation(int number)
        => " " + (number < 10 ? "0" + number : number.ToString());

    private static string GetCellRepresentation(EBoardState cellValue) =>
        cellValue switch
        {
            EBoardState.Empty => "   ",
            EBoardState.X => " X ",
            EBoardState.O => " O ",
            EBoardState.XWin => "XXXX",
            EBoardState.OWin => "OOOO",
            _ => " ? "
        };

    public static void GetWinner(string win)
    {
        Console.WriteLine("Winner is: " + win);
    }
}
