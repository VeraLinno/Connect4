using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
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
    public string CurrentPlayer { get; set; } = default!;


    public EBoardState Winner { get; set; } = EBoardState.Empty;
    public bool GameOver => Winner == EBoardState.XWin || Winner == EBoardState.OWin;

    public IActionResult OnGet(string id, int? x)
    {
        if (string.IsNullOrWhiteSpace(id))
            return RedirectToPage("Index");

        GameId = id;
        var conf = _configRepo.Load(id);

        if (conf == null)
            return RedirectToPage("Index");

        GameBrain = new GameBrain(
            conf,
            conf.Player1Name,
            conf.Player2Name
        );

        GameBrain.SetBoardFromList(conf.BoardState);
        Winner = CheckWinner(conf);
        
        if (x.HasValue && Winner == EBoardState.Empty)
        {
            GameBrain.ProcessMove(x.Value);

            Winner = CheckWinner(conf);
            
            if (Winner == EBoardState.Empty && conf.IsVsAi && !GameBrain.IsNextPlayerX())
            {
                GameBrain.MakeAiMove();

                Winner = CheckWinner(conf);
            }

            conf.BoardState = GameBrain.GetBoardAsList();
            var newId = _configRepo.Save(conf);
            conf.FileName = newId;
            GameId = newId;

        }

        CurrentPlayer = Winner != EBoardState.Empty
            ? string.Empty
            : GameBrain.IsNextPlayerX()
                ? $"{conf.Player1Name} (X) turn"
                : $"{conf.Player2Name} (O) turn";

        return Page();
    }

    private EBoardState CheckWinner(GameConfiguration conf)
    {
        for (int xx = 0; xx < conf.BoardWidth; xx++)
        {
            for (int yy = 0; yy < conf.BoardHeight; yy++)
            {
                var res = GameBrain.GetWinner(xx, yy);
                if (res == EBoardState.XWin || res == EBoardState.OWin)
                    return res;
            }
        }
        return EBoardState.Empty;
    }

}
