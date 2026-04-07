namespace ConsoleApp1;

public enum WeaponType
{
    OneHanded,
    TwoHanded,
    Shield
}

public abstract class AbstractWeapon : Item
{
    public AbstractWeapon(string name, char? symbol = null) : base(name, ItemType.Weapon, symbol){}
    public override void OnPickup(HerosEquipment equipment)
    {
        equipment.EquipmentList.Add(this);
    }
        
    public virtual WeaponType Type { get; set; }
        
    public override bool Wear(Hero hero)
    {
        if (this.Type != WeaponType.TwoHanded)
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
            else
            {
                return false;
            }
        }
        else
        {
            if (hero.Hands.RightHand == null && hero.Hands.LeftHand == null)
            {
                hero.Hands.RightHand = this;
                hero.Hands.LeftHand = this;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public virtual bool Unwear(Hero hero)
    {
        if (hero.Hands.RightHand != this && hero.Hands.LeftHand != this)
        {
            return false;
        }
        if (hero.Hands.RightHand == this)
        {
            hero.Hands.RightHand = null;
        }
        if (hero.Hands.LeftHand == this)
        {
            hero.Hands.LeftHand = null;
        }
        return true;
    }
        
    public abstract void ApplyModifier(Hero hero);
    public abstract void RemoveModifier(Hero hero);
}