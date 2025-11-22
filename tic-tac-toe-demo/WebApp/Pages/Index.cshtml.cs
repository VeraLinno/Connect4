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

    public List<(string id, string description)> Configurations { get; set; } = new();

    public void OnGet()
    {
        Configurations = _repo.List();
    }

    public IActionResult OnPostDelete(string id)
    {
        _repo.Delete(id);
        return RedirectToPage();
    }
}