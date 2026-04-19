
using ConsoleApp1.ConfigurationFile;

namespace ConsoleApp1.Logger;

public class EventLog
{
    private EventLog(){}
    private static EventLog? eventLog;
    private string HeroName;
    private ConfigManager configManager;
    private ISavingLogsStrategy SavingLogsStrategy { get; set; }
    
    public static EventLog GetEventLog()
    {
        if (eventLog == null)
            eventLog = new EventLog();
        return eventLog;
    }

    public void Initialise(string configurationFilePath, ISavingLogsStrategy savingLogsStrategy)
    {
        configManager = new ConfigManager(configurationFilePath);
        HeroName = configManager.GetHeroName();
        savingLogsStrategy = new SavingLogs(configManager.GetLogPath());
    }
    
    public void Log(LogType logType, string[]? context = null)
    {
        string message = getMessage(logType, context);
        SavingLogsStrategy.Save($"{DateTime.Now.ToString()} : {message}");
        //render recent messages
        
    }
    
    public void renderAllLogs()
    {
        
    }

    private List<string> GetRecentLogs()
    {
        return SavingLogsStrategy.Load().Take(3).ToList();
    }
    
    private string getMessage(LogType logType, string[]? context = null)
    {
        return logType switch
        {
            LogType.WallHit => LogTexts.WallHit(HeroName),
            LogType.ButtonHit => LogTexts.ButtonHit(HeroName),
            LogType.ItemPick => LogTexts.ItemPick(HeroName, context != null ? context[0] : "unknown item"),
            LogType.WeaponEquip => LogTexts.WeaponEquip(HeroName, context[0] ?? "unknownWeapon"),
            LogType.HeroHits => LogTexts.HeroHits(HeroName, context ?? new string[]{"unknown enemy", "unknown"}),
            LogType.EnemyHits => LogTexts.EnemyHits(HeroName, context ?? new string[]{"unknown enemy", "unknown"}),
            LogType.DefeatedEnemy => LogTexts.DefeatedEnemy(HeroName, context != null ? context[0] : "unknown enemy"),
            LogType.DefeatedHero => LogTexts.DefeatedEnemy(HeroName, context != null ? context[0] : "unknown hero"),
            _ => "unknown event"
        };
    }
}


