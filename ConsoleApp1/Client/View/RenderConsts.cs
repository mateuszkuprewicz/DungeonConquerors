namespace ConsoleApp1.Client.View;

public static class RenderConsts
{
    public const int MapHeight = 20;
    public const int MapWidth = 40;
    public const int Tab = 15;
    
    public static readonly (int, int) StatsTableStart = (43, 0);
    public static readonly (int, int) EquipmentTableStart = (43, 5);
    public static readonly (int, int) HandsTableStart = (43 + Tab, 6);
    public static readonly (int, int) Info = (43, 20);
    public static readonly (int, int) DefaultCursorPosition = (0, 26);
    public static readonly (int, int) Instruction = (0, 21);
    public static readonly (int, int) EnemyStatsStart = (43, 16);
}