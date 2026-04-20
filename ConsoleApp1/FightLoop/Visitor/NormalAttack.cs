namespace ConsoleApp1;

public class NormalAttack : IAttackVisitor
{
    public int CalculateDamage(HeavyWeapon weapon, HeroStats stats)
        => stats.Strength + stats.Agressiveness + weapon.GetBonusDamage();

    public int CalculateDamage(OneHandedWeapon weapon, HeroStats stats)
        => stats.Agility + stats.Luck + weapon.GetBonusDamage();

    public int CalculateDamage(MagicalWeapon weapon, HeroStats stats)
        => stats.Wisdom + weapon.GetBonusDamage();
    
    public int CalculateDefaultDamage(HeroStats stats)
        => 0;

    public int CalculateDefense(HeavyWeapon weapon, HeroStats stats)
        => stats.Strength + stats.Luck;

    public int CalculateDefense(OneHandedWeapon weapon, HeroStats stats)
        => stats.Agility + stats.Luck;

    public int CalculateDefense(MagicalWeapon weapon, HeroStats stats)
        => stats.Agility + stats.Luck;

    public int CalculateDefaultDefence(HeroStats stats)
        => stats.Agility;
}