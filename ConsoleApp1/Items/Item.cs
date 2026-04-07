
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
