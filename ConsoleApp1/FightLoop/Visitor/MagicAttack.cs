namespace ConsoleApp1;

public class MagicAttack : IAttackVisitor
{
    public int CalculateDamage(HeavyWeapon weapon, HeroStats stats)
        => 1 + weapon.GetBonusDamage();

    public int CalculateDamage(OneHandedWeapon weapon, HeroStats stats)
        => 1 + weapon.GetBonusDamage();

    public int CalculateDamage(MagicalWeapon weapon, HeroStats stats)
        => stats.Wisdom + weapon.GetBonusDamage();

    public int CalculateDefaultDamage(HeroStats stats)
        => 0;

    public int CalculateDefense(HeavyWeapon weapon, HeroStats stats)
        => stats.Luck;

    public int CalculateDefense(OneHandedWeapon weapon, HeroStats stats)
        => stats.Luck;

    public int CalculateDefense(MagicalWeapon weapon, HeroStats stats)
        => stats.Wisdom * 2;

    public int CalculateDefaultDefence(HeroStats stats)
        => stats.Luck ;
}