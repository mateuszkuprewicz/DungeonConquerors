using ConsoleApp1.MovingAI;

namespace ConsoleApp1;

public class Enemy : IMovingEnemy
{
    public AbstractMovingState MovingState { private get; set; }
    public string Name { get; }
    public char Symbol { get; private set; }
    public int Hp { get; private set; }
    public int Damage { get; private set; }
    private int Defense { get; set; }
    public (int X, int Y) Position { get; set; }
    private Enemy?[,] Enemies { get; set; }

    public Enemy(int hp, int dmg, int defence, string name, Enemy?[,] enemies, (int x, int y) position, GameMap map,
        char symbol = 'E')
    {
        (Hp, Damage, Defense, Name, Symbol, Enemies, Position) = (hp, dmg, defence, name, symbol, enemies, position);
        MovingState = new RandomMoving(this, map);
    }
    
    public void Move()
    {
        (int,int) curPos = this.Position;
        (int, int) nextPos = MovingState.GetNextMove();
        if (Enemies[nextPos.Item2, nextPos.Item1] == null)
        {
            Enemies[curPos.Item2, curPos.Item1] = null;
            Position = (nextPos.Item1, nextPos.Item2);
            Enemies[nextPos.Item2, nextPos.Item1] = this;
        }
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
    }
}