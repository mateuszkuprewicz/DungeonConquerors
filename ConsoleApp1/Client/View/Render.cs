using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Client.View;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.ShallowModel;
using ConsoleApp1.View;

namespace ConsoleApp1;

public class Render
{
    public static Lock ConsoleLock = new Lock();
    
    // Nowa flaga blokująca rysowanie UI
    public static bool IsRenderingFullScreenMode = false;
    
    private Shared.ShallowModel.GameState _state;

    public Render(Shared.ShallowModel.GameState state)
    {
        _state = state;
    }

    public void RenderAll()
    {
        if (IsRenderingFullScreenMode) return;
        
        Console.Clear();
        if (_state?.Map == null) return; 

        lock (ConsoleLock)
        {
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
        if (IsRenderingFullScreenMode) return;
        
        var _gameMap = _state?.Map;
        if (_gameMap?.TyleTypes == null || _gameMap.Map == null) return;

        lock (ConsoleLock)
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < RenderConsts.MapHeight; i++)
            {
                if (_gameMap.TyleTypes[i] == null || _gameMap.Map[i] == null) continue;

                for (int j = 0; j < RenderConsts.MapWidth; j++)
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
                Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
            }
        }
    }
    
    public void UpdateSingleTile(int x, int y)
    {
        if (IsRenderingFullScreenMode) return;
        
        var _gameMap = _state?.Map;
        if (_gameMap?.TyleTypes == null || y < 0 || y >= RenderConsts.MapHeight || x < 0 || x >= RenderConsts.MapWidth) return;

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

            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }

    public void RenderMenu()
    {
        if (IsRenderingFullScreenMode) return;
        
        if (_state?.Hero?.Equipment == null || _state?.Hero?.Stats == null || _state?.Hero?.Hands == null) return;

        RenderStats();
        RenderEquipment();
        RenderHeroHands();
        RenderInfo();

        if (_state.Map?.Enemies != null && 
            _state.Hero.Pos.Y >= 0 && _state.Hero.Pos.Y < RenderConsts.MapHeight &&
            _state.Hero.Pos.X >= 0 && _state.Hero.Pos.X < RenderConsts.MapWidth)
        {
            var enemy = _state.Map.Enemies[_state.Hero.Pos.Y][_state.Hero.Pos.X];
            if (enemy != null)
            {
                RenderEnemyStats(enemy);
                RenderCombatInstructions();
            }
            else
            {
                ClearEnemyStats();
                RenderExplorationInstructions();
            }
        }
        else
        {
            ClearEnemyStats();
            RenderExplorationInstructions();
        }
    }

    private void RenderCombatInstructions()
    {
        if (IsRenderingFullScreenMode) return;
        
        lock (ConsoleLock)
        {
            for (int i = RenderConsts.Instruction.Item2; i < RenderConsts.DefaultCursorPosition.Item2; i++)
            {
                Console.SetCursorPosition(RenderConsts.Instruction.Item1, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            
            Console.SetCursorPosition(RenderConsts.Instruction.Item1, RenderConsts.Instruction.Item2);
            Console.Write($"{KeyConsts.Hit.letter} - hit enemy.");
            
            Console.SetCursorPosition(RenderConsts.Instruction.Item1, RenderConsts.Instruction.Item2 + 1);
            Console.Write($"{KeyConsts.Leave.letter} - run away.");
            
            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }

    private void RenderExplorationInstructions()
    {
        if (IsRenderingFullScreenMode) return;
        
        lock (ConsoleLock)
        {
            for (int i = RenderConsts.Instruction.Item2; i < RenderConsts.DefaultCursorPosition.Item2; i++)
            {
                Console.SetCursorPosition(RenderConsts.Instruction.Item1, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            
            Console.SetCursorPosition(RenderConsts.Instruction.Item1, RenderConsts.Instruction.Item2);
            Console.Write($"Use {KeyConsts.MoveLeft.letter} {KeyConsts.MoveUp.letter} {KeyConsts.MoveRight.letter} {KeyConsts.MoveDown.letter} to move.");
            
            Console.SetCursorPosition(RenderConsts.Instruction.Item1, RenderConsts.Instruction.Item2 + 1);
            Console.Write($"{KeyConsts.PickItem.letter} - pick item, {KeyConsts.DropItem.letter} - drop item.");
            
            Console.SetCursorPosition(RenderConsts.Instruction.Item1, RenderConsts.Instruction.Item2 + 2);
            Console.Write($"{KeyConsts.EquipWeapon.letter} - equip weapon, {KeyConsts.UnequipWeapon.letter} - unequip weapon.");

            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }
    
    public void UpdateEquipmentScroll(int oldPointer, int newPointer)
    {
        if (IsRenderingFullScreenMode) return;
        
        lock (ConsoleLock)
        {
            PrintNthEquipmentLine(oldPointer, newPointer);
            PrintNthEquipmentLine(newPointer, newPointer);
        }
    }

    public void PrintNthEquipmentLine(int i, int activePointer)
    {
        if (IsRenderingFullScreenMode) return;
        
        var _hero = _state?.Hero;
        if (i < 0 || _hero?.Equipment?.EquipmentList == null) return;
    
        lock (ConsoleLock)
        {
            Console.SetCursorPosition(RenderConsts.EquipmentTableStart.Item1, RenderConsts.EquipmentTableStart.Item2 + 4 + i);
            Console.Write(new string(' ', 34));
            Console.SetCursorPosition(RenderConsts.EquipmentTableStart.Item1, RenderConsts.EquipmentTableStart.Item2 + 4 + i);

            if (i == activePointer && _hero.Equipment.EquipmentList.Count != 0)
            {
                Console.SetCursorPosition(RenderConsts.EquipmentTableStart.Item1 - 1, RenderConsts.EquipmentTableStart.Item2 + 4 + i);
                Console.Write(">");
            }
            else
            {
                Console.SetCursorPosition(RenderConsts.EquipmentTableStart.Item1 - 1, RenderConsts.EquipmentTableStart.Item2 + 4 + i);
                Console.Write(" ");
            }
        
            if (i < _hero.Equipment.EquipmentList.Count)
            {
                Console.Write($"{_hero.Equipment.EquipmentList[i].Name}");
            }

            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }

    public void RenderEquipment()
    {
        if (IsRenderingFullScreenMode) return;
        
        var _hero = _state?.Hero;
        if (_hero?.Equipment?.EquipmentList == null) return;

        lock(ConsoleLock)
        {
            Console.SetCursorPosition(RenderConsts.EquipmentTableStart.Item1, RenderConsts.EquipmentTableStart.Item2);
            Console.WriteLine($"Equipment");
            Console.SetCursorPosition(RenderConsts.EquipmentTableStart.Item1, RenderConsts.EquipmentTableStart.Item2 + 1);
            Console.Write($"Gold: {_hero.Equipment.Gold}");
            Console.SetCursorPosition(RenderConsts.EquipmentTableStart.Item1, RenderConsts.EquipmentTableStart.Item2 + 2);
            Console.Write($"Coins: {_hero.Equipment.Coins}");

            if(_hero.Equipment.EquipmentList.Count == 0)
            {
                Console.SetCursorPosition(RenderConsts.EquipmentTableStart.Item1, RenderConsts.EquipmentTableStart.Item2 + 4);
                Console.Write("No equipment.");
            }

            for (int i = 0; i < _hero.Equipment.EquipmentList.Count; i++)
            {
                PrintNthEquipmentLine(i, _hero.Equipment.EquipmentPointer);
            }
            
            for(int i = _hero.Equipment.EquipmentList.Count; i < 10; i++) 
            {
                Console.SetCursorPosition(RenderConsts.EquipmentTableStart.Item1 - 1, RenderConsts.EquipmentTableStart.Item2 + 4 + i);
                Console.Write(new string(' ', 35));
            }

            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }

    public void RenderStats()
    {
        if (IsRenderingFullScreenMode) return;
        
        var _hero = _state?.Hero;
        if (_hero?.Stats == null) return;

        lock (ConsoleLock)
        {
            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1, RenderConsts.StatsTableStart.Item2);
            Console.Write(new string(' ', 34));
            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1, RenderConsts.StatsTableStart.Item2);
            Console.Write($"Stats");

            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1, RenderConsts.StatsTableStart.Item2 + 1);
            Console.Write(new string(' ', 34));
            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1, RenderConsts.StatsTableStart.Item2 + 1);
            Console.Write($"Health: {_hero.Stats.Health}");
            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1 + RenderConsts.Tab, RenderConsts.StatsTableStart.Item2 + 1);
            Console.Write($"Luck: {_hero.Stats.Luck}");

            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1, RenderConsts.StatsTableStart.Item2 + 2);
            Console.Write(new string(' ', 34));
            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1, RenderConsts.StatsTableStart.Item2 + 2);
            Console.Write($"Strength: {_hero.Stats.Strength}");
            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1 + RenderConsts.Tab, RenderConsts.StatsTableStart.Item2 + 2);
            Console.Write($"Agility: {_hero.Stats.Agility}");

            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1, RenderConsts.StatsTableStart.Item2 + 3);
            Console.Write(new string(' ', 34));
            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1, RenderConsts.StatsTableStart.Item2 + 3);
            Console.Write($"Wisdom: {_hero.Stats.Wisdom}");
            Console.SetCursorPosition(RenderConsts.StatsTableStart.Item1 + RenderConsts.Tab, RenderConsts.StatsTableStart.Item2 + 3);
            Console.Write($"Aggresivness: {_hero.Stats.Agressiveness}");

            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }

    public void RenderHeroHands()
    {
        if (IsRenderingFullScreenMode) return;
        
        var _hero = _state?.Hero;
        if (_hero?.Hands == null) return;

        lock(ConsoleLock)
        {
            Console.SetCursorPosition(RenderConsts.HandsTableStart.Item1, RenderConsts.HandsTableStart.Item2);
            Console.Write(new string(' ', 19));
            Console.SetCursorPosition(RenderConsts.HandsTableStart.Item1, RenderConsts.HandsTableStart.Item2);
            Console.Write("LH: ");
            if (_hero.Hands.LeftHand == null) Console.Write("...");
            else Console.Write($"{_hero.Hands.LeftHand.Name}");

            Console.SetCursorPosition(RenderConsts.HandsTableStart.Item1, RenderConsts.HandsTableStart.Item2 + 1);
            Console.Write(new string(' ', 19));
            Console.SetCursorPosition(RenderConsts.HandsTableStart.Item1, RenderConsts.HandsTableStart.Item2 + 1);
            Console.Write("RH: ");
            if (_hero.Hands.RightHand == null) Console.Write("...");
            else Console.Write($"{_hero.Hands.RightHand.Name}");

            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }

    public void RenderInfo()
    {
        if (IsRenderingFullScreenMode) return;
        
        var _gameMap = _state?.Map;
        var _hero = _state?.Hero;
        if (_gameMap?.Map == null || _hero == null) return;

        lock(ConsoleLock)
        {
            Console.SetCursorPosition(RenderConsts.Info.Item1, RenderConsts.Info.Item2);
            Console.Write(new string(' ', 34));

            Console.SetCursorPosition(RenderConsts.Info.Item1, RenderConsts.Info.Item2);
            Console.Write("You are standing on: ");
            
            if (_gameMap.Map[_hero.Pos.Y]?[_hero.Pos.X] == null)
                Console.Write("nothing.");
            else
            {
                Console.Write(_gameMap.Map[_hero.Pos.Y][_hero.Pos.X]!.Name);
            }
            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }

    private static int lastAnnouncementCount = 0;
    public static async Task RenderAnnouncement(string announcement)
    {
        if (IsRenderingFullScreenMode) return;
        
        int myAnnouncementCount;
        lock (ConsoleLock)
        {
            lastAnnouncementCount++;
            myAnnouncementCount = lastAnnouncementCount;
            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
            Console.Write(announcement);
        }

        await Task.Delay(2000);

        // Upewniamy się, że w międzyczasie nie otwarto pełnego ekranu zanim wyczyścimy log z dołu
        if (IsRenderingFullScreenMode) return;
        
        lock(ConsoleLock)
        {
            if(myAnnouncementCount == lastAnnouncementCount)
            {
                Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
        }
    }
    
    public void RenderEnemies()
    {
        if (IsRenderingFullScreenMode) return;
        
        var _gameMap = _state?.Map;
        if (_gameMap?.Enemies == null) return;

        int myId = _state.Hero != null ? _state.Hero.Id : -1; 
        
        lock (ConsoleLock)
        {
            for (int i = 0; i < RenderConsts.MapHeight; i++)
            {
                if (_gameMap.Enemies[i] == null) continue;
                
                for (int j = 0; j < RenderConsts.MapWidth; j++)
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
            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }
    
    public static void RenderEnemyStats(ShallowEnemy enemy) 
    {
        if (IsRenderingFullScreenMode) return;
        
        if (enemy == null) return;
        lock (ConsoleLock)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(RenderConsts.EnemyStatsStart.Item1, RenderConsts.EnemyStatsStart.Item2 + i);
                Console.Write(new string(' ', 34));
            }

            Console.SetCursorPosition(RenderConsts.EnemyStatsStart.Item1, RenderConsts.EnemyStatsStart.Item2);
            Console.Write($"Enemy: {enemy.Name}");

            Console.SetCursorPosition(RenderConsts.EnemyStatsStart.Item1, RenderConsts.EnemyStatsStart.Item2 + 1);
            Console.Write($"Health: {enemy.Hp}");

            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }
    
    public static void ClearEnemyStats()
    {
        if (IsRenderingFullScreenMode) return;
        
        lock (ConsoleLock)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(RenderConsts.EnemyStatsStart.Item1, RenderConsts.EnemyStatsStart.Item2 + i);
                Console.Write(new string(' ', 34));
            }
            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
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