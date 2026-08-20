namespace ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;


public class DeltaUpdateMessage
{
    public List<MapDelta> Deltas { get; set; } = new(); 
    
    public List<(int Id, ShallowHero? Hero)> UpdatedHeroes { get; set; } = new(); 
}

public class MapDelta
{
    public int X { get; set; }
    public int Y { get; set; }
    public ShallowItem? Item { get; set; }
    public ShallowEnemy? Enemy { get; set; }
}

public class NewPlayer
{
    public int Id{get;set;}
    public int X{get;set;}
    public int Y{get;set;}
}

public class LogMessege
{
    public string Text { get; set; }
}