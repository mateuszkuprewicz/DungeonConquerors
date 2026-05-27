// using System.Windows.Markup;
// using ConsoleApp1.ChainOfKeyOperations;
// using ConsoleApp1.ConfigurationFile;
// using ConsoleApp1.Dungeon_Themes;
// using ConsoleApp1.GameState;
// using ConsoleApp1.Logger;
// using ConsoleApp1.LoopState;
// using ConsoleApp1.SoundPropagation.SoundMediation;
// using ConsoleApp1.View;
//
// namespace ConsoleApp1
// {
//     internal class GameLoop
//     {
//         static void Main(string[] args)
//         {
//             //"config.json", initializing logger
//             ConfigManager configManager = new ConfigManager(Path.Combine(Environment.CurrentDirectory, "Infrastructure", "ConfigurationFile", "config.json"));
//             var heroName = configManager.GetHeroName();
//             var logFilePath = configManager.GetLogPath();
//             string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
//             ISavingLogsStrategy logSaver = new SavingLogs(logFilePath, heroName);
//             EventLog eventLog = EventLog.GetEventLog();
//             eventLog.Initialise(heroName,  logSaver);
//             
//             //initializing hero, and dungeon map
//             GameMap map = new GameMap();
//             DungeonSoundManager soundManager = new DungeonSoundManager(map);
//             Hero myHero = new Hero(soundManager);
//             myHero.HeroName = heroName;
//             MapBuilder builder = new MapBuilder(map, soundManager);
//             IDungeonTheme dungeonTheme = new ColonyTheme();
//             MapDirector mapDirector = new MapDirector(builder, dungeonTheme);
//             mapDirector.CreateDungeon();
//             
//             InstructionRender instructionRender = new InstructionRender();
//             InstructionBuilder instructionBuilder = new InstructionBuilder(myHero, map, instructionRender);
//             
//             Render render = new Render(myHero, map);
//             render.RenderAll();
//             LogRenderer log_render = new LogRenderer(eventLog, new Lock());
//             
//             //initializing game loop 
//             ConsoleKeyInfo key;
//             GameStateContext stateContext = new GameStateContext(map, myHero, render, log_render); 
//             
//             while (true)
//             {
//                 instructionBuilder.PrintInstructionInGameLoop();
//                 
//                 key = Console.ReadKey(true);
//                 stateContext.HandleInput(key.Key);
//                 
//                 //Enemies Move
//                 stateContext.Update();
//                 
//                 //corect render
//                 stateContext.Render();
//                 
//             }
//         }
//     }
// }
