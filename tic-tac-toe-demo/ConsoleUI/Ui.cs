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
        for (int x = 0; x < gameBoard.GetLength(0); x++)
        {
            Console.Write("|" + GetNumberRepresentation(x + 1));
        }
        Console.WriteLine(); 
        
        for (int y = 0; y < gameBoard.GetLength(1); y++) 
        {
            for (int x = 0; x < gameBoard.GetLength(0); x++) {
                Console.Write("---+");            
            }
            Console.WriteLine("---");
           
            Console.Write(GetNumberRepresentation(y + 1));
            for (int x = 0; x < gameBoard.GetLength(0); x++)
            {
                Console.Write("|" + GetCellRepresentation(gameBoard[x, y]));
            }
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
            EBoardState.XWin => "XXX",
            EBoardState.OWin => "OOO",
            _ => " ? "
        };
}