using BLL;

namespace ConsoleUI;

public static class Ui 
{
    public static void ShowNextPlayer(string nextPlayer)
    {
        Console.WriteLine("Next player: " + nextPlayer);
    }
    
    public static void LoadBoard(EBoardState[, ]  gameBoard)
    {
        //TODO: round up
        //double index = gameBoard.GetLength(0) / 2;
        //var result = Math.Ceiling(index);
        
        Console.Write("   ");
        for (int x = -(gameBoard.GetLength(0) / 2); x < gameBoard.GetLength(0) + (gameBoard.GetLength(0) / 2); x++)
        {
            if (x < 0 || x >= gameBoard.GetLength(0))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[" + GetNumberRepresentation(GetNewIndex(x, gameBoard.GetLength(0)) + 1) + "]");
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
        
        for (int y = 0; y < gameBoard.GetLength(1); y++) 
        {
            for (int x = - (gameBoard.GetLength(0) / 2); x < gameBoard.GetLength(0) + (gameBoard.GetLength(0) / 2); x++) {
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
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(GetNumberRepresentation(y + 1));
            Console.ResetColor();
            
            for (int x = -(gameBoard.GetLength(0) / 2); x < gameBoard.GetLength(0) + (gameBoard.GetLength(0) / 2); x++)
            {
                if (x < 0 || x >= gameBoard.GetLength(0))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("[" + GetCellRepresentation(gameBoard[GetNewIndex(x, gameBoard.GetLength(0)), y]) + "]");
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

    private static int GetNewIndex(int index, int boardWidth)
    {
        int newIndex = ((index % boardWidth) + boardWidth) % boardWidth;
        return newIndex;
    }


    public static void GetWinner(string win)
    {
        Console.WriteLine("Winner is: " + win);
    }
}