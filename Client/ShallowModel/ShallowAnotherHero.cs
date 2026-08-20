using System.Text.Json.Serialization;

namespace ConsoleApp1.Shared.ShallowModel;

public class ShallowAnotherHero
{
    public int ID { get; set; }
    public char Name { get; init; }
    public Position Pos { get; set; }

    public ShallowAnotherHero(int id, (int X, int Y) position)
    {
        ID = id;
        Name = id.ToString()[0];
        Pos = new Position(position.X, position.Y);
    }
    
    [JsonConstructor]
    public ShallowAnotherHero(){}
    
}