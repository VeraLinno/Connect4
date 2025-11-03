using BLL;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DAL;

public class ConfigRepositoryEF : IRepository<GameConfiguration>
{
    private readonly AppDbContext _dbContext;

    public ConfigRepositoryEF(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<(string id, string description)> List()
    {
        var dbConfigs = _dbContext.GameConfigurations.AsNoTracking().ToList();
        var res = new List<(string id, string description)>();

        foreach (var dbConf in dbConfigs)
        {
            res.Add((dbConf.Id.ToString(), dbConf.Name));
        }

        return res;
    }

    public string Save(GameConfiguration data)
    {
        var existing = _dbContext.GameConfigurations
            .FirstOrDefault(x => x.Id == data.Id);

        if (existing == null)
        {
            data.Id = Guid.NewGuid();
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");

            var baseName = string.IsNullOrWhiteSpace(data.Name)
                ? "unnamed"
                : SanitizeFileName(data.Name);
            
            data.Name = $"{baseName} {data.BoardWidth}x{data.BoardHeight} - win {data.WinCondition} - {timestamp}";
            _dbContext.GameConfigurations.Add(data);
        }
        else
        {
            existing.BoardWidth = data.BoardWidth;
            existing.BoardHeight = data.BoardHeight;
            existing.WinCondition = data.WinCondition;
            
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
            var baseName = string.IsNullOrWhiteSpace(data.Name)
                ? "unnamed"
                : SanitizeFileName(data.Name);

            existing.Name = $"{baseName} {data.BoardWidth}x{data.BoardHeight} - win {data.WinCondition} - {timestamp}";
        }

        _dbContext.SaveChanges();
        return data.Id.ToString();
    }

    public GameConfiguration Load(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new ArgumentException("Invalid configuration ID format", nameof(id));

        var config = _dbContext.GameConfigurations
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == guid);

        if (config == null)
            throw new KeyNotFoundException($"No configuration found with ID {id}");

        return config;
    }

    public void Delete(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new ArgumentException("Invalid configuration ID format", nameof(id));

        var config = _dbContext.GameConfigurations.FirstOrDefault(x => x.Id == guid);
        if (config == null)
            throw new KeyNotFoundException($"No configuration found with ID {id}");

        _dbContext.GameConfigurations.Remove(config);
        _dbContext.SaveChanges();
    }

    private static string SanitizeFileName(string name, int maxLength = 128)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
        sanitized = Regex.Replace(sanitized, "_{2,}", "_").Trim('_');

        if (sanitized.Length > maxLength)
            sanitized = sanitized.Substring(0, maxLength);

        return string.IsNullOrWhiteSpace(sanitized) ? "unnamed" : sanitized;
    }
}
