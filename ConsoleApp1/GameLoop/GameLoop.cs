using System.Windows.Markup;
using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.ConfigurationFile;
using ConsoleApp1.Dungeon_Themes;
using ConsoleApp1.Logger;

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
            Hero myHero = new Hero();
            myHero.HeroName = heroName;
            GameMap map = new GameMap();
            MapBuilder builder = new MapBuilder(map);
            IDungeonTheme dungeonTheme = new ColonyTheme();
            MapDirector mapDirector = new MapDirector(builder, dungeonTheme);
            mapDirector.CreateDungeon();
            
            InstructionBuilder instructionBuilder = new InstructionBuilder(myHero, map);
            
            Render.RenderMap(myHero, map);
            Render.RenderEnemies(map, myHero);
            Render.RenderMenu(myHero, map);

            //initializing game loop 
            ConsoleKeyInfo key;
            KeyNode move = new MoveNode(myHero, map);
            KeyNode pick = new PickDropNode(myHero, map);
            KeyNode weaponEquip = new WeaponEquipmentNode(myHero, map);
            KeyNode  scroll = new EquipmentScrollNode(myHero);
            KeyNode fight = new FightNode(myHero, map);
            KeyNode logView = new LogChangeViewNode();
            KeyNode logScroll = new LogScrollNode();
            KeyNode sentinel = new Sentinel();
            move.SetNextHandler(pick);
            pick.SetNextHandler(weaponEquip);
            weaponEquip.SetNextHandler(scroll);
            scroll.SetNextHandler(fight);
            fight.SetNextHandler(logView);
            logView.SetNextHandler(logScroll);
            logScroll.SetNextHandler(sentinel);
            while (true)
            {
                instructionBuilder.PrintInstructionInGameLoop();
                key = Console.ReadKey(true);
                move.HandleKey(key.Key);
                //eventLog.Log();
            }
        }
    }
}
