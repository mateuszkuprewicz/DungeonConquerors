using ConsoleApp1.Logger;
using ConsoleApp1.MovingAI;
using ConsoleApp1.SoundPropagation;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1;

public class Enemy : IMovingEnemy, ISoundHearer
{
    public AbstractMovingState MovingState { private get; set; }
    public int Id;
    public string Name { get; }
    public char Symbol { get; private set; }
    public int Hp { get; private set; }
    public int Damage { get; private set; }
    private int Defense { get; set; }
    public (int X, int Y) Position { get; set; }
    private Enemy?[,] Enemies { get; set; }
    private GameMap Map { get; set; }

    private ISoundSubscribtion sub;

    public Enemy(int hp, int dmg, int defence, string name, Enemy?[,] enemies, (int x, int y) position, GameMap map, ISoundSubscribtion subscribtion, char symbol = 'E')
    {
        (Hp, Damage, Defense, Name, Symbol, Enemies, Position) = (hp, dmg, defence, name, symbol, enemies, position);
        MovingState = new RandomMoving(this, map);
        Map = map;
        sub = subscribtion;
        sub.Subscribe(this);
    }
    
    public (int X, int Y) Move()
    {
        (int,int) curPos = this.Position;
        (int, int) nextPos = MovingState.GetNextMove();
        
        Enemies[curPos.Item2, curPos.Item1] = null; 
        Position = (nextPos.Item1, nextPos.Item2); 
        Enemies[nextPos.Item2, nextPos.Item1] = this;

        return Position;
    }

    public string Hear(NoiseEvent sound)
    {
        if (!sound.HasReached(this.Position)) return null;
        
        Queue<(int, int)> path = sound.GetPathToSource(this.Position);
        MovingState = new TargetedMoving(this, Map, path);
        
        return $"Enemy {Name} heard a sound coming from {sound.Source}!";
    }
    
    public void ReceiveDamage(int damage)
    {
        int d = damage -  Defense;
        if (d > 0) 
            Hp -= d;
    }

    public void Die()
    {
        Enemies[Position.Y, Position.X] = null;
        sub.Unsubscribe(this);
    }
}