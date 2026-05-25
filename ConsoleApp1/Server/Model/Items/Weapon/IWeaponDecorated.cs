namespace ConsoleApp1.Items.Weapon;
using ConsoleApp1.FightLoop.Visitor.CalculateBonusDamageVisitor;
public interface IWeaponDecorated
{
    public void ApplyModifier(Hero hero);
    public void RemoveModifier(Hero hero);
    public int AcceptDamage(IAttackVisitor visitor, HeroStats stats);
    public int AcceptDefense(IAttackVisitor visitor, HeroStats stats);
    public bool Wear(Hero hero);
    public string Name
    {
        get;
    }
    public char Symbol { get; }

    public int AcceptCalculateBonusDamage(BonusDamageVisitor visitor);
}