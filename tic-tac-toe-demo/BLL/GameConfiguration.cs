namespace BLL;

public class GameConfiguration
{
    public int BoardWidth { get; set; } = 5;
    public int BoardHeight { get; set; } = 5;
    public int WinCondition { get; set; } = 4;
    
    public EPlayerType P1Type { get; set; } = EPlayerType.Human;
    public EPlayerType P2Type { get; set; } = EPlayerType.Human;
}