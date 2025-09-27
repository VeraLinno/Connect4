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
        if (MenuCommands.ReservedKeys.Contains(key))
        {
            throw new ArgumentException($"Key {key} is reserved and cannot be used");
        }
        
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
                MenuItems[MenuCommands.BackKey] = new MenuItem() { Key = MenuCommands.ExitKey, Value = MenuCommands.ExitValue };
                break;
            case EMenuLevel.First:
                MenuItems[MenuCommands.MainKey] = new MenuItem() { Key = MenuCommands.MainKey, Value = MenuCommands.MainValue };
                MenuItems[MenuCommands.BackKey] = new MenuItem() { Key = MenuCommands.ExitKey, Value = MenuCommands.ExitValue };
                break;
            case EMenuLevel.Deep:
                MenuItems[MenuCommands.BackKey] = new MenuItem() { Key = MenuCommands.BackKey, Value = MenuCommands.BackValue };
                MenuItems[MenuCommands.MainKey] = new MenuItem() { Key = MenuCommands.MainKey, Value = MenuCommands.MainValue };
                MenuItems[MenuCommands.BackKey] = new MenuItem() { Key = MenuCommands.ExitKey, Value = MenuCommands.ExitValue };
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
            if (MenuCommands.ReservedKeys.Contains(userChoice))
            {
                // TODO: Handle exit, return to main menu, or back
                menuRunning = false;
            }
            else
            {
                if (MenuItems.ContainsKey(userChoice))
                {
                    var returnValueFromMethodToRun = MenuItems[userChoice].MethodToRun?.Invoke();
                    if (returnValueFromMethodToRun == MenuCommands.ExitKey)
                    {
                        menuRunning = false;
                        userChoice = MenuCommands.ExitKey;
                    }
                    else if (returnValueFromMethodToRun == MenuCommands.MainKey && Level != EMenuLevel.Root)
                    {
                        menuRunning = false;
                        userChoice = MenuCommands.MainKey;
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