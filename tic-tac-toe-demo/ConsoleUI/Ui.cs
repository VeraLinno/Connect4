using BLL;

namespace ConsoleUI;

public static class Ui 
{
    public static void ShowNextPlayer(bool isNextPlayer)
    {
        Console.WriteLine("Next player:" + (isNextPlayer ? "X" : "O"));
    }
    
    public static void LoadBoard(EBoardState[, ]  gameBoard)
    {
        Console.Write("   ");
        for (int x = -3; x < gameBoard.GetLength(0) + 3; x++)
        {
            if (x < 0 || x >= gameBoard.GetLength(0))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[" + GetNumberRepresentation(GetNewIndex(x) + 1) + "]");
            }
            else
            {
                Console.ResetColor();
                Console.Write("|" + GetNumberRepresentation(x + 1) + "|"); 
            }
        }
        Console.WriteLine(); 
        
        for (int y = 0; y < gameBoard.GetLength(1); y++) 
        {
            for (int x = -3; x < gameBoard.GetLength(0) + 3; x++) {
                if (x < 1 || x > gameBoard.GetLength(0))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("---+");
                }
                else
                {
                    Console.ResetColor();
                    Console.Write("---+"); 
                }
            }
            Console.WriteLine("---");
           
            Console.ResetColor();
            Console.Write(GetNumberRepresentation(y + 1));
            
            for (int x = -3; x < gameBoard.GetLength(0) + 3; x++)
            {
                if (x < 0 || x >= gameBoard.GetLength(0))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("[" + GetCellRepresentation(gameBoard[GetNewIndex(x), y]) + "]");
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

    private static string GetNumberRepresentation(int number)
    {
        return " " + (number < 10 ? "0" + number : number.ToString());
    }

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

    private static int GetNewIndex(int index) =>
        index switch
        {
            -3 => 2,
            -2 => 3,
            -1 => 4,
            5 => 0,
            6 => 1,
            7 => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
        };

    public static void GetWinner(string win)
    {
        Console.WriteLine("Winner is: " + win);
    }
}