using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Test : PageModel
{
    [BindProperty]
    public string UserName { get; set; } = default!;
    public string FinalUserName { get; set; } = "none yet!";
    public bool? HasError { get; set; }
    
    public void OnGet()
    {
        UserName = "some name";
        Console.Write("OnGet...");
    }

    public void OnPost()
    {
        if (UserName.Length < 2)
        {
            HasError = true;
        }
        else
        {
            HasError = false;
            FinalUserName = UserName + " final";
        }
    }
}