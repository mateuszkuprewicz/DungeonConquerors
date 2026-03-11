using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{

    enum ItemType
    {
        Useless,
        Weapon,
        Gold,
        Coin
    }

    internal abstract class Item
    {
        public string Name { get; set; }
        public char Symbol { get; set; }
        public ItemType ItemType { get; set; }

        public virtual bool Wear(Hero hero)
        {
            return false;
        }

        public Item(string name, ItemType itemType, char? symbol = null)
        {
            Name = name;
            ItemType = itemType;
            Symbol = symbol != null ? symbol.Value : Name[0];
        }
    }

    enum WeaponType
    {
        OneHanded,
        TwoHanded,
        Shield
    }
    internal class Weapon : Item
    {
        public WeaponType WeaponType { get; set; }
        public Weapon(string name, WeaponType weaponType, char? symbol = null) : base(name, ItemType.Weapon, symbol)
        {
            WeaponType = weaponType;
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
    }

    internal class Gold : Item
    {
        public Gold() : base("Gold", ItemType.Gold, 'G')
        {
        }
    }

    internal class Coin : Item
    {
        public Coin() : base("Coin", ItemType.Coin, 'C')
        {
        }
    }
}
