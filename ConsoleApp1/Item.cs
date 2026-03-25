
namespace ConsoleApp1
{

    public enum ItemType
    {
        Useless,
        Weapon,
        Gold,
        Coin
    }

    public abstract class Item
    {
        public string Name { get; set; }
        public char Symbol { get; set; }
        public ItemType ItemType { get; set; }

        public virtual bool Wear(Hero hero)
        {
            return false;
        }

        public abstract void OnPickup(HerosEquipment equipment);
        
        public Item(string name, ItemType itemType, char? symbol = null)
        {
            Name = name;
            ItemType = itemType;
            Symbol = symbol != null ? symbol.Value : Name[0];
        }
    }
    
    public enum WeaponType
    {
        OneHanded,
        TwoHanded,
        Shield
    }
    public class Weapon : Item
    {
        public const int WeaponTypeCount = 3; //number of WeaponTypes 
        public WeaponType WeaponType { get; set; }
        public Weapon(string name, WeaponType weaponType, char? symbol = null) : base(name, ItemType.Weapon, symbol)
        {
            WeaponType = weaponType;
        }

        public override void OnPickup(HerosEquipment equipment)
        {
            equipment.EquipmentList.Add(this);
        }

        public override bool Wear(Hero hero)
        {
            if (this.WeaponType != WeaponType.TwoHanded)
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

        public bool Unwear(Hero hero)
        {
            //if(this.WeaponType == WeaponType.TwoHanded)
            //{
            //    hero.Hands.RightHand = null;
            //    hero.Hands.LeftHand = null;
            //    return true;
            //}
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
    }

    internal class UselessItem : Item
    {
        public UselessItem(string name, char? symbol = null) : base(name, ItemType.Useless, symbol)
        {
        }

        public override void OnPickup(HerosEquipment equipment)
        {
            equipment.EquipmentList.Add(this);
        }
    }

    internal class Gold : Item
    {
        public Gold() : base("Gold", ItemType.Gold, 'G')
        {
        }
        public override void OnPickup(HerosEquipment equipment)
        {
            equipment.Gold++;
        }
    }

    internal class Coin : Item
    {
        public Coin() : base("Coin", ItemType.Coin, 'C')
        {
        }
        
        public override void OnPickup(HerosEquipment equipment)
        {
            equipment.Coins++;
        }
    }
}
