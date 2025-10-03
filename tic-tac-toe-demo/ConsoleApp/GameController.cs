using BLL;
using ConsoleUI;

namespace ConsoleApp;

public class GameController
{
    private GameBrain GameBrain { get; set; }

    public GameController()
    {
        GameBrain = new GameBrain(new GameConfiguration(), player1Name: "Player 1", player2Name: "Player 2");
    }
    
    
    public void GameLoop ()
    {
        // Game loop logic here
        
        // get the player move 
        // update gamebrain state (is it over)
        // draw out the UI
        // when game over then stop

        var gameOver = false;

        do
        {
            Console.Clear();
            
            // draw the board
            Ui.LoadBoard(GameBrain.GetBoard());
            Ui.ShowNextPlayer(GameBrain.IsNextPlayerX());
            

            Console.Write("Choice (x,y):");
            var input = Console.ReadLine();
            if (input?.ToLower() == "x")
            {
                gameOver = true;
            }
            if (input == null) continue;
            
            var parts = input.Split(", ");
            if (parts.Length == 2)
            {
                if (int.TryParse(parts[0], out var x) && int.TryParse(parts[1], out var y))
                {
                    GameBrain.ProcessMove(x - 1, y - 1);
                    
                    var winner = GameBrain.GetWinner(x - 1, y);
                    if (winner != EBoardState.Empty)
                    {
                        Console.WriteLine("Winner is: " + (winner == EBoardState.XWin ? "X" : "O")); 
                        break;
                    }
                }
            }

        } while (gameOver == false);
    }
}