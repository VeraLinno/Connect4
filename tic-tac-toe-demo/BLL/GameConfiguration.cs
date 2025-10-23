namespace BLL;

public class GameConfiguration
{
    public string? Name { get; set; } = "Cylindrical 5x5";
    public int BoardWidth { get; set; } = 5;
    public int BoardHeight { get; set; } = 5;
    public int WinCondition { get; set; } = 4;
}