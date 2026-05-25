namespace ConsoleApp1.Shared.ShallowModel;

public class ShallowHero
{
    public int Id { get; set; }
    public char Name { get; set; }
    public Position Pos { get; set; }
    public ShallowEquipment Equipment { get; set; }
    public ShallowHeroHands Hands { get; set; }
    public ShallowHeroStats Stats { get; set; }
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
}