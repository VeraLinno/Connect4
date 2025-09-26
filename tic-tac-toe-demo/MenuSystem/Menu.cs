using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MenuSystem;
public class Menu
{
    //exit, back1, return to me
    private string Title { get; set; } = default!;
    public Dictionary<string, MenuItem> MenuItems { get; set; } = new();
    private EMenuLevel Level { get; set; }

    public void AddMenuItem(string key, string value, Func<string> methodToRun)
    {
        if (MenuItems.ContainsKey(key))
        {
            throw new ArgumentException($"Key {key} already exists");
        }

        MenuItems[key] = new MenuItem() { Key = key, Value = value, MethodToRun = methodToRun };
    }

    public Menu(string title, EMenuLevel level)
    {
        Title = title;
        Level = level;

        switch (level)
        {
            case EMenuLevel.Root:
                MenuItems["x"] = new MenuItem() { Key = "x", Value = "exit" };
                break;
            case EMenuLevel.First:
                MenuItems["m"] = new MenuItem() { Key = "m", Value = "return to main" };
                MenuItems["x"] = new MenuItem() { Key = "x", Value = "exit" };
                break;
            case EMenuLevel.Deep:
                MenuItems["b"] = new MenuItem() { Key = "b", Value = "back to previous Menu" };
                MenuItems["m"] = new MenuItem() { Key = "m", Value = "return to main" };
                MenuItems["x"] = new MenuItem() { Key = "x", Value = "exit" };
                break;
        }
    }

    public string Run()
    {
        Console.Clear();
        var menuRunning = true;
        var userChoice = "";
        do
        {
            DisplayMenu();
            Console.Write("Please enter your choice: ");
            var userInput = Console.ReadLine();
            if (userInput == null)
            {
                Console.WriteLine("Invalid choice.. Please try again.");
                continue;
            }

            userChoice = userInput.Trim().ToLower();
            if (userChoice == "x" || userChoice == "m"  || userChoice == "b")
            {
                // TODO: Handle exit, return to main menu, or back
                menuRunning = false;
            }
            else
            {
                if (MenuItems.ContainsKey(userInput))
                {
                    var returnValueFromMethodToRun = MenuItems[userChoice].MethodToRun?.Invoke();
                    if (returnValueFromMethodToRun == "x")
                    {
                        menuRunning = false;
                        userChoice = "x";
                    }
                    else if (returnValueFromMethodToRun == "m" && Level != EMenuLevel.Root)
                    {
                        menuRunning = false;
                        userChoice = "m";
                    }
                }
                else 
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
            Console.WriteLine();
        } while (menuRunning);

        return userChoice; // return the user choice to the caller
    }

    public void DisplayMenu()
    {
        Console.WriteLine(Title);
        Console.WriteLine("-----------------------");
        foreach (var item in MenuItems.Values)
        {
            Console.WriteLine(item);
        }
    }
}