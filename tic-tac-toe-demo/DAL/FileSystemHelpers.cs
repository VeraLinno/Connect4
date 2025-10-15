namespace DAL;

public static class FileSystemHelpers
{
    private const string AppName = "TicTacToe";
    public static string GetConfigDirectory()
    {
        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var finalDir = homeDirectory + Path.DirectorySeparatorChar + 
                        AppName + Path.DirectorySeparatorChar + "configs";
        
        Directory.CreateDirectory(finalDir);
        
        return finalDir;
    }
    
    public static string GetGameDirectory()
    {
        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var finalDir = homeDirectory + Path.DirectorySeparatorChar + 
                       AppName + Path.DirectorySeparatorChar + "savegames";
        
        Directory.CreateDirectory(finalDir);
        
        return finalDir;
    }
}