using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using BLL;

namespace DAL;

// cannot be static because of the interface
public class ConfigRepositoryJson: IRepository<GameConfiguration>
{
    public List<(string id, string description)> List()
    {
        var dir = FileSystemHelpers.GetConfigDirectory();
        var result = new List<(string id, string description, DateTime lastUpdate)>();
        
        foreach (var fullFileName in Directory.EnumerateFiles(dir))
        {
            var filename =  Path.GetFileName(fullFileName);
            if (!filename.EndsWith(".json")) continue;
            result.Add(
                (
                    Path.GetFileName(filename),
                    Path.GetFileNameWithoutExtension(filename),
                    File.GetLastWriteTime(fullFileName))
            );
        }
        var sorted = result
            .OrderByDescending(x => x.lastUpdate)
            .Select(x => (x.id, x.description))
            .ToList();

        return sorted;
    }
    
    public async Task<List<(string id, string description)>> ListAsync()
    {
        return List();
    }
    
    public string Save(GameConfiguration data)
    {
        var dir = FileSystemHelpers.GetConfigDirectory();

        // Delete old file if exists
        if (!string.IsNullOrWhiteSpace(data.FileName))
        {
            var oldPath = Path.Combine(dir, data.FileName);
            if (File.Exists(oldPath))
                File.Delete(oldPath);
        }

        // Create new filename
        var safeName = SanitizeFileName(data.Name ?? "Game");
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
        var filename = $"{safeName} - {data.BoardWidth}x{data.BoardHeight} - win {data.WinCondition} - {timestamp}.json";
        var fullPath = Path.Combine(dir, filename);

        // Serialize
        var jsonStr = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fullPath, jsonStr);

        // Save filename into config object for next time
        data.FileName = filename;

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


    public GameConfiguration? Load(string id)
    {
        var dir = FileSystemHelpers.GetConfigDirectory();
        var filename = id.EndsWith(".json") ? id : id + ".json";
        var fullPath = Path.Combine(dir, filename);

        if (!File.Exists(fullPath))
            return null;

        var jsonText = File.ReadAllText(fullPath);
        var conf = JsonSerializer.Deserialize<GameConfiguration>(jsonText);

        if (conf != null)
            conf.FileName = filename;   // ← SUPER OLULINE

        return conf;
    }



    public void Delete(string id)
    {
        var fileName = id.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ? id : id + ".json";

        var jsonFilename = Path.Combine(FileSystemHelpers.GetConfigDirectory(), fileName);
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