using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using BLL;

namespace DAL;

// cannot be static because of the interface
public class ConfigRepositoryJSON: IRepository<GameConfiguration>
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
        
        // filename
        var safeName = SanitizeFileName(data.Name);
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
        var filename = $"{safeName} - {data.BoardWidth}x{data.BoardHeight} - win {data.WinCondition} - {timestamp}.json";
        
        // file location
        var fullFileName = FileSystemHelpers.GetConfigDirectory() + Path.DirectorySeparatorChar + filename;
        
        // save file
        File.WriteAllText(fullFileName, jsonStr);
        return filename;
    }

    private static string SanitizeFileName(string name, int maxLength = 128)
    {
        if (string.IsNullOrWhiteSpace(name)) return "unnamed";

        var invalid = Path.GetInvalidFileNameChars();
        var sb = new StringBuilder(name.Length);
        foreach (var ch in name)
        {
            sb.Append(invalid.Contains(ch) ? '_' : ch);
        }
        var sanitized = sb.ToString().Trim();

        if (sanitized.Length > maxLength)
            sanitized = sanitized.Substring(0, maxLength);
        
        return Regex.Replace(sanitized, "_{2,}", "_").Trim('_');
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
        try
        {
            if (File.Exists(jsonFilename))
            {
                File.Delete(jsonFilename);
            }
        }
        catch (IOException ex)
        {
            throw new IOException($"Failed to delete config file {jsonFilename}", ex);
        }
    }
}