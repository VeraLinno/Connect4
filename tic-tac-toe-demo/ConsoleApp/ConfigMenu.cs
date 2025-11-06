using BLL;
using DAL;

namespace ConsoleApp
{
    public static class ConfigMenu
    {
        public static GameConfiguration? Load(IRepository<GameConfiguration> configRepo)
        {
            var data = configRepo.List();
            if (data.Count == 0)
            {
                Console.WriteLine("No saved configurations found.");
                return null;
            }

            for (int i = 0; i < data.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {data[i].description}");
            }

            Console.Write("Select config to load, 0 to cancel: ");
            var userChoice = Console.ReadLine();

            if (int.TryParse(userChoice, out var choice))
            {
                if (choice > 0 && choice <= data.Count)
                {
                    var selected = data[choice - 1];
                    var loadedConfig = configRepo.Load(selected.id);

                    Console.WriteLine($"Loaded: {loadedConfig.Name} ({loadedConfig.BoardWidth}x{loadedConfig.BoardHeight})");

                    return loadedConfig;
                }
            }

            Console.WriteLine("Cancelled or invalid choice.");
            return null;
        }


        public static void Edit(IRepository<GameConfiguration> configRepo)
        {
            var configs = configRepo.List();
            if (configs.Count == 0)
            {
                Console.WriteLine("No configurations to edit found.");
            }
            
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
                        configRepo.Delete(selected.id);
                        gameConfig.Name = name.Trim();
                        var newFile = configRepo.Save(gameConfig);
                        Console.WriteLine($"Configuration name updated. New file: {newFile}");
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

        public static void Delete(IRepository<GameConfiguration> configRepo)
        {
            var configs = configRepo.List();
            if (configs.Count == 0)
            {
                Console.WriteLine("No configurations to delete found.");
            }
            
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
