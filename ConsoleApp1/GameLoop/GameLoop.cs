using System.Windows.Markup;
using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.ConfigurationFile;
using ConsoleApp1.Dungeon_Themes;
using ConsoleApp1.Logger;
using ConsoleApp1.SoundPropagation.SoundMediation;
using ConsoleApp1.View;

namespace ConsoleApp1
{
    internal class GameLoop
    {
        static void Main(string[] args)
        {
            //"config.json", initializing logger
            ConfigManager configManager = new ConfigManager(Path.Combine(Environment.CurrentDirectory, "ConfigurationFile", "config.json"));
            var heroName = configManager.GetHeroName();
            var logFilePath = configManager.GetLogPath();
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ISavingLogsStrategy logSaver = new SavingLogs(logFilePath, heroName);
            EventLog eventLog = EventLog.GetEventLog();
            eventLog.Initialise(heroName,  logSaver);
            
            //initializing hero, and dungeon map
            GameMap map = new GameMap();
            DungeonSoundManager soundManager = new DungeonSoundManager(map);
            Hero myHero = new Hero(soundManager);
            myHero.HeroName = heroName;
            MapBuilder builder = new MapBuilder(map, soundManager);
            IDungeonTheme dungeonTheme = new ColonyTheme();
            MapDirector mapDirector = new MapDirector(builder, dungeonTheme);
            mapDirector.CreateDungeon();
            
            InstructionBuilder instructionBuilder = new InstructionBuilder(myHero, map);
            
            Render render = new Render(myHero, map);
            render.RenderAll();
            LogRenderer log_render = new LogRenderer(eventLog, new Lock());
            
            //initializing game loop 
            ConsoleKeyInfo key;
            KeyNode move = new MoveNode(myHero, map, render);
            KeyNode pick = new PickDropNode(myHero, map, render);
            KeyNode weaponEquip = new WeaponEquipmentNode(myHero, map, render);
            KeyNode  scroll = new EquipmentScrollNode(render);
            KeyNode fight = new FightNode(myHero, map, render);
            KeyNode log = new LogChangeViewNode(log_render, render);
            KeyNode sentinel = new Sentinel();
            move.SetNextHandler(pick);
            pick.SetNextHandler(weaponEquip);
            weaponEquip.SetNextHandler(scroll);
            scroll.SetNextHandler(fight);
            fight.SetNextHandler(log);
            log.SetNextHandler(sentinel);
            while (true)
            {
                instructionBuilder.PrintInstructionInGameLoop();
                key = Console.ReadKey(true);
                move.HandleKey(key.Key);
                
                //Enemies Move
                var enemiesToMove = new HashSet<Enemy>();
                foreach (var enemy in map.enemies)
                {
                    if (enemy != null)
                    {
                        enemiesToMove.Add(enemy);
                    }
                }
                
                foreach (var enemy in enemiesToMove)
                {
                    enemy.Move();
                }
                
                //corect render
                render.RenderMap();
                render.RenderEnemies();

                if (log_render.IsRenderingAllLogs) log_render.RenderAll();
                else log_render.RenderLast();
            }
        }
    }
}
