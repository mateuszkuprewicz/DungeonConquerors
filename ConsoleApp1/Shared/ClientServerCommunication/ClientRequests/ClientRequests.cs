namespace ConsoleApp1.DTO.ClientRequests;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class ClientMove
{
    public int Id;
    public Direction Direction { get; set; }
}

public class ClientPickUp{}

public class ClientDrop
{
    public int ItemNumber{get; set;}
}

public class ClientEquip
{
    public int ItemNumber{get; set;}
}

public class ClientUnequip {}

public class ClientHit
{
    public HitType Type{get; set;}
}

public class ClientRunAway(){}

public enum HitType
{
    HeavyAttack,
    SneakyAttack,
    MagicAttack
}