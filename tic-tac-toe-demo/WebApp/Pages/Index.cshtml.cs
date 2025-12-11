using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly IRepository<GameConfiguration> _repo;

    public IndexModel(IRepository<GameConfiguration> repo)
    {
        _repo = repo;
    }
    
    public class ConfigRow
    {
        public string Id { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Status { get; set; } = "Active";
    }

    public List<ConfigRow> Configurations { get; set; } = new();

    public void OnGet()
    {
        var list = _repo.List();  // returns (id, description)

        foreach (var (id, description) in list)
        {
            var conf = _repo.Load(id);
            string status = "Active";

            var brain = new GameBrain(conf, conf.Player1Name, conf.Player2Name);
            brain.SetBoardFromList(conf.BoardState);

            EBoardState winner = EBoardState.Empty;

            for (int x = 0; x < conf.BoardWidth; x++)
            {
                for (int y = 0; y < conf.BoardHeight; y++)
                {
                    var res = brain.GetWinner(x, y);
                    if (res == EBoardState.XWin || res == EBoardState.OWin)
                    {
                        winner = res;
                        break;
                    }
                }
                if (winner != EBoardState.Empty) break;
            }

            if (winner != EBoardState.Empty)
                status = "Completed";

            Configurations.Add(new ConfigRow
            {
                Id = id,
                Description = description,
                Status = status
            });
        }
    }

    public IActionResult OnPostDelete(string id)
    {
        _repo.Delete(id);
        return RedirectToPage();
    }
}