using System.Collections.Concurrent;
using ConsoleApp1.Server.Controller.Command;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.View;
using ConsoleApp1.Server.View.ViewCommand;


namespace ConsoleApp1.Server;
using ConsoleApp1.ConfigurationFile;
using ConsoleApp1.Logger;
using ConsoleApp1.Dungeon_Themes;
using ConsoleApp1.SoundPropagation.SoundMediation;

public class ServerProgram
{
    public async void Run(int port)
    {
        //"config.json", initializing logger
        ConfigManager configManager = new ConfigManager(Path.Combine(Environment.CurrentDirectory, "Infrastructure", "ConfigurationFile", "config.json"));
        var heroName = configManager.GetHeroName();
        var logFilePath = configManager.GetLogPath();
        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        ISavingLogsStrategy logSaver = new SavingLogs(logFilePath, heroName);
        EventLog eventLog = EventLog.GetEventLog();
        eventLog.Initialise(heroName,  logSaver);
            
        //initializing dungeon map
        GameMap map = new GameMap();
        DungeonSoundManager soundManager = new DungeonSoundManager(map);
        MapBuilder builder = new MapBuilder(map, soundManager);
        IDungeonTheme dungeonTheme = new ColonyTheme();
        MapDirector mapDirector = new MapDirector(builder, dungeonTheme);
        mapDirector.CreateDungeon();

        var cts = new CancellationTokenSource();
        var modelCommands = new BlockingCollection<IModelCommand>();
        var viewCommands = new BlockingCollection<IViewCommand>();
        var clientStates = new ClientStates.ClientStates();
        
        //Initializing collections and threads
        ClientGameInitialiser cgi = new ClientGameInitialiser(map, modelCommands, clientStates);
        var gameLoop = new GameLoop(clientStates, modelCommands, viewCommands, cts);
        var viewWriter = new ViewWriter(viewCommands, clientStates, cts);
        var serverListener = new ServerListener(8080, cgi, clientStates, cts);
        
        //starting server tasks
        List<Task> tasks = new List<Task>();
        
        
        tasks.Add(Task.Run(()=>gameLoop.Run()));
        tasks.Add(viewWriter.RunAsync());
        tasks.Add(serverListener.Run());
        
        await Task.WhenAll(tasks);
    }
}