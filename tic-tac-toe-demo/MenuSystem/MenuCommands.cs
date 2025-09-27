namespace MenuSystem;

public static class MenuCommands
{
    public const string ExitKey = "x";
    public const string BackKey = "b";
    public const string MainKey = "m";

    public const string ExitValue = "Exit";
    public const string BackValue = "Back to previous menu";
    public const string MainValue = "Return to main menu";

    public static readonly HashSet<string> ReservedKeys = new()
    {
        ExitKey, BackKey, MainKey
    };
}