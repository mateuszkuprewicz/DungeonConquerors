namespace ConsoleApp1;

public class Enemy
{
    public string Name { get; }
    public char Symbol { get; private set; }
    public int Hp { get; private set; }
    public int Damage { get; private set; }
    private int Defense { get; set; }
    private (int X, int Y) Position { get; set; }
    private Enemy?[,] Enemies { get; set; }
    
    public Enemy(int hp, int dmg, int defence, string name, Enemy?[,] enemies, (int x, int y) position, char symbol = 'E') 
        => (Hp, Damage, Defense, Name, Symbol, Enemies, Position) = (hp, dmg, defence, name, symbol, enemies, position);

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