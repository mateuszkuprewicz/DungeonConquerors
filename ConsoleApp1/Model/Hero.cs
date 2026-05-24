using ConsoleApp1.Items.Weapon;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public class Hero
    {
        public string HeroName { private get; set; }
        public  HeroStats Stats { get; private set; }
        public HeroHands Hands { get; private set; }
        public HerosEquipment Equipment { get; private set; }
        public (int X, int Y) Position { get; private set; }

        private ISoundPublisher _soundPublisher;
        
        public bool Move(Direction direction, GameMap gameMap)
        {
            (int, int) newPosition = (-1,-1);
            switch (direction)
            {
                case Direction.Up:
                    newPosition = (Position.X, Position.Y - 1);
                    break;
                case Direction.Down:
                    newPosition = (Position.X, Position.Y + 1);
                    break;
                case Direction.Left:
                    newPosition = (Position.X - 1, Position.Y);
                    break;
                case Direction.Right:
                    newPosition = (Position.X + 1, Position.Y);
                    break;
            }
            if(IsPositionValid(newPosition, gameMap))
            {
                Position = newPosition;
                return true;
            }
            
            return false;
        }
        private static bool IsPositionValid((int X, int Y) position, GameMap gameMap)
        {
            if (position.X < 0 || position.X >= GameMap.MapWidth) return false;
            if (position.Y < 0 || position.Y >= GameMap.MapHeight) return false;
            if (gameMap.map[position.Y, position.X] == null) return false;
            return true;
        }
        public Hero(ISoundPublisher soundPublisher)
        {             
            Stats = new HeroStats();
            _soundPublisher = soundPublisher;
            Equipment = new HerosEquipment(this, _soundPublisher);
            Hands = new HeroHands();
            Position = (0, 0);
        }
        public Hero(int strength, int agility, int luck, int agressiveness, int wisdom, int health, ISoundPublisher soundPublisher)
        {
            Stats = new HeroStats(strength, agility, luck, agressiveness, wisdom, health);
            _soundPublisher = soundPublisher;
            Equipment = new HerosEquipment(this, _soundPublisher);
            Hands = new HeroHands();
            Position = (0, 0);
        }
    }

    public class HeroStats
    {
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }
        public int Agressiveness { get; set; }
        public int Wisdom { get; set; }
        public int Health { get; set; }
        public HeroStats()
        {
            Strength = 5;
            Agility = 5;
            Luck = 5;
            Agressiveness = 5;
            Wisdom = 5;
            Health = 100;
        }
        public HeroStats(int strength, int agility, int luck, int agressiveness, int wisdom, int health)
        {
            Strength = strength;
            Agility = agility;
            Luck = luck;
            Agressiveness = agressiveness;
            Wisdom = wisdom;
            Health = health;
        }
    }

    public class HerosEquipment
    {
        public readonly int MaxEquipment = 10;
        public int Coins { get; set; }
        public int Gold { get; set; }
        private Hero hero;

        private ISoundPublisher _soundPublisher; 

        public List<Item> EquipmentList { get; private set; }
        public int EquipmentPointer = 0;
        public (int completion, Item? item) PickItem((int X, int Y) position, GameMap gameMap)
        {
            if (gameMap.map[position.Y, position.X].Count() == 0)
                return (0, null);
            if(EquipmentList.Count >= MaxEquipment)
                return (-1, null);
            var item = gameMap.map[position.Y, position.X].Pop();
            item.OnPickup(this);
            if(item.SoundRange > 0)
                _soundPublisher.Notify(hero.Position, item.SoundRange);
            return (1, item);
        }
        public bool DropItem((int X, int Y) position, GameMap gameMap)
        {
            if (EquipmentPointer < 0 || EquipmentPointer >= EquipmentList.Count)
            {
                EquipmentPointer = 0;
                return false;
            }
            var item = EquipmentList[EquipmentPointer];
            EquipmentList.RemoveAt(EquipmentPointer);
            gameMap.map[position.Y, position.X].Push(item);
            if(EquipmentPointer > 0)
                EquipmentPointer--;
            return true;
        }
        public HerosEquipment(Hero hero, ISoundPublisher soundPublisher)
        {
            Coins = 0;
            Gold = 0;
            EquipmentList = new List<Item>();
            this.hero = hero;
            _soundPublisher = soundPublisher;
        }
        public HerosEquipment(int money, int gold, Hero hero, ISoundPublisher soundPublisher)
        {
            Coins = money;
            Gold = gold;
            EquipmentList = new List<Item>();
            this.hero = hero;
            _soundPublisher = soundPublisher;
        }
    }

    public class HeroHands
    {
        public AbstractWeapon? LeftHand { get; set; }
        public AbstractWeapon? RightHand { get; set; }
        public (bool completion, Item? item) EquipWeapon(Hero hero)
        {
            HerosEquipment equipment = hero.Equipment;
            if(equipment.EquipmentList.Count == 0) return (false, null);
            var item = equipment.EquipmentList[equipment.EquipmentPointer];
            if(item.Wear(hero))
            {
                equipment.EquipmentList.RemoveAt(equipment.EquipmentPointer);
                if(equipment.EquipmentPointer > 0)
                    equipment.EquipmentPointer--;
                return (true, item);
            }
            else return (false, null);
        }
        public bool UnequipWeapon(Hero hero, GameMap map)
        {
            AbstractWeapon? item = RightHand ?? LeftHand;
            if(item == null)
                return false;
            if(item.Unwear(hero))
            {
                if(hero.Equipment.EquipmentList.Count >= hero.Equipment.MaxEquipment)
                {
                    map.map[hero.Position.Y, hero.Position.X].Push(item);
                }
                else
                {
                    hero.Equipment.EquipmentList.Add(item);
                }
                return true;
            }
            else return false;
        }
        public HeroHands()
        {
            LeftHand = null;
            RightHand = null;
        }
    }
}
