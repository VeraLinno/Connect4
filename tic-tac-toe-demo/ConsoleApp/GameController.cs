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
                IsCylinder = isCylinder
            };

            _gameBrain = new GameBrain(newConfig, player1Name, player2Name);
        }
        else
        {
            existingConfig.BoardWidth = width;
            existingConfig.BoardHeight = height;
            existingConfig.WinCondition = winCondition;
            existingConfig.IsCylinder = isCylinder;

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

            Ui.LoadBoard(_gameBrain.GetBoard(), _gameBrain.GetConfiguration().IsCylinder);

            string nextPlayer = _gameBrain.IsNextPlayerX()
                ? _gameBrain.GameConfiguration.Player1Name + " (X)"
                : _gameBrain.GameConfiguration.Player2Name + " (O)";

            Ui.ShowNextPlayer(nextPlayer);

            Console.Write("Choice (column), 's' to save, 'x' to exit: ");
            var input = Console.ReadLine();

            if (input?.ToLower() == "x") break;
            if (input?.ToLower() == "s") { Save(); continue; }

            if (!int.TryParse(input, out int col)) continue;

            col -= 1;
            if (col < 0 || col >= _gameBrain.GetBoard().GetLength(0)) continue;

            _gameBrain.ProcessMove(col);

            int row = FindPlacedRow(col);

            var winner = _gameBrain.GetWinner(col, row);
            if (winner != EBoardState.Empty)
            {
                string winName = winner == EBoardState.XWin
                    ? _gameBrain.GameConfiguration.Player1Name + " (X)"
                    : _gameBrain.GameConfiguration.Player2Name + " (O)";

                Ui.GetWinner(winName);
                Save();
                return;
            }
        }
    }

    private int FindPlacedRow(int column)
    {
        for (int y = 0; y < _gameBrain.GetBoard().GetLength(1); y++)
            if (_gameBrain.GetBoard()[column, y] != EBoardState.Empty)
                return y;

        return 0;
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
