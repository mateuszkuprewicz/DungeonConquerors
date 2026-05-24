namespace ConsoleApp1.Dungeon_Themes;
using ConsoleApp1.Items.Weapon;

public class TaxOfficeTheme : IDungeonTheme
{
    public List<Item> Items { get; }
    public int ItemCount { get; }
    public Item Artifact { get; }
    public List<string> EnemyNames { get; }
    public int EnemyCount { get; }
    public DungeonTypes DungeonType { get; }
    public string Message { get; }

    public TaxOfficeTheme()
    {
        Items = initItems();
        ItemCount = 15;
        Artifact = initArtifact();
        EnemyNames = initEnemies();
        EnemyCount = 9;
        DungeonType = DungeonTypes.CorridorDungeon;
        Message = "Welcome to the Tax Office. Take a number and prepare yourself...";
    }

    private List<Item> initItems()
    {
        var item1 = new UselessItem("PIT-37");

        var item2 = new WisdomBoostDecorator(
            new OneHandedWeapon("Pen of Precise Calculations"));

        var item3 = new StrengthBoostDecorator(
            new HeavyWeapon("Stamp of Final Decision"));

        var item4 = new LuckBoostDecorator(
            new MagicalWeapon("Queue Skipper"));
        
        var item5 = new HealthBoostDecorator(
            new HeavyWeapon("Binder of Eternal Documents"));
        

        return new List<Item>()
        {
            item1, item2, item3, item4, item5
        };
    }

    private Item initArtifact()
    {
        return new WisdomBoostDecorator(
            new LuckBoostDecorator(
                new HealthBoostDecorator(
                    new MagicalWeapon("Bribe"))));
    }

    private List<string> initEnemies()
    {
        return new List<string>()
        {
            "Clerk",
            "Tax Inspector",
            "Queue Keeper",
            "Accountant"
        };
    }
}