using BLL;
using ConsoleApp;
using DAL;
using MenuSystem;

Console.WriteLine("Hello, TIC-TAC-TOE!");
var menu0 = new Menu("Tic-Tac-Toe Main Menu", EMenuLevel.Root);
menu0.AddMenuItem("n", "New game", () =>
{
    var controller = new GameController();
    controller.GameLoop();
    return "abc";
});

var menuConfig = new Menu("Tic-Tac-Toe Configuration", EMenuLevel.First);

var configRepo = new ConfigRepositoryJSON();

// TODO: we need to do the config logic (nagu we have gameBrain logic)
menuConfig.AddMenuItem("l", "Load", () =>
{
    var count = 0;
    var data = configRepo.List();
    foreach (var configName in data)
    {
        Console.WriteLine((count + 1) + ": " + configName);
        count++;
    }
    Console.Write("Select config to load, 0 to skip: ");
    var userChoice = Console.ReadLine();
    return "abc";
});

menuConfig.AddMenuItem("e", "Edit", () =>
{
    var configs = configRepo.List();
    for (var i = 0; i < configs.Count; i++)
    {
        Console.WriteLine($"{i + 1}: {configs[i]}");
    }

    Console.Write("Select config to edit, 0 to cancel: ");
    var userChoice = Console.ReadLine();
    
    if (int.TryParse(userChoice, out var choice))
    {
        if (choice > 0 && choice <= configs.Count)
        {
            var id = configs[choice - 1];
            var gameConfig = configRepo.Load(id);
            
            Console.Write("Write a new name for the game, leave empty to cancel: ");
            var name = Console.ReadLine();
            
            if (!string.IsNullOrWhiteSpace(name))
            {
                gameConfig.Name = name.Trim();
            }
            var newFileName = configRepo.Save(gameConfig);
            Console.WriteLine($"Configuration name updated. New file: {newFileName}");
        }
        else
        {
            Console.WriteLine("Cancelled or invalid choice.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input.");
    }

    Console.WriteLine();
    return "abc";
});

menuConfig.AddMenuItem("c", "Create", () =>
{
    Console.Write("Write a name for the game, 0 to cancel: ");
    var userChoice = Console.ReadLine();
    if (userChoice != String.Empty)
    {
        configRepo.Save(new GameConfiguration() { Name = userChoice });
    };
    return "abc"; 
});

menuConfig.AddMenuItem("d", "Delete", () =>
{
    var configs = configRepo.List();
    for (var i = 0; i < configs.Count; i++)
    {
        Console.WriteLine($"{i + 1}: {configs[i]}");
    }

    Console.Write("Select config to delete, 0 to cancel: ");
    var userChoice = Console.ReadLine();
    
    if (int.TryParse(userChoice, out var choice))
    {
        if (choice > 0 && choice <= configs.Count)
        {
            var id = configs[choice - 1];
            Console.Write($"Are you sure you want to delete '{id}'? (y/n): ");
            var confirm = Console.ReadLine();
            if (confirm?.ToLower() == "y")
            {
                configRepo.Delete(id);
                Console.WriteLine($"Deleted configuration: {id}");
            }
            else
            {
                Console.WriteLine("Cancelled.");
            }
        }
        else
        {
            Console.WriteLine("Cancelled or invalid choice.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input.");
    }

    Console.WriteLine();
    return "abc";
});

menu0.AddMenuItem("c", "Game Configuration", menuConfig.Run);

menu0.Run();
Console.WriteLine("Exit......");
