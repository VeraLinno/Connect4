using BLL;
using ConsoleUI;
using DAL;

namespace ConsoleApp;

public class GameController
{
    private readonly IRepository<GameConfiguration> _configRepo;
    private readonly GameBrain _gameBrain;
    private readonly GameConfiguration? _existingConfig;

    public GameController(IRepository<GameConfiguration> configRepo,
        string player1Name, string player2Name,
        GameConfiguration? existingConfig = null)
    {
        _configRepo = configRepo;
        _existingConfig = existingConfig;

        if (existingConfig == null)
        {
            var newConfig = new GameConfiguration
            {
                Player1Name = player1Name,
                Player2Name = player2Name,
                IsCylinder = false
            };

            _gameBrain = new GameBrain(newConfig, player1Name, player2Name);
        }
        else
        {
            _gameBrain = new GameBrain(existingConfig, player1Name, player2Name);
            _gameBrain.SetBoardFromList(existingConfig.BoardState);
        }
    }

    public GameController(
        IRepository<GameConfiguration> configRepo,
        string player1Name,
        string player2Name,
        int width,
        int height,
        int winCondition,
        bool isCylinder,
        string gameMode,
        GameConfiguration? existingConfig = null)
    {
        _configRepo = configRepo;
        _existingConfig = existingConfig;

        if (existingConfig == null)
        {
            var newConfig = new GameConfiguration
            {
                Player1Name = player1Name,
                Player2Name = player2Name,
                BoardWidth = width,
                BoardHeight = height,
                WinCondition = winCondition,
                IsCylinder = isCylinder,
                GameMode = gameMode
            };

            _gameBrain = new GameBrain(newConfig, player1Name, player2Name);
        }
        else
        {
            existingConfig.BoardWidth = width;
            existingConfig.BoardHeight = height;
            existingConfig.WinCondition = winCondition;
            existingConfig.IsCylinder = isCylinder;
            existingConfig.GameMode = gameMode;

            _gameBrain = new GameBrain(existingConfig, player1Name, player2Name);
            _gameBrain.SetBoardFromList(existingConfig.BoardState);
        }
    }


    public void GameLoop()
    {
        bool gameOver = false;

        while (!gameOver)
        {
            Console.Clear();

            var cfg = _gameBrain.GetConfiguration();

            Ui.LoadBoard(_gameBrain.GetBoard(), cfg.IsCylinder);

            string nextPlayer = _gameBrain.IsNextPlayerX()
                ? cfg.Player1Name + " (X)"
                : cfg.Player2Name + " (O)";

            Ui.ShowNextPlayer(nextPlayer);

            if (cfg.GameMode == "EVE")
            {
                Thread.Sleep(600);
                _gameBrain.MakeAiMove();
                if (CheckWinOrDraw(ref gameOver)) return;
                continue;
            }

            if (cfg.GameMode == "PVE" && !_gameBrain.IsNextPlayerX())
            {
                Thread.Sleep(500);
                _gameBrain.MakeAiMove();
                if (CheckWinOrDraw(ref gameOver)) return;
                continue;
            }
            
            Console.Write("Choice (column), 's' to save, 'x' to exit: ");
            var input = Console.ReadLine();

            if (input?.ToLower() == "x") return;
            if (input?.ToLower() == "s") { Save(); continue; }

            if (!int.TryParse(input, out int col)) continue;

            col -= 1;
            if (col < 0 || col >= _gameBrain.GetBoard().GetLength(0)) continue;

            _gameBrain.ProcessMove(col);

            if (CheckWinOrDraw(ref gameOver)) return;
        }
    }

    private bool CheckWinOrDraw(ref bool gameOver)
    {
        var cfg = _gameBrain.GetConfiguration();
        var board = _gameBrain.GetBoard();
        int w = board.GetLength(0);
        int h = board.GetLength(1);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                var result = _gameBrain.GetWinner(x, y);
                if (result != EBoardState.Empty)
                {
                    string winName = result == EBoardState.XWin
                        ? cfg.Player1Name + " (X)"
                        : cfg.Player2Name + " (O)";

                    Ui.GetWinner(winName);
                    Save();
                    gameOver = true;
                    return true;
                }
            }
        }

        if (_gameBrain.IsBoardFull())
        {
            Console.WriteLine("DRAW! Board is full.");
            Save();
            gameOver = true;
            return true;
        }

        return false;
    }

    private void Save()
    {
        var cfg = _gameBrain.GetConfiguration();
        cfg.BoardState = _gameBrain.GetBoardAsList();

        if (_existingConfig != null)
        {
            cfg.Name = _existingConfig.Name;
            var saved = _configRepo.Save(cfg);
            Console.WriteLine($"Existing game updated: {saved}");
        }
        else
        {
            Console.Write("Enter save name (0 to cancel): ");
            var name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || name == "0")
            {
                Console.WriteLine("Save cancelled.");
                return;
            }

            cfg.Name = name;
            var saved = _configRepo.Save(cfg);
            Console.WriteLine($"Saved as: {saved}");
        }
    }
}
