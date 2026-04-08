namespace ConsoleApp1;

public class OneHandedWeapon : AbstractWeapon
{
    public OneHandedWeapon(string name, char? symbol = null) : base(name, symbol) { }

    public override void ApplyModifier(Hero hero) { }
    public override void RemoveModifier(Hero hero) { }
}

public class HeavyWeapon : AbstractWeapon
{
    public HeavyWeapon(string name, char? symbol = null) : base(name, symbol) { }

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

    public override void ApplyModifier(Hero hero) { }
    public override void RemoveModifier(Hero hero) { }
}

public class MagicalWeapon : AbstractWeapon
{
    public MagicalWeapon(string name, char? symbol = null) : base(name, symbol) { }

    public override void ApplyModifier(Hero hero) { }
    public override void RemoveModifier(Hero hero) { }
}