using ConsoleApp1.Shared;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1;
    public class Render
    {
        const int MapHeight = 20;
        const int MapWidth = 40;
        const int Tab = 15;
        private static Lock ConsoleLock = new Lock();

        private static readonly (int, int) StatsTableStart = (43, 0);
        private static readonly (int, int) EquipmentTableStart = (43, 5);
        private static readonly (int, int) HandsTableStart = (43 + Tab, 6);
        private static readonly (int, int) Info = (43, 20);
        public static readonly (int, int) DefaultCursorPosition = (0, 26);
        public static readonly (int, int) Instruction = (0, 21);

        private ShallowHero _hero; 
        private ShallowMap _gameMap;

        public Render(ShallowHero hero, ShallowMap map)
        {
            _hero = hero;
            _gameMap = map;
        }

        public void RenderAll()
        {
            Console.Clear();
            RenderMap();
            RenderMenu();
            RenderEnemies();
        }
        
        public void RenderMap()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    if (_gameMap.TyleTypes[i][j] == TyleType.Normal)
                    {
                        if (_gameMap.Map[i][j] == null)
                            Console.Write(" ");
                        else Console.Write(_gameMap.Map[i][j].Symbol);
                    }
                    else
                        Console.Write("█");
                }
                Console.WriteLine();
            }

            Console.SetCursorPosition(_hero.Pos.X, _hero.Pos.Y);
            Console.Write("H");
            Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
        }

        public void RenderMenu()
        {
            RenderStats();
            RenderEquipment();
            RenderHeroHands();
            RenderInfo();
        }

        public void ActualiseAfterHeroMove((int X, int Y) previousPosition)
        {
            lock (ConsoleLock)
            {
                (int X, int Y) = previousPosition;
                Console.SetCursorPosition(X, Y);
                if(_gameMap.Enemies[Y][X] != null)
                    Console.Write("E");
                else if (_gameMap.Map[Y][X] == null)
                    Console.Write(" ");
                else Console.Write(_gameMap.Map[Y][X]!.Symbol);
                
                Console.SetCursorPosition(_hero.Pos.X, _hero.Pos.Y);
                Console.Write("H");
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }
        
        public void ActualiseAfterEnemyMove((int X, int Y) previousPosition, ShallowEnemy enemy)
        {
            lock (ConsoleLock)
            {
                (int X, int Y) = previousPosition;
                Console.SetCursorPosition(X, Y);
                if (_gameMap.Map[Y][X] == null)
                    Console.Write(" ");
                else Console.Write(_gameMap.Map[Y][X]!.Symbol);
                
                Console.SetCursorPosition(enemy.Pos.X, enemy.Pos.Y);
                Console.Write("E");
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }

        //static int EquipmentCursor = 0;
        public void EquipmentScroll(ConsoleKey k)
        {
            if(k == ConsoleKey.UpArrow)
            {
                if (_hero.Equipment.EquipmentPointer > 0 && _hero.Equipment.EquipmentPointer - 1 < _hero.Equipment.EquipmentList.Count())
                {
                    _hero.Equipment.EquipmentPointer--;
                    PrintNthEquipmentLine(_hero.Equipment.EquipmentPointer);
                    PrintNthEquipmentLine(_hero.Equipment.EquipmentPointer + 1);
                }
            }
            else if(k == ConsoleKey.DownArrow)
            {
                if (_hero.Equipment.EquipmentPointer + 1 < _hero.Equipment.EquipmentList.Count)
                {
                    _hero.Equipment.EquipmentPointer++;
                    PrintNthEquipmentLine(_hero.Equipment.EquipmentPointer);
                    PrintNthEquipmentLine(_hero.Equipment.EquipmentPointer - 1);
                }
            }
        }

        public void PrintNthEquipmentLine(int i)
        {
            if (i < 0) return;
            Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 4 + i);
            Console.Write(new string(' ', Console.WindowWidth - EquipmentTableStart.Item1));
            Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 4 + i);

            if (i == _hero.Equipment.EquipmentPointer && _hero.Equipment.EquipmentList.Count != 0)
            {
                Console.SetCursorPosition(EquipmentTableStart.Item1 - 1, EquipmentTableStart.Item2 + 4 + i);
                Console.Write(">");
            }
            else
            {
                Console.SetCursorPosition(EquipmentTableStart.Item1 - 1, EquipmentTableStart.Item2 + 4 + i);
                Console.Write(" ");
            }
            Console.Write($"{_hero.Equipment.EquipmentList[i].Name}");

            Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
        }

        public void RenderEquipment()
        {
            lock(ConsoleLock)
            {
                Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2);
                Console.WriteLine($"Equipment");
                Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 1);
                Console.Write($"Gold: {_hero.Equipment.Gold}");
                Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 2);
                Console.Write($"Coins: {_hero.Equipment.Coins}");

                if(_hero.Equipment.EquipmentList.Count == 0)
                {
                    Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 4);
                    Console.Write("No equipment.");
                }

                for (int i = 0; i < _hero.Equipment.EquipmentList.Count; i++)
                {
                    PrintNthEquipmentLine(i);
                }
                for(int i = _hero.Equipment.EquipmentList.Count; i < ModelConsts.MaxEquipment; i++)
                {
                    Console.SetCursorPosition(EquipmentTableStart.Item1 - 1, EquipmentTableStart.Item2 + 4 + i);
                    Console.Write(new string(' ', Console.WindowWidth - EquipmentTableStart.Item1 + 1));
                }

                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }

        public void RenderStats()
        {
            lock (ConsoleLock)
            {
                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2);
                Console.Write(new string(' ', Console.WindowWidth - StatsTableStart.Item1));
                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2);
                Console.Write($"Stats");

                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2 + 1);
                Console.Write(new string(' ', Console.WindowWidth - StatsTableStart.Item1));
                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2 + 1);
                Console.Write($"Health: {_hero.Stats.Health}");
                Console.SetCursorPosition(StatsTableStart.Item1 + Tab, StatsTableStart.Item2 + 1);
                Console.Write($"Luck: {_hero.Stats.Luck}");

                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2 + 2);
                Console.Write(new string(' ', Console.WindowWidth - StatsTableStart.Item1));
                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2 + 2);
                Console.Write($"Strength: {_hero.Stats.Strength}");
                Console.SetCursorPosition(StatsTableStart.Item1 + Tab, StatsTableStart.Item2 + 2);
                Console.Write($"Agility: {_hero.Stats.Agility}");

                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2 + 3);
                Console.Write(new string(' ', Console.WindowWidth - StatsTableStart.Item1));
                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2 + 3);
                Console.Write($"Wisdom: {_hero.Stats.Wisdom}");
                Console.SetCursorPosition(StatsTableStart.Item1 + Tab, StatsTableStart.Item2 + 3);
                Console.Write($"Aggresivness: {_hero.Stats.Agressiveness}");

                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }

        public void RenderHeroHands()
        {
            lock(ConsoleLock)
            {
                Console.SetCursorPosition(HandsTableStart.Item1, HandsTableStart.Item2);
                Console.Write(new string(' ', Console.WindowWidth - HandsTableStart.Item1));
                Console.SetCursorPosition(HandsTableStart.Item1, HandsTableStart.Item2);
                Console.Write("LH: ");
                if (_hero.Hands.LeftHand == null) Console.Write("...");
                else Console.Write($"{_hero.Hands.LeftHand.Name}");

                Console.SetCursorPosition(HandsTableStart.Item1, HandsTableStart.Item2 + 1);
                Console.Write(new string(' ', Console.WindowWidth - HandsTableStart.Item1));
                Console.SetCursorPosition(HandsTableStart.Item1, HandsTableStart.Item2 + 1);
                Console.Write("RH: ");
                if (_hero.Hands.RightHand == null) Console.Write("...");
                else Console.Write($"{_hero.Hands.RightHand.Name}");

                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }

        public void RenderInfo()
        {
           lock(ConsoleLock)
            {
                Console.SetCursorPosition(Info.Item1, Info.Item2);
                Console.Write(new string(' ', Console.WindowWidth - Info.Item1));

                Console.SetCursorPosition(Info.Item1, Info.Item2);
                Console.Write("You are standing on: ");
                if (_gameMap.Map[_hero.Pos.Y][_hero.Pos.X] == null)
                    Console.Write("nothing.");
                else
                {
                    Console.Write(_gameMap.Map[_hero.Pos.Y][_hero.Pos.X]!.Name);
                }
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }

        private static int lastAnnouncementCount = 0;
        public static async Task RenderAnnouncement(string announcement)
        {
            //Interlocked.Increment(ref lastAnnouncementCount);
            int myAnnouncementCount;
            lock (ConsoleLock)
            {
                lastAnnouncementCount++;
                myAnnouncementCount = lastAnnouncementCount;
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
                Console.Write(announcement);
            }

            await Task.Delay(2000);

            lock(ConsoleLock)
            {
                if(myAnnouncementCount == lastAnnouncementCount)
                {
                    Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
                    Console.WriteLine(new string(' ', Console.WindowWidth));
                }
            }
        }
        
        public void RenderEnemies()
        {
            lock (ConsoleLock)
            {
                for (int i = 0; i < ModelConsts.MapHeight; i++)
                for (int j = 0; j < ModelConsts.MapWidth; j++)
                {
                    if (_gameMap.Enemies[i][j] != null)
                    {
                        Console.SetCursorPosition(j, i);
                        if(_hero.Pos.Y != i ||  _hero.Pos.X != j) 
                            Console.Write(_gameMap.Enemies[i][j].Symbol);
                    }
                    
                }
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }
        
        private static readonly (int, int) EnemyStatsStart = (43, 16);

        public static void RenderEnemyStats(Enemy enemy)
        {
            lock (ConsoleLock)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.SetCursorPosition(EnemyStatsStart.Item1, EnemyStatsStart.Item2 + i);
                    Console.Write(new string(' ', Console.WindowWidth - EnemyStatsStart.Item1));
                }

                Console.SetCursorPosition(EnemyStatsStart.Item1, EnemyStatsStart.Item2);
                Console.Write($"Enemy: {enemy.Name}");

                Console.SetCursorPosition(EnemyStatsStart.Item1, EnemyStatsStart.Item2 + 1);
                Console.Write($"Health: {enemy.Hp}");
                Console.SetCursorPosition(EnemyStatsStart.Item1 + Tab, EnemyStatsStart.Item2 + 1);
                Console.Write($"Damage: {enemy.Damage}");

                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }
        
        public static void ClearEnemyStats()
        {
            lock (ConsoleLock)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.SetCursorPosition(EnemyStatsStart.Item1, EnemyStatsStart.Item2 + i);
                    Console.Write(new string(' ', Console.WindowWidth - EnemyStatsStart.Item1));
                }
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }
        
        public static void RenderGameOver()
        {
            lock (ConsoleLock)
            {
                Console.Clear();
                int centerX = Console.WindowWidth / 2 - 5;
                int centerY = Console.WindowHeight / 2;
                Console.SetCursorPosition(centerX, centerY);
                Console.Write("GAME OVER");
            }
        }
    }

