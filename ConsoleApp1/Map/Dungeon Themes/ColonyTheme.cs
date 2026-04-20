namespace ConsoleApp1.Dungeon_Themes;

public class ColonyTheme : IDungeonTheme
{
    public List<Item> Items { get; }
    public int ItemCount { get; }
    public Item Artifact { get; }
    public List<string> EnemyNames { get;}
    public int EnemyCount { get; }
    public DungeonTypes DungeonType { get;}
    public string Message { get; }

    public ColonyTheme()
    {
        Items = initItems();
        ItemCount = 15;
        Artifact = initArtifact();
        EnemyNames = initEnemies();
        EnemyCount = 8;
        DungeonType = DungeonTypes.ChamberDungeon;
        Message = "Welcome to the Colony!";
    }

    private List<Item> initItems()
    {
        var item1 = new UselessItem("Mushroom");
        var item2 = new LuckBoostDecorator(new OneHandedWeapon("Torch"));
        var item3 = new StrengthBoostDecorator(new HeavyWeapon("Paladin Sword", 5));
        var item4 = new WisdomBoostDecorator(new MagicalWeapon("Inonos' Staff of Brightness", 2));
        var item5 = new AggressivenessBoostDecorator(new MagicalWeapon("Beliar's Staff of Wrath", 3));
        return new List<Item>() { item1, item2, item3, item4, item5 };
    }

    private Item initArtifact()
    {
        return new LuckBoostDecorator(
            new StrengthBoostDecorator(new AgilityBoostDecorator(new HeavyWeapon("Xardas' Sword", 20))));
    }

    private List<string> initEnemies()
    {
        return new List<string>() { "Bloodfly", "Scavenger", "Orc" };
    }
}