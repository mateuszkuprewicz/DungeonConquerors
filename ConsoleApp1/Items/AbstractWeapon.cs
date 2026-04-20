using ConsoleApp1.FightLoop.Visitor.CalculateBonusDamageVisitor;

namespace ConsoleApp1;

public abstract class AbstractWeapon : Item, IWeaponDecorated
{
    public AbstractWeapon(string name, int weaponDamage = 0, char? symbol = null) : base(name, symbol)
    {
        this.WeaponDamage = weaponDamage;
    }

    public override void OnPickup(HerosEquipment equipment)
    {
        equipment.EquipmentList.Add(this);
    }

    public override bool Wear(Hero hero)
    {
        if (hero.Hands.RightHand == null)
        {
            hero.Hands.RightHand = this;
            return true;
        }
        else if (hero.Hands.LeftHand == null)
        {
            hero.Hands.LeftHand = this;
            return true;
        }
        return false;
    }

    public virtual bool Unwear(Hero hero)
    {
        if (hero.Hands.RightHand != this && hero.Hands.LeftHand != this)
            return false;
        if (hero.Hands.RightHand == this)
            hero.Hands.RightHand = null;
        if (hero.Hands.LeftHand == this)
            hero.Hands.LeftHand = null;
        return true;
    }

    public int WeaponDamage = 0;
    
    public abstract void ApplyModifier(Hero hero);
    public abstract void RemoveModifier(Hero hero);
    public abstract int AcceptDamage(IAttackVisitor visitor, HeroStats stats);
    public abstract int AcceptDefense(IAttackVisitor visitor, HeroStats stats);
    public abstract int AcceptCalculateBonusDamage(BonusDamageVisitor visitor);

}

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