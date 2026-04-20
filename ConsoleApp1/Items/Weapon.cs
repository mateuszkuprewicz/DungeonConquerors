namespace ConsoleApp1;

public class OneHandedWeapon : AbstractWeapon
{
    public OneHandedWeapon(string name, int weaponDamage = 0, char? symbol = null) : base(name, weaponDamage, symbol) { }

    public override void ApplyModifier(Hero hero) { }
    public override void RemoveModifier(Hero hero) { }
    
    public override int AcceptDamage(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDamage(this, stats);
    public override int AcceptDefense(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDefense(this, stats);

    public override int GetBonusDamage()
    {
        return _weaponDamage;
    }
}

public class HeavyWeapon : AbstractWeapon
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

    public override  void ApplyModifier(Hero hero) { }
    public override void RemoveModifier(Hero hero) { }
    
    public override int AcceptDamage(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDamage(this, stats);
    public override int AcceptDefense(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDefense(this, stats);
    
    public override int GetBonusDamage()
    {
        return _weaponDamage;
    }
}

public class MagicalWeapon : AbstractWeapon, IWeaponDecorated
{
    public MagicalWeapon(string name, int weaponDamage = 0, char? symbol = null) : base(name, weaponDamage, symbol) { }

    public override void ApplyModifier(Hero hero) { }
    public override void RemoveModifier(Hero hero) { }
    
    public override  int AcceptDamage(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDamage(this, stats);
    public override  int AcceptDefense(IAttackVisitor visitor, HeroStats stats)
        => visitor.CalculateDefense(this, stats);
    public override int GetBonusDamage()
    {
        return _weaponDamage;
    }
}