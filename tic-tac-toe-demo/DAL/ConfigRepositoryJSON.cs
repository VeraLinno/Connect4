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
    
        // does file exist
        var existingFile = Directory.EnumerateFiles(dir, "*.json")
            .FirstOrDefault(file =>
            {
                try
                {
                    var json = File.ReadAllText(file);
                    var conf = JsonSerializer.Deserialize<GameConfiguration>(json);
                    return conf?.Id == data.Id;
                }
                catch
                {
                    return false;
                }
            });

        // data -> json
        var jsonStr = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        
        // new file
        var safeName = SanitizeFileName(data.Name);
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
        var baseFilename = $"{safeName} - {data.BoardWidth}x{data.BoardHeight} - win {data.WinCondition} - {timestamp}";
        var filename = baseFilename.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
            ? baseFilename
            : baseFilename + ".json";

        var fullFileName = Path.Combine(dir, filename);
        
        // if exist - delete old one
        if (existingFile != null && File.Exists(existingFile))
        {
            try
            {
                File.Delete(existingFile);
            }
            catch (IOException ex)
            {
                throw new IOException($"Failed to delete old config file {existingFile}", ex);
            }
        }
        
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
        var fileName = id.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ? id : id + ".json";
        var fullPath = Path.Combine(FileSystemHelpers.GetConfigDirectory(), fileName);

        var jsonText =  File.ReadAllText(fullPath);
        var conf = JsonSerializer.Deserialize<GameConfiguration>(jsonText);
        
        return conf ?? throw new NullReferenceException("Json deserialization returned null. Data:" + jsonText);
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