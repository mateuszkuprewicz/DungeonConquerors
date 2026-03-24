using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    internal static class Render
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
 
        public static void RenderMap(Hero hero, GameMap gameMap)
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    if (gameMap.map[i, j] != null)
                    {
                        if (gameMap.map[i, j].Count() == 0)
                            Console.Write(" ");
                        else Console.Write(gameMap.map[i, j].Peek().Symbol);
                    }
                    else
                        Console.Write("█");
                }
                Console.WriteLine();
            }

            Console.SetCursorPosition(hero.Position.X, hero.Position.Y);
            Console.Write("H");
            Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
        }

        public static void RenderMenu(Hero hero, GameMap gameMap)
        {
            RenderStats(hero);
            RenderEquipment(hero);
            RenderHeroHands(hero);
            RenderInfo(gameMap, hero);
        }

        public static void ActualiseAfterHeroMove(Hero hero, (int X, int Y) previousPosition, GameMap gameMap)
        {
            lock (ConsoleLock)
            {
                (int X, int Y) = previousPosition;
                Console.SetCursorPosition(X, Y);
                if (gameMap.map[Y, X].Count() == 0)
                    Console.Write(" ");
                else Console.Write(gameMap.map[Y, X].Peek().Symbol);
                Console.SetCursorPosition(hero.Position.X, hero.Position.Y);
                Console.Write("H");
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }

        //static int EquipmentCursor = 0;
        public static void EquipmentScroll(Hero hero, ConsoleKey k)
        {
            if(k == ConsoleKey.UpArrow)
            {
                if (hero.Equipment.EquipmentPointer > 0 && hero.Equipment.EquipmentPointer - 1 < hero.Equipment.EquipmentList.Count())
                {
                    hero.Equipment.EquipmentPointer--;
                    PrintNthEquipmentLine(hero, hero.Equipment.EquipmentPointer);
                    PrintNthEquipmentLine(hero, hero.Equipment.EquipmentPointer + 1);
                }
            }
            else if(k == ConsoleKey.DownArrow)
            {
                if (hero.Equipment.EquipmentPointer + 1 < hero.Equipment.EquipmentList.Count)
                {
                    hero.Equipment.EquipmentPointer++;
                    PrintNthEquipmentLine(hero, hero.Equipment.EquipmentPointer);
                    PrintNthEquipmentLine(hero, hero.Equipment.EquipmentPointer - 1);
                }
            }
        }

        public static void PrintNthEquipmentLine(Hero hero, int i)
        {
            if (i < 0) return;
            Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 4 + i);
            Console.Write(new string(' ', Console.WindowWidth - EquipmentTableStart.Item1));
            Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 4 + i);

            if (i == hero.Equipment.EquipmentPointer && hero.Equipment.EquipmentList.Count != 0)
            {
                Console.SetCursorPosition(EquipmentTableStart.Item1 - 1, EquipmentTableStart.Item2 + 4 + i);
                Console.Write(">");
            }
            else
            {
                Console.SetCursorPosition(EquipmentTableStart.Item1 - 1, EquipmentTableStart.Item2 + 4 + i);
                Console.Write(" ");
            }
            Console.Write($"{hero.Equipment.EquipmentList[i].Name}");

            Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
        }

        public static void RenderEquipment(Hero hero)
        {
            lock(ConsoleLock)
            {
                Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2);
                Console.WriteLine($"Equipment");
                Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 1);
                Console.Write($"Gold: {hero.Equipment.Gold}");
                Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 2);
                Console.Write($"Coins: {hero.Equipment.Coins}");

                if(hero.Equipment.EquipmentList.Count == 0)
                {
                    Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 4);
                    Console.Write("No equipment.");
                }

                for (int i = 0; i < hero.Equipment.EquipmentList.Count; i++)
                {
                    PrintNthEquipmentLine(hero, i);
                }
                for(int i = hero.Equipment.EquipmentList.Count; i < hero.Equipment.MaxEquipment; i++)
                {
                    Console.SetCursorPosition(EquipmentTableStart.Item1 - 1, EquipmentTableStart.Item2 + 4 + i);
                    Console.Write(new string(' ', Console.WindowWidth - EquipmentTableStart.Item1 + 1));
                }

                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }

        public static void RenderStats(Hero hero)
        {
            lock(ConsoleLock)
            {
                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2);
                Console.WriteLine($"Stats");

                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2 + 1);
                Console.Write($"Health: {hero.Stats.Health}");
                Console.SetCursorPosition(StatsTableStart.Item1 + Tab, StatsTableStart.Item2 + 1);
                Console.Write($"Luck: {hero.Stats.Luck}\n");

                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2 + 2);
                Console.Write($"Strength: {hero.Stats.Strength}");
                Console.SetCursorPosition(StatsTableStart.Item1 + Tab, StatsTableStart.Item2 + 2);
                Console.Write($"Agility: {hero.Stats.Agility}\n");

                Console.SetCursorPosition(StatsTableStart.Item1, StatsTableStart.Item2 + 3);
                Console.Write($"Wisdom: {hero.Stats.Wisdom}");
                Console.SetCursorPosition(StatsTableStart.Item1 + Tab, StatsTableStart.Item2 + 3);
                Console.Write($"Aggresivness: {hero.Stats.Agressiveness}\n");

                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }

        public static void RenderHeroHands(Hero hero)
        {
            lock(ConsoleLock)
            {
                Console.SetCursorPosition(HandsTableStart.Item1, HandsTableStart.Item2);
                Console.Write(new string(' ', Console.WindowWidth - HandsTableStart.Item1));
                Console.SetCursorPosition(HandsTableStart.Item1, HandsTableStart.Item2);

                Console.Write("LH: ");
                if (hero.Hands.LeftHand == null) Console.Write("...");
                else
                {
                    Console.Write($"{hero.Hands.LeftHand.Name}");
                }
                Console.SetCursorPosition(HandsTableStart.Item1, HandsTableStart.Item2 + 1);
                Console.Write(new string(' ', Console.WindowWidth - HandsTableStart.Item1));
                Console.SetCursorPosition(HandsTableStart.Item1, HandsTableStart.Item2 + 1);
                Console.Write("RH: ");
                if (hero.Hands.RightHand == null) Console.Write("...");
                else
                {
                    Console.Write($"{hero.Hands.RightHand.Name}");
                }
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }

        public static void RenderInfo(GameMap map, Hero hero)
        {
           lock(ConsoleLock)
            {
                Console.SetCursorPosition(Info.Item1, Info.Item2);
                Console.Write(new string(' ', Console.WindowWidth - Info.Item1));

                Console.SetCursorPosition(Info.Item1, Info.Item2);
                Console.Write("You are standing on: ");
                if (map.map[hero.Position.Y, hero.Position.X] == null || map.map[hero.Position.Y, hero.Position.X].Count == 0)
                    Console.Write("nothing.");
                else
                {
                    Console.Write(map.map[hero.Position.Y, hero.Position.X].Peek().Name);
                }
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }

        private static int lastAnnouncementCount = 0;
        public static async Task RenderAnnouncement(string announcement)
        {
            Interlocked.Increment(ref lastAnnouncementCount);
            int myAnnouncementCount = lastAnnouncementCount;
            lock (ConsoleLock)
            {
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
                Console.WriteLine(announcement);
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
    }
}
