using System.Text.Json;
using BLL;

namespace DAL;

// cannot be static because of the interface
public class ConfigRepository: IRepository<GameConfiguration>
{
    public List<string> List()
    {
        var dir = FileSystemHelpers.GetConfigDirectory();
        var result = new List<string>();
        foreach (var fullFileName in Directory.EnumerateFiles(dir))
        {
            var filename =  Path.GetFileName(fullFileName);
            if (!filename.EndsWith(".json")) continue;
            result.Add(Path.GetFileNameWithoutExtension(filename));
        }
        return result;
    }

    // TODO: what if we just need to update already existing config (with filename change)
    public string Save(GameConfiguration data)
    {
        // data -> json
        var jsonStr = JsonSerializer.Serialize(data);
        // save the data
        // filename
        // TODO: sanitize data.Name, its unsafe to use it directly
        var filename = $"{data.Name} - {data.BoardWidth}x{data.BoardHeight} - win: {data.WinCondition} " + ".json";
        // file location
        var fullFileName = FileSystemHelpers.GetConfigDirectory() + Path.DirectorySeparatorChar + filename;
        // save file
        File.WriteAllText(fullFileName, jsonStr);
        return filename;
    }

    public GameConfiguration Load(string id)
    {
        var jsonFilename = FileSystemHelpers.GetConfigDirectory() + Path.DirectorySeparatorChar + id + ".json";
        var jsonText =  File.ReadAllText(jsonFilename);
        var conf = JsonSerializer.Deserialize<GameConfiguration>(jsonText);
        
        return conf ?? throw new NullReferenceException("Json deserialization returned null. Data:" + jsonText);
    }

    public void Delete(string id)
    {
        var jsonFilename = FileSystemHelpers.GetConfigDirectory() + Path.DirectorySeparatorChar + id + ".json";
        if (File.Exists(jsonFilename))
        {
            File.Delete(jsonFilename);
        }
    }
}