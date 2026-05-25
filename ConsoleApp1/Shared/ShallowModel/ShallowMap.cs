namespace ConsoleApp1.Shared.ShallowModel;

public class ShallowMap
{
    public TyleType[][] TyleTypes { get; init; }
    public ShallowItem?[][] Map {get; set;}
    public ShallowEnemy?[][] Enemies {get; set;}
    public List<ShallowAnotherHero> Heroes {get; set;}
    
    public ShallowMap(ShallowItem?[][] map, TyleType[][] types, ShallowEnemy?[][] enemies, List<ShallowAnotherHero> heroes)
    {
        Map = map;
        Enemies = enemies;
        Heroes = heroes;
        TyleTypes = types;
    }
}

public enum TyleType
{
    Wall,
    Normal
}