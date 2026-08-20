using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1
{
    public class GameMap
    {
        public Stack<Item>?[,] map;
        public Enemy?[,] enemies;
        public Hero?[,] heroes;
        public int ExistingFiels;

        public GameMap()
        {
            map = new Stack<Item>?[ModelConsts.MapHeight, ModelConsts.MapWidth];
            enemies = new Enemy?[ModelConsts.MapHeight, ModelConsts.MapWidth];
            heroes = new Hero?[ModelConsts.MapHeight, ModelConsts.MapWidth];
            ExistingFiels = 0;
        }
        
        public (int X, int Y) GetRandomFreePosition()
        {
            Random rnd = new Random();
            int x, y;
            do
            {
                x = rnd.Next(ModelConsts.MapWidth);
                y = rnd.Next(ModelConsts.MapHeight);
            } 
            while (map[y, x] == null || enemies[y, x] != null); 
    
            return (x, y);
        }
        
        public ShallowMap MapShallower()
        {
            int height = ModelConsts.MapHeight;
            int width = ModelConsts.MapWidth;

            var shallowTypes = new TyleType[height][];
            var shallowItems = new ShallowItem?[height][];
            var shallowEnemies = new ShallowEnemy?[height][];
            
            var shallowHeroes = new List<ShallowHero>();

            for (int y = 0; y < height; y++)
            {
                shallowTypes[y] = new TyleType[width];
                shallowItems[y] = new ShallowItem?[width];
                shallowEnemies[y] = new ShallowEnemy?[width];

                for (int x = 0; x < width; x++)
                {
                    var itemStack = map[y, x];
            
                    if (itemStack == null)
                    {
                        shallowTypes[y][x] = TyleType.Wall;
                    }
                    else
                    {
                        shallowTypes[y][x] = TyleType.Normal;

                        if (itemStack.Count > 0)
                        {
                            var topItem = itemStack.Peek();
                            shallowItems[y][x] = new ShallowItem
                            {
                                Name = topItem.Name,
                                Symbol = topItem.Symbol
                            };
                        }
                    }

                    var enemy = enemies[y, x];
                    if (enemy != null)
                    {
                        shallowEnemies[y][x] = new ShallowEnemy
                        {
                            Id = enemy.Id, 
                            Hp = enemy.Hp,
                            Name = enemy.Name,
                            Symbol = enemy.Symbol,
                            Pos = new Position(x, y) 
                        };
                    }

                    var hero = heroes[y, x];
                    if (hero != null)
                    {
                        shallowHeroes.Add(ConvertToShallowHero(hero)); 
                    }
                }
            }
            
            return new ShallowMap
            {
                Map = shallowItems,
                TyleTypes = shallowTypes,
                Enemies = shallowEnemies,
                Heroes = shallowHeroes
            };
        }
        
        private static ShallowHero ConvertToShallowHero(Hero serverHero)
        {
            if (serverHero == null) return null;

            var shallowHero = new ShallowHero(serverHero.Id, serverHero.Position);

            if (!string.IsNullOrEmpty(serverHero.HeroName))
            {
                shallowHero.Name = serverHero.HeroName[0];
            }

            shallowHero.Stats = new ShallowHeroStats
            {
                Strength = serverHero.Stats.Strength,
                Agility = serverHero.Stats.Agility,
                Luck = serverHero.Stats.Luck,
                Agressiveness = serverHero.Stats.Agressiveness,
                Wisdom = serverHero.Stats.Wisdom,
                Health = serverHero.Stats.Health
            };

            shallowHero.Hands = new ShallowHeroHands
            {
                LeftHand = serverHero.Hands.LeftHand != null ? new ShallowItem 
                { 
                    Name = serverHero.Hands.LeftHand.Name, 
                    Symbol = serverHero.Hands.LeftHand.Symbol 
                } : null,
            
                RightHand = serverHero.Hands.RightHand != null ? new ShallowItem 
                { 
                    Name = serverHero.Hands.RightHand.Name, 
                    Symbol = serverHero.Hands.RightHand.Symbol 
                } : null
            };

            shallowHero.Equipment = new ShallowEquipment
            {
                Coins = serverHero.Equipment.Coins,
                Gold = serverHero.Equipment.Gold,
                EquipmentList = serverHero.Equipment.EquipmentList.Select(item => new ShallowItem
                {
                    Name = item.Name,
                    Symbol = item.Symbol
                }).ToList()
            };

            return shallowHero;
        }
    }
}
