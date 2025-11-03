using BLL;
using ConsoleUI;

namespace ConsoleApp;

public class GameController
{
    private GameBrain GameBrain { get; set; }

    public GameController()
    {
        GameBrain = new GameBrain(new GameConfiguration(), "Player 1", "Player 2");
    }

    public void GameLoop()
    {
        // Game loop logic here

        // get the player move
        // update gamebrain state
        // draw out the ui
        // when game over, stop

        var gameOver = false;
        do
        {
            Console.Clear();

            // draw the board
            Ui.LoadBoard(GameBrain.GetBoard());
            Ui.ShowNextPlayer(GameBrain.IsNextPlayerX());

            Console.Write("Choice (x):");
            var input = Console.ReadLine();
            if (input?.ToLower() == "x")
            {
                gameOver = true;
            }
            
            if (input == null ) continue;
            if (int.TryParse(input, out var column))
            {
                column -= 1;
                if (column >= 0 && column < GameBrain.GetBoard().GetLength(0))
                {
                    GameBrain.ProcessMove(column);
                    
                    int row = 0;
                    for (int r = 0; r < GameBrain.GetBoard().GetLength(1); r++)
                    {
                        if (GameBrain.GetBoard()[column, r] != EBoardState.Empty)
                        {
                            row = r;
                            break;
                        }
                    }
                    var winner = GameBrain.GetWinner(column, row);
                    if (winner != EBoardState.Empty)
                    {
                        string win = winner == EBoardState.XWin ? "X" : "O";
                        Ui.GetWinner(win);
                        break;
                    }
                }
            }
        } while (gameOver == false);
    }
}