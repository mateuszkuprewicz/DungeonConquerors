namespace ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

public class ClientMoved
{
    public int ClientId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public class ClientPickedUp
{
    public int ClientId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public ShallowItem? UnderLyingItem { get; set; }
    public ShallowEquipment? ClientEquipment { get; set; }
}

public class ClientDropped
{
    public int ClientId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public ShallowItem? Dropped { get; set; }
    public ShallowEquipment? ClientEquipment { get; set; }
}

public class ClientEquipped
{
    public int ClientId { get; set; }
    public ShallowEquipment? ClientEquipment { get; set; }
    public ShallowHeroHands? ClientHands { get; set; }
}

public class ClientUnequipped
{
    public int ClientId { get; set; }
    public ShallowEquipment? ClientEquipment { get; set; }
    public ShallowHeroHands? ClientHands { get; set; }
}

public class ClientHitBroadcast
{
    public int ClientId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int EnemyId{ get; set; }
    public int EnemyHp{ get; set; }
    public int ClientHp{ get; set; }
}

public class ClientRunAwayBroadcast
{
    public int ClientId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int ClientHp { get; set; }
}

public class ClientDied
{
    public int ClientId { get; set; }
}

public class NewClient
{
    public int Id{get;set;}
    public int X{get;set;}
    public int Y{get;set;}
}

public class SoundPropogation
{
    public List<int> Enemies;
    public List<int> Heros;
}

public class RequestRejected
{
    public required string Text { get; set; }
}