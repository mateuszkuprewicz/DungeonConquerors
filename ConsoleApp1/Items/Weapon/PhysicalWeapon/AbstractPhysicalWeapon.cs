namespace ConsoleApp1.Items.Weapon;
using ConsoleApp1.FightLoop.Visitor.CalculateBonusDamageVisitor;

public abstract class AbstractPhysicalWeapon : AbstractWeapon
{
    public AbstractPhysicalWeapon(string name, int weaponDamage = 0, char? symbol = null) : base(name, weaponDamage, symbol) { }

    public override void ApplyModifier(Hero hero) { }
    public override void RemoveModifier(Hero hero) { }  
    public override int AcceptCalculateBonusDamage(BonusDamageVisitor visitor)
    {
        return visitor.CalculateBonusDamage(this);
    }
}