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

var configRepo = new ConfigRepository();

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
menuConfig.AddMenuItem("e", "Edit", () => " abc");
menuConfig.AddMenuItem("c", "Create", () =>
{
    configRepo.Save(new GameConfiguration(){Name = "Cylindrical"});
    return "abc";

});
menuConfig.AddMenuItem("d", "Delete", () => " abc");

menu0.AddMenuItem("c", "Game Configuration", menuConfig.Run);

menu0.Run();
Console.WriteLine("Exit......");
