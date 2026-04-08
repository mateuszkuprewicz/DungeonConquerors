namespace ConsoleApp1;

public class StealthAttack : IAttackVisitor
{
    public int CalculateDamage(HeavyWeapon weapon, HeroStats stats)
        => (stats.Strength + stats.Agressiveness) / 2;

    public int CalculateDamage(OneHandedWeapon weapon, HeroStats stats)
        => (stats.Agility + stats.Luck) * 2;

    public int CalculateDamage(MagicalWeapon weapon, HeroStats stats)
        => 1;
    
    public int CalculateDefaultDamage(HeroStats stats)
        => 0;

    public int CalculateDefense(HeavyWeapon weapon, HeroStats stats)
        => stats.Strength;

    public int CalculateDefense(OneHandedWeapon weapon, HeroStats stats)
        => stats.Agility;

    public int CalculateDefense(MagicalWeapon weapon, HeroStats stats)
        => 0;

    public int CalculateDefaultDefence(HeroStats stats)
        => 0;
}