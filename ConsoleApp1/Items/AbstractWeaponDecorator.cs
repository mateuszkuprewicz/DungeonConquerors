namespace ConsoleApp1;

public abstract class AbstractWeaponDecorator : AbstractWeapon
{
    protected AbstractWeapon InnerWeapon { get; }
    protected int ModifierValue { get; }

    protected AbstractWeaponDecorator(AbstractWeapon innerWeapon, int modifierValue, string suffix)
        : base($"{innerWeapon.Name} ({suffix})", innerWeapon.Symbol)
    {
        InnerWeapon = innerWeapon;
        ModifierValue = modifierValue;
        ItemType = innerWeapon.ItemType;
    }
    
    public override WeaponType Type
    {
        get
        {
            return InnerWeapon.Type;
        }
        set{}
    }

    public override bool Wear(Hero hero)
    {
        WeaponType type = Type;
        bool b = base.Wear(hero);
        if(b)
            ApplyModifier(hero);
        return b;
    }

    public override bool Unwear(Hero hero)
    {
        bool b = base.Unwear(hero);
        if(b)
            RemoveModifier(hero);
        return b;
    }
    
    public abstract override void ApplyModifier(Hero hero);
    public abstract override void RemoveModifier(Hero hero);
}
