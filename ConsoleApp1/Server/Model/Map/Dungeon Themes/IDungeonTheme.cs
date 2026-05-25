namespace ConsoleApp1.Dungeon_Themes;

public interface IDungeonTheme
{
    public List<Item> Items { get; }
    public int ItemCount { get; }
    public Item Artifact { get; }
    public List<string> EnemyNames { get; }
    public int EnemyCount { get; }
    public DungeonTypes DungeonType { get; }
    public string Message { get; }
}