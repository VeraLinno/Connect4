
using BLL;
using DAL;

namespace ConsoleApp
{
    public static class ConfigMenu
    {
        public static void Load(IRepository<GameConfiguration> configRepo)
        {
            var data = configRepo.List();
            for (int i = 0; i < data.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {data[i].description}");
            }

            Console.Write("Select config to load, 0 to skip: ");
            var userChoice = Console.ReadLine();
        }

        public static void Edit(IRepository<GameConfiguration> configRepo)
        {
            var configs = configRepo.List();
            for (var i = 0; i < configs.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {configs[i].description}");
            }

            Console.Write("Select config to edit, 0 to cancel: ");
            var userChoice = Console.ReadLine();

            if (int.TryParse(userChoice, out var choice))
            {
                if (choice > 0 && choice <= configs.Count)
                {
                    var selected = configs[choice - 1];
                    var gameConfig = configRepo.Load(selected.id);

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
        }

        public static void Create(IRepository<GameConfiguration> configRepo)
        {
            Console.Write("Write a name for the game, 0 to cancel: ");
            var userChoice = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(userChoice))
            {
                configRepo.Save(new GameConfiguration() { Name = userChoice });
            }
        }

        public static void Delete(IRepository<GameConfiguration> configRepo)
        {
            var configs = configRepo.List();
            for (var i = 0; i < configs.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {configs[i].description}");
            }

            Console.Write("Select config to delete, 0 to cancel: ");
            var userChoice = Console.ReadLine();

            if (int.TryParse(userChoice, out var choice))
            {
                if (choice > 0 && choice <= configs.Count)
                {
                    var selected = configs[choice - 1];
                    Console.Write($"Are you sure you want to delete '{selected.description}'? (y/n): ");
                    var confirm = Console.ReadLine();
                    if (confirm?.ToLower() == "y")
                    {
                        configRepo.Delete(selected.id);
                        Console.WriteLine($"Deleted configuration: {selected.description}");
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
        }
    }
}
