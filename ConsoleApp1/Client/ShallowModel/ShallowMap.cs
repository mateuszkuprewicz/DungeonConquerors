using System.Text.Json.Serialization;

namespace ConsoleApp1.Shared.ShallowModel;

public class ShallowMap
{
    public int PlayerId { get; set; }
    public TyleType[][] TyleTypes { get; init; }
    public ShallowItem?[][] Map {get; set;}
    public ShallowEnemy?[][] Enemies {get; set;}
    public List<ShallowHero> Heroes {get; set;}
    
    
    public ShallowMap(ShallowItem?[][] map, TyleType[][] types, ShallowEnemy?[][] enemies, List<ShallowHero> heroes)
    {
        Map = map;
        Enemies = enemies;
        Heroes = heroes;
        TyleTypes = types;
    } 
    
    [JsonConstructor] 
    public ShallowMap() { }
}

public enum TyleType
{
    Wall,
    Normal
}