// See https://aka.ms/new-console-template for more information

using System.Reflection.Metadata;
using MenuSystem;

Console.WriteLine("Hello, TIC-TAC-TOE");

var menu0 = new Menu("Tic-Tac-Toe main menu", EMenuLevel.Root);
menu0.AddMenuItem("a", "New Game", () =>
{
    // TODO: GameStart();
    return "";
});

var menu1 = new Menu("Tic-Tac-Toe Level1 menu", EMenuLevel.First);
menu1.AddMenuItem("a", "Level1 - Option a, returns b", () =>
{
    Console.WriteLine("Level1 - Option a was called");
    return "b";
});

var menu2 = new Menu("Tic-Tac-Toe Level2 menu", EMenuLevel.Deep);
menu2.AddMenuItem("a", "Level2 - Option a, returns m", () =>
{
    Console.WriteLine("Level2 - Option a was called");
    return "m";
});

var menu3 = new Menu("Tic-Tac-Toe Level3 Menu", EMenuLevel.Deep);
menu3.AddMenuItem("a", "Level3 - Option A, returns z", () =>
{
    Console.WriteLine("Level3 - Option A was called");
    return "z";
});


menu0.AddMenuItem("1", "Level0 - Go to level1", menu1.Run);
menu1.AddMenuItem("2", "Level1 - Go to level2", menu2.Run);
menu2.AddMenuItem("3", "Level2 - Go to level3", menu3.Run);

menu0.Run();
Console.WriteLine("Exit......");
