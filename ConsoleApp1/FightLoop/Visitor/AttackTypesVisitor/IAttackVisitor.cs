namespace ConsoleApp1;

public interface IAttackVisitor
{
    int CalculateDamage(HeavyWeapon weapon, HeroStats stats);
    int CalculateDamage(OneHandedWeapon weapon, HeroStats stats);
    int CalculateDamage(MagicalWeapon weapon, HeroStats stats);
    int CalculateDefaultDamage(HeroStats stats);
    
    int CalculateDefense(HeavyWeapon weapon, HeroStats stats);
    int CalculateDefense(OneHandedWeapon weapon, HeroStats stats);
    int CalculateDefense(MagicalWeapon weapon, HeroStats stats);
    int CalculateDefaultDefence(HeroStats stats);
    
}