using ConsoleApp1.FightLoop.Visitor.CalculateBonusDamageVisitor;

namespace ConsoleApp1.FightLoop.Visitor.AttackTypesVisitor;

public class NormalAttack : IAttackVisitor
{
    public static readonly BonusDamageVisitor BonusDamageVisitor = new();
    public int CalculateDamage(HeavyWeapon weapon, HeroStats stats)
        => stats.Strength + stats.Agressiveness + weapon.AcceptCalculateBonusDamage(BonusDamageVisitor);

    public int CalculateDamage(OneHandedWeapon weapon, HeroStats stats)
        => stats.Agility + stats.Luck + weapon.AcceptCalculateBonusDamage(BonusDamageVisitor);

    public int CalculateDamage(MagicalWeapon weapon, HeroStats stats)
        => stats.Wisdom + weapon.AcceptCalculateBonusDamage(BonusDamageVisitor);
    
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