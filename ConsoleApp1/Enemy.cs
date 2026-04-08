namespace ConsoleApp1;

public class Enemy
{
    private string Name { get; set; }
    public char Symbol { get; private set; }
    public int Hp { get; private set; }
    public int Damage { get; private set; }
    private int Defense { get; set; }
    
    public Enemy(int hp, int dmg, int defence, string name, char symbol = 'E') 
        => (Hp, Damage, Defense, Name, Symbol) = (hp, dmg, defence, name,  symbol);

    public void ReceiveDamage(int damage)
    {
        int d = damage -  Defense;
        if (d > 0) 
            Hp -= d;
    }
    
}