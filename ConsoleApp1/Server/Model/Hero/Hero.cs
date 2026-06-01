using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.GameState;
using ConsoleApp1.Items.Weapon;
using ConsoleApp1.SoundPropagation.SoundMediation;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1
{
    public class Hero
    {
        public string HeroName { get; set; }
        public int Id { get; set; }
        public  HeroStats Stats { get; private set; }
        public HeroHands Hands { get; private set; }
        public HerosEquipment Equipment { get; private set; }
        public (int X, int Y) Position { get; set; }
        
        public HeroStateContext HeroStateContext { get; private set; }

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
                HeroStateContext.Update(Position, gameMap);
                return true;
            }
            
            return false;
        }
        private static bool IsPositionValid((int X, int Y) position, GameMap gameMap)
        {
            if (position.X < 0 || position.X >= ModelConsts.MapWidth) return false;
            if (position.Y < 0 || position.Y >= ModelConsts.MapHeight) return false;
            if (gameMap.map[position.Y, position.X] == null) return false;
            if (gameMap.heroes[position.Y, position.X] != null) return false;
            return true;
        }
        public Hero(int Id, ISoundPublisher soundPublisher)
        {
            this.Id = Id;
            Stats = new HeroStats();
            _soundPublisher = soundPublisher;
            Equipment = new HerosEquipment(this, _soundPublisher);
            Hands = new HeroHands();
            HeroStateContext = new HeroStateContext();
        }
        
        public ShallowHero ToShallowHero()
        {
            var shallowHero = new ShallowHero
            {
                Id = this.Id,
                Name = this.Id.ToString()[0], 
                Pos = new Position(this.Position.X, this.Position.Y),
        
                Stats = new ShallowHeroStats
                {
                    Strength = this.Stats.Strength,
                    Agility = this.Stats.Agility,
                    Luck = this.Stats.Luck,
                    Agressiveness = this.Stats.Agressiveness,
                    Wisdom = this.Stats.Wisdom,
                    Health = this.Stats.Health
                },
        
                Equipment = new ShallowEquipment
                {
                    Coins = this.Equipment.Coins,
                    Gold = this.Equipment.Gold,
                    EquipmentPointer = this.Equipment.EquipmentPointer,
            
                    EquipmentList = this.Equipment.EquipmentList.Select(item => new ShallowItem 
                    { 
                       Symbol = item.Symbol,
                       Name = item.Name
                    }).ToList()
                },
        
                Hands = new ShallowHeroHands
                {
                    LeftHand = this.Hands.LeftHand != null ? new ShallowItem 
                    { 
                        Symbol = this.Hands.LeftHand.Symbol,
                        Name = this.Hands.LeftHand.Name
                    } : null,
            
                    RightHand = this.Hands.RightHand != null ? new ShallowItem 
                    { 
                        Symbol = this.Hands.RightHand.Symbol,
                        Name = this.Hands.RightHand.Name
                    } : null
                }
            };

            return shallowHero;
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
            if(EquipmentList.Count >= ModelConsts.MaxEquipment)
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
                if(hero.Equipment.EquipmentList.Count >= ModelConsts.MaxEquipment)
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
