using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class NewGameModel : PageModel
{
    private readonly IRepository<GameConfiguration> _repo;

    public NewGameModel(IRepository<GameConfiguration> repo)
    {
        _repo = repo;
    }

    [BindProperty] public string GameName { get; set; } = "Connect4 - Cylindrical";
    [BindProperty] public string Player1Name { get; set; } = "Player1";
    [BindProperty] public string Player2Name { get; set; } = "Player2";
    [BindProperty] public bool IsVsAi { get; set; }
    [BindProperty] public int BoardWidth { get; set; } = 5;
    [BindProperty] public int BoardHeight { get; set; } = 5;

    public IActionResult OnPost()
    {
        var conf = new GameConfiguration
        {
            Player1Name = Player1Name,
            Player2Name = Player2Name,
            IsVsAi = IsVsAi,
            BoardWidth = BoardWidth,
            BoardHeight = BoardHeight,
            Name = GameName
        };

        conf.BoardState = new List<List<EBoardState>>();
        for (int y = 0; y < BoardHeight; y++)
        {
            var row = new List<EBoardState>();
            for (int x = 0; x < BoardWidth; x++)
                row.Add(EBoardState.Empty);
            conf.BoardState.Add(row);
        }

        var id = _repo.Save(conf);

        return RedirectToPage("GamePlay", new { id });
    }
}