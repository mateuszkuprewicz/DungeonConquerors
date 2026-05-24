namespace ConsoleApp1.View;

public static class RenderConsts
{
    const int MapHeight = 20;
    const int MapWidth = 40;
    const int Tab = 15;

    private static readonly (int, int) StatsTableStart = (43, 0);
    private static readonly (int, int) EquipmentTableStart = (43, 5);
    private static readonly (int, int) HandsTableStart = (43 + Tab, 6);
    private static readonly (int, int) Info = (43, 20);
    public static readonly (int, int) DefaultCursorPosition = (0, 26);
    public static readonly (int, int) Instruction = (0, 21);
}