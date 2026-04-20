namespace ConsoleApp1.FightLoop.Visitor.CalculateBonusDamageVisitor;

public class BonusDamageVisitor
{
    public int CalculateBonusDamage(AbstractWeaponDecorator weapon)
    {
        return weapon.WeaponDamage + weapon.InnerWeapon.AcceptCalculateBonusDamage(this);
    }

    public int CalculateBonusDamage(AbstractPhysicalWeapon weapon)
    {
        return weapon.WeaponDamage;
    }
}