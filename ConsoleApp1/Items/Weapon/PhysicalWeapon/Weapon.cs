namespace ConsoleApp1.Items.Weapon;
using ConsoleApp1.FightLoop.Visitor.CalculateBonusDamageVisitor;


public class OneHandedWeapon : AbstractPhysicalWeapon
{
    public OneHandedWeapon(string name, int weaponDamage = 0, char? symbol = null) : base(name, weaponDamage, symbol){}
    
    public override int AcceptDamage(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDamage(this, stats);
    public override int AcceptDefense(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDefense(this, stats);
    

}

public class HeavyWeapon : AbstractPhysicalWeapon
{
    public HeavyWeapon(string name, int weaponDamage = 0, char? symbol = null) : base(name, weaponDamage, symbol) { }

    public override bool Wear(Hero hero)
    {
        if (hero.Hands.RightHand == null && hero.Hands.LeftHand == null)
        {
            hero.Hands.RightHand = this;
            hero.Hands.LeftHand = this;
            return true;
        }
        return false;
    }
    
    public override int AcceptDamage(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDamage(this, stats);
    public override int AcceptDefense(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDefense(this, stats);
    
}

public class MagicalWeapon : AbstractPhysicalWeapon
{
    public MagicalWeapon(string name, int weaponDamage = 0, char? symbol = null) : base(name, weaponDamage, symbol) { }
    
    public override int AcceptDamage(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDamage(this, stats);
    public override int AcceptDefense(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDefense(this, stats);

}