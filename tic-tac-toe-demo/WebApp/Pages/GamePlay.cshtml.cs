using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class GamePlay : PageModel
{
    private readonly IRepository<GameConfiguration> _configRepo;

    public GamePlay(IRepository<GameConfiguration> configRepo)
    {
        _configRepo = configRepo;
    }

    public string GameId { get; set; } = default!;
    public GameBrain GameBrain { get; set; } = default!;
    public string Winner { get; set; } = "";

    public void OnGet(string id, int? x)
    {
        GameId = id;

        // Load configuration
        var conf = _configRepo.Load(id);
        GameBrain = new GameBrain(conf, conf.Player1Name, conf.Player2Name);

        // TODO: restore saved board from database if you're storing it

        // If user clicked column
        if (x.HasValue)
        {
            // determine where the piece will fall
            int placedRow = -1;
            for (int row = GameBrain.GameConfiguration.BoardHeight - 1; row >= 0; row--)
            {
                if (GameBrain.GetBoard()[x.Value, row] == EBoardState.Empty)
                {
                    placedRow = row;
                    break;
                }
            }

            if (placedRow >= 0)
            {
                // Execute move
                GameBrain.ProcessMove(x.Value);

                // Determine winner
                var result = GameBrain.GetWinner(x.Value, placedRow);
                if (result == EBoardState.XWin)
                    Winner = conf.Player1Name + " (X)";
                if (result == EBoardState.OWin)
                    Winner = conf.Player2Name + " (O)";
            }

            // TODO: save updated board state
        }
    }
}