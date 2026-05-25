namespace ConsoleApp1.Dungeon_Themes;
using ConsoleApp1.Items.Weapon;

public class FrozenLand : IDungeonTheme
{
    public List<Item> Items { get; }
    public int ItemCount { get; }
    public Item Artifact { get; }
    public List<string> EnemyNames { get; }
    public int EnemyCount { get; }
    public DungeonTypes DungeonType { get; }
    public string Message { get; }

    public FrozenLand()
    {
        Items = initItems();
        ItemCount = 15;
        Artifact = initArtifact();
        EnemyNames = initEnemies();
        EnemyCount = 10;
        DungeonType = DungeonTypes.BasicDungeon;
        Message = "You feel the freezing wind... Welcome to the Frozen Land!";
    }

    private List<Item> initItems()
    {
        var item1 = new UselessItem("Frozen Bone");
        
        var item2 = new AgilityBoostDecorator(
            new OneHandedWeapon("Ice Dagger"));

        var item3 = new StrengthBoostDecorator(
            new HeavyWeapon("Glacier Hammer"));

        var item4 = new WisdomBoostDecorator(
            new MagicalWeapon("Staff of Eternal Winter"));

        var item5 = new HealthBoostDecorator(
            new OneHandedWeapon("Frost Guard Blade"));

        var item6 = new LuckBoostDecorator(
            new AgilityBoostDecorator(
                new OneHandedWeapon("Snow Whisper")));

        var item7 = new StrengthWeakenDecorator(
            new LuckBoostDecorator(
                new MagicalWeapon("Cursed Icicle")));

        return new List<Item>()
        {
            item1, item2, item3, item4, item5, item6, item7
        };
    }

    private Item initArtifact()
    {
        return new HealthBoostDecorator(
            new StrengthBoostDecorator(
                new WisdomBoostDecorator(
                    new HeavyWeapon("FingerTip of the Ice Titan"))));
    }

    private List<string> initEnemies()
    {
        return new List<string>()
        {
            "Frost Wolf",
            "Ice Golem",
            "Frozen Wraith",
            "Snow Stalker",
            "Blizzard Spirit",
            "Glacier Troll",
            "Ice Witch"
        };
    }
}