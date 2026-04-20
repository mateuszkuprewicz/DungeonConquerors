using ConsoleApp1.FightLoop.Visitor.CalculateBonusDamageVisitor;
using ConsoleApp1.Items.Weapon;


namespace ConsoleApp1.FightLoop.Visitor.AttackTypesVisitor;

public class MagicAttack : IAttackVisitor
{
    private static readonly BonusDamageVisitor BonusDamageVisitor = new();
    public int CalculateDamage(HeavyWeapon weapon, HeroStats stats)
        => 1 + weapon.AcceptCalculateBonusDamage(BonusDamageVisitor);

    public int CalculateDamage(OneHandedWeapon weapon, HeroStats stats)
        => 1 + weapon.AcceptCalculateBonusDamage(BonusDamageVisitor);

    public int CalculateDamage(MagicalWeapon weapon, HeroStats stats)
        => stats.Wisdom + weapon.AcceptCalculateBonusDamage(BonusDamageVisitor);

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