using ConsoleApp;
using MenuSystem;

Console.WriteLine("Hello, TIC-TAC-TOE!");

var menu0 = new Menu("Tic-Tac-Toe Main Menu", EMenuLevel.Root);
menu0.AddMenuItem("n", "New game", () =>
{
    var controller = new GameController();
    controller.GameLoop();
    return "abc";
});

menu0.Run();
Console.WriteLine("Exit......");
