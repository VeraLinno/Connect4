using BLL;
using ConsoleUI;
using DAL;

namespace ConsoleApp;

public class GameController
{
    private readonly IRepository<GameConfiguration> _configRepo;
    private readonly GameBrain _gameBrain;
    private readonly GameConfiguration? _existingConfig;

    public GameController(IRepository<GameConfiguration> configRepo, GameConfiguration? existingConfig = null)
    {
        _configRepo = configRepo;
        _existingConfig = existingConfig;

        if (existingConfig == null)
        {
            var newConfig = new GameConfiguration();
            _gameBrain = new GameBrain(newConfig, "Player 1", "Player 2");
        }
        else
        {
            _gameBrain = new GameBrain(existingConfig, "Player 1", "Player 2");
            _gameBrain.SetBoardFromList(existingConfig.BoardState);
        }
    }

    public void GameLoop()
    {
        var gameOver = false;
        do
        {
            Console.Clear();

            Ui.LoadBoard(_gameBrain.GetBoard());
            Ui.ShowNextPlayer(_gameBrain.IsNextPlayerX());

            Console.Write("Choice (x) ('s' to save, 'x' to exit): ");
            var input = Console.ReadLine();
            if (input?.ToLower() == "x")
            {
                gameOver = true;
                continue;
            }
            
            if (input?.ToLower() == "s")
            {
                Save();
                continue;
            }

            if (int.TryParse(input, out var column))
            {
                column -= 1;
                if (column >= 0 && column < _gameBrain.GetBoard().GetLength(0))
                {
                    _gameBrain.ProcessMove(column);

                    int row = 0;
                    for (int r = 0; r < _gameBrain.GetBoard().GetLength(1); r++)
                    {
                        if (_gameBrain.GetBoard()[column, r] != EBoardState.Empty)
                        {
                            row = r;
                            break;
                        }
                    }

                    var winner = _gameBrain.GetWinner(column, row);
                    if (winner != EBoardState.Empty)
                    {
                        string win = winner == EBoardState.XWin ? "X" : "O";
                        Ui.GetWinner(win);
                        
                        Save();
                        gameOver = true;
                    }
                }
            }

        } while (!gameOver);
    }

    private void Save()
    {
        var currentConfig = _gameBrain.GetConfiguration();
        currentConfig.BoardState = _gameBrain.GetBoardAsList();

        if (_existingConfig != null)
        {
            currentConfig.Name = _existingConfig.Name;
            var savedFile = _configRepo.Save(currentConfig);
            Console.WriteLine($"Existing game updated: {savedFile}");
        }
        else
        {
            Console.Write("Write a name for the game, 0 to cancel: ");
            var userChoice = Console.ReadLine(); 
        
            if (!string.IsNullOrWhiteSpace(userChoice))
            {
                currentConfig.Name = userChoice;
                var savedFile = _configRepo.Save(currentConfig);
                    
                Console.WriteLine($"Game saved to: {savedFile}");
            }
            else
            {
                Console.WriteLine("Save cancelled.");
            }
        }
    }
}
