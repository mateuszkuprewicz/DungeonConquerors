using System.Text.Json.Serialization;

namespace ConsoleApp1.Shared.ShallowModel;

public class ShallowHero
{
    public int Id { get; set; }
    public char Name { get; set; }
    public Position Pos { get; set; }
    public ShallowEquipment Equipment { get; set; }
    public ShallowHeroHands Hands { get; set; }
    public ShallowHeroStats Stats { get; set; }

    public ShallowHero(int Id, (int X, int Y) position)
    {
        this.Id = Id;
        Pos = new Position(position.X, position.Y);
        Name = Id.ToString()[0];
        Stats = new ShallowHeroStats();
        Equipment = new ShallowEquipment();
        Hands = new ShallowHeroHands();
    }
    
    [JsonConstructor]
    public ShallowHero(){}
}

public class ShallowEquipment
{
    public List<ShallowItem> EquipmentList { get; set; }
    public int EquipmentPointer = 0;
    public int Coins { get; set; }
    public int Gold { get; set; }
}

public class ShallowHeroHands
{
    public ShallowItem? LeftHand { get; set; }
    public ShallowItem? RightHand { get; set; }
}

public class ShallowHeroStats
{
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Luck { get; set; }
    public int Agressiveness { get; set; }
    public int Wisdom { get; set; }
    public int Health { get; set; }

    public ShallowHeroStats(int uselles = 0)
    {
        Strength = 5;
        Agility = 5;
        Luck = 5;
        Agressiveness = 5;
        Wisdom = 5;
        Health = 100;
    }
    
    [JsonConstructor]
    public ShallowHeroStats(){}
}