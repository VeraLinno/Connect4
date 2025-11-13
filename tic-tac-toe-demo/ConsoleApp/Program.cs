using BLL;
using ConsoleApp;
using DAL;
using MenuSystem;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, CONNECT4!");
IRepository<GameConfiguration> configRepo;

// Choose ONE!

//configRepo = new ConfigRepositoryJSON();

using var dbContext = GetDbContext();
configRepo = new ConfigRepositoryEF(dbContext);


var menu0 = new Menu("Connect4 Main Menu", EMenuLevel.Root);
menu0.AddMenuItem("n", "New game", () =>
{
    var controller = new GameController(configRepo);
    controller.GameLoop();
    return "abc";
});

var menuConfig = new Menu("Connect4 Configurations", EMenuLevel.First);


menuConfig.AddMenuItem("l", "Load", () =>
{
    var loadedConfig = ConfigMenu.Load(configRepo);
    if (loadedConfig != null)
    {
        var controller = new GameController(configRepo, loadedConfig);
        controller.GameLoop();
    }
    else
    {
        Console.WriteLine("No configuration loaded.");
    }

    return "abc";
});


menuConfig.AddMenuItem("e", "Edit", () =>
{
    ConfigMenu.Edit(configRepo);
    return "abc";
});

menuConfig.AddMenuItem("d", "Delete", () =>
{
    ConfigMenu.Delete(configRepo);
    return "abc";
});

menu0.AddMenuItem("c", "Game Configuration", menuConfig.Run);

menu0.Run();
Console.WriteLine("Exit......");



AppDbContext GetDbContext()
{
    // ========================= DB STUFF ========================
    var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    homeDirectory = homeDirectory + Path.DirectorySeparatorChar;

// We are using SQLite
    var connectionString = $"Data Source={homeDirectory}tictactoe.db";

    var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(connectionString)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        //.LogTo(Console.WriteLine)
        .Options;

    var dbContext = new AppDbContext(contextOptions);
    
    // apply any pending migrations (recreates db as needed)
    dbContext.Database.Migrate();
    
    
    return dbContext;
}
