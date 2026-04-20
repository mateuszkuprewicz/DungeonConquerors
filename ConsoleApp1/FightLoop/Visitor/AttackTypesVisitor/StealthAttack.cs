using ConsoleApp1.FightLoop.Visitor.CalculateBonusDamageVisitor;

namespace ConsoleApp1.FightLoop.Visitor.AttackTypesVisitor;

public class StealthAttack : IAttackVisitor
{
    public static readonly BonusDamageVisitor BonusDamageVisitor = new();
    public int CalculateDamage(HeavyWeapon weapon, HeroStats stats)
        => (stats.Strength + stats.Agressiveness) / 2 + weapon.AcceptCalculateBonusDamage(BonusDamageVisitor);

    public int CalculateDamage(OneHandedWeapon weapon, HeroStats stats)
        => (stats.Agility + stats.Luck) * 2 + weapon.AcceptCalculateBonusDamage(BonusDamageVisitor);

    public int CalculateDamage(MagicalWeapon weapon, HeroStats stats)
        => 1 + weapon.AcceptCalculateBonusDamage(BonusDamageVisitor);
    
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