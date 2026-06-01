using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

    private Shared.ShallowModel.GameState _state;

    public Render(Shared.ShallowModel.GameState state)
    {
        _state = state;
    }

    public void RenderAll()
    {
        if (_state?.Map == null) return; 

        lock (ConsoleLock)
        {
            Console.Clear();
            RenderMap();
            
            if (_state?.Hero != null)
            {
                RenderMenu();
            }
            
            RenderEnemies();
        }
    }
    
    public void RenderMap()
    {
        var _gameMap = _state?.Map;
        if (_gameMap?.TyleTypes == null || _gameMap.Map == null) return;

        lock (ConsoleLock)
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < MapHeight; i++)
            {
                if (_gameMap.TyleTypes[i] == null || _gameMap.Map[i] == null) continue;

                for (int j = 0; j < MapWidth; j++)
                {
                    if (_gameMap.TyleTypes[i][j] == TyleType.Normal)
                    {
                        if (_gameMap.Map[i][j] == null)
                            Console.Write(" ");
                        else Console.Write(_gameMap.Map[i][j]!.Symbol);
                    }
                    else
                    {
                        Console.Write("█");
                    }
                }
                Console.WriteLine();
            }

            if (_state?.Hero != null)
            {
                var _hero = _state.Hero;
                Console.SetCursorPosition(_hero.Pos.X, _hero.Pos.Y);
                Console.Write($"{_hero.Name}");
                Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
            }
        }
    }
    
    public void UpdateSingleTile(int x, int y)
    {
        var _gameMap = _state?.Map;
        if (_gameMap?.TyleTypes == null || y < 0 || y >= MapHeight || x < 0 || x >= MapWidth) return;

        lock (ConsoleLock)
        {
            Console.SetCursorPosition(x, y);

            if (_state.Hero != null && _state.Hero.Pos.X == x && _state.Hero.Pos.Y == y)
            {
                Console.Write(_state.Hero.Name);
            }
            else if (_gameMap.Heroes != null && _gameMap.Heroes.Any(h => h.Pos.X == x && h.Pos.Y == y))
            {
                var otherHero = _gameMap.Heroes.First(h => h.Pos.X == x && h.Pos.Y == y);
                Console.Write(otherHero.Name);
            }
            else if (_gameMap.Enemies[y]?[x] != null)
            {
                Console.Write(_gameMap.Enemies[y][x]!.Symbol);
            }
            else if (_gameMap.TyleTypes[y][x] == TyleType.Normal)
            {
                if (_gameMap.Map[y]?[x] == null)
                    Console.Write(" "); 
                else
                    Console.Write(_gameMap.Map[y][x]!.Symbol); 
            }
            else
            {
                Console.Write("█");
            }

            Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
        }
    }

    public void RenderMenu()
    {
        if (_state?.Hero?.Equipment == null || _state?.Hero?.Stats == null || _state?.Hero?.Hands == null) return;

        RenderStats();
        RenderEquipment();
        RenderHeroHands();
        RenderInfo();
    }
    
    public void UpdateEquipmentScroll(int oldPointer, int newPointer)
    {
        lock (ConsoleLock)
        {
            PrintNthEquipmentLine(oldPointer, newPointer);
            PrintNthEquipmentLine(newPointer, newPointer);
        }
    }

    public void PrintNthEquipmentLine(int i, int activePointer)
    {
        var _hero = _state?.Hero;
        if (i < 0 || _hero?.Equipment?.EquipmentList == null) return;
    
        lock (ConsoleLock)
        {
            Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 4 + i);
            Console.Write(new string(' ', Console.WindowWidth - EquipmentTableStart.Item1));
            Console.SetCursorPosition(EquipmentTableStart.Item1, EquipmentTableStart.Item2 + 4 + i);

            if (i == activePointer && _hero.Equipment.EquipmentList.Count != 0)
            {
                Console.SetCursorPosition(EquipmentTableStart.Item1 - 1, EquipmentTableStart.Item2 + 4 + i);
                Console.Write(">");
            }
            else
            {
                Console.SetCursorPosition(EquipmentTableStart.Item1 - 1, EquipmentTableStart.Item2 + 4 + i);
                Console.Write(" ");
            }
        
            if (i < _hero.Equipment.EquipmentList.Count)
            {
                Console.Write($"{_hero.Equipment.EquipmentList[i].Name}");
            }

            Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
        }
    }

    public void RenderEquipment()
    {
        var _hero = _state?.Hero;
        if (_hero?.Equipment?.EquipmentList == null) return;

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
                PrintNthEquipmentLine(i, _hero.Equipment.EquipmentPointer);
            }
            
            for(int i = _hero.Equipment.EquipmentList.Count; i < 10; i++) 
            {
                Console.SetCursorPosition(EquipmentTableStart.Item1 - 1, EquipmentTableStart.Item2 + 4 + i);
                Console.Write(new string(' ', Console.WindowWidth - EquipmentTableStart.Item1 + 1));
            }

            Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
        }
    }

    public void RenderStats()
    {
        var _hero = _state?.Hero;
        if (_hero?.Stats == null) return;

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
        var _hero = _state?.Hero;
        if (_hero?.Hands == null) return;

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
        var _gameMap = _state?.Map;
        var _hero = _state?.Hero;
        if (_gameMap?.Map == null || _hero == null) return;

        lock(ConsoleLock)
        {
            Console.SetCursorPosition(Info.Item1, Info.Item2);
            Console.Write(new string(' ', Console.WindowWidth - Info.Item1));

            Console.SetCursorPosition(Info.Item1, Info.Item2);
            Console.Write("You are standing on: ");
            
            if (_gameMap.Map[_hero.Pos.Y]?[_hero.Pos.X] == null)
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
        var _gameMap = _state?.Map;
        if (_gameMap?.Enemies == null) return;

        int myId = _state.Hero != null ? _state.Hero.Id : -1; 
        
        lock (ConsoleLock)
        {
            for (int i = 0; i < MapHeight; i++)
            {
                if (_gameMap.Enemies[i] == null) continue;
                
                for (int j = 0; j < MapWidth; j++)
                {
                    if (_gameMap.Enemies[i][j] != null)
                    {
                        Console.SetCursorPosition(j, i);
                        
                        if (_state.Hero != null)
                        {
                            if (_state.Hero.Pos.Y != i || _state.Hero.Pos.X != j) 
                                Console.Write(_gameMap.Enemies[i][j]!.Symbol);
                        }
                        else
                        {
                            Console.Write(_gameMap.Enemies[i][j]!.Symbol);
                        }
                    }
                }
            }

            if (_gameMap.Heroes != null)
            {
                foreach (var enemy_hero in _gameMap.Heroes)
                {
                    if (myId != -1 && enemy_hero.Id == myId) continue;

                    Console.SetCursorPosition(enemy_hero.Pos.X, enemy_hero.Pos.Y);
                    Console.Write(enemy_hero.Name);
                }
            }
            Console.SetCursorPosition(DefaultCursorPosition.Item1, DefaultCursorPosition.Item2);
        }
    }
    
    private static readonly (int, int) EnemyStatsStart = (43, 16);

    public static void RenderEnemyStats(ShallowEnemy enemy) 
    {
        if (enemy == null) return;
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
            Console.SetCursorPosition(centerX, Math.Max(0, centerY));
            Console.Write("GAME OVER");
        }
    }
}