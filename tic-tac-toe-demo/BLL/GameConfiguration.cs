using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BLL;

public class GameConfiguration: BaseEntity
{
    public string? Name { get; set; } = "Cylindrical 5x5";
    public int BoardWidth { get; set; } = 5;
    public int BoardHeight { get; set; } = 5;
    public int WinCondition { get; set; } = 4;
    public bool IsVsAi { get; set; } = false;
    
    public string Player1Name { get; set; } = "Player1";
    public string Player2Name { get; set; } = "Player2";
    
    [NotMapped]
    public List<List<EBoardState>>? BoardState { get; set; }
    
    public string? BoardStateJson
    {
        get => BoardState == null ? null : JsonSerializer.Serialize(BoardState);
        set => BoardState = string.IsNullOrWhiteSpace(value) ? null : JsonSerializer.Deserialize<List<List<EBoardState>>>(value);
    }
}