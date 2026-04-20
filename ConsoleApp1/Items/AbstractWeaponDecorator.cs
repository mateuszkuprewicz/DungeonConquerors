using System.Runtime.CompilerServices;
using ConsoleApp1.FightLoop.Visitor.CalculateBonusDamageVisitor;

namespace ConsoleApp1;

public abstract class AbstractWeaponDecorator : AbstractWeapon
{
    public IWeaponDecorated InnerWeapon { get; }
    protected int ModifierValue { get; }

    protected AbstractWeaponDecorator(IWeaponDecorated innerWeapon, int modifierValue, int bonusDamage, string suffix)
        : base($"{innerWeapon.Name} ({suffix})", innerWeapon.Symbol)
    {
        InnerWeapon = innerWeapon;
        ModifierValue = modifierValue;
        WeaponDamage =  bonusDamage;
    }
    
    
    public override bool Wear(Hero hero)
    {
        bool b = InnerWeapon.Wear(hero);
        if (b)
        {
            ReplaceInHands(hero);
            ApplyModifier(hero);
        }
        return b;
    }

    public override bool Unwear(Hero hero)
    {
        bool b = base.Unwear(hero);
        if (b)
        {
            RemoveModifier(hero);
            //InnerWeapon.RemoveModifier(hero);
        }
        return b;
    }
    
    private void ReplaceInHands(Hero hero)
    {
        if (hero.Hands.RightHand == InnerWeapon)
            hero.Hands.RightHand = this;
        if (hero.Hands.LeftHand == InnerWeapon)
            hero.Hands.LeftHand = this;
    }
    
    public abstract override void ApplyModifier(Hero hero);
    public abstract override void RemoveModifier(Hero hero);
    public override int AcceptDamage(IAttackVisitor visitor, HeroStats stats)
        => InnerWeapon.AcceptDamage(visitor, stats);
    public override int AcceptDefense(IAttackVisitor visitor, HeroStats stats)
        => InnerWeapon.AcceptDefense(visitor, stats);

    public override int AcceptCalculateBonusDamage(BonusDamageVisitor visitor)
    {
        return visitor.CalculateBonusDamage(this);
    }
    
}
