namespace ConsoleApp1.Shared.ShallowModel;

public class ShallowMap
{
    public Item?[,] Map {get; set;}
    public Enemy?[,] Enemies {get; set;}
    public List<ShallowAnotherHero> Heroes {get; set;}
    
    public ShallowMap(Item?[,] map, Enemy?[,] enemies, List<ShallowAnotherHero> heroes)
    {
        Map = map;
        Enemies = enemies;
        Heroes = heroes;
    }
}