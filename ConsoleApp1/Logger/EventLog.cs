
using ConsoleApp1.ConfigurationFile;

namespace ConsoleApp1.Logger;

public class EventLog
{
    private EventLog(){}
    private static EventLog? eventLog;
    private string HeroName;
    private ISavingLogsStrategy SavingLogsStrategy { get; set; }
    
    public static EventLog GetEventLog()
    {
        if (eventLog == null)
            eventLog = new EventLog();
        return eventLog;
    }

    public void Initialise(string heroName, ISavingLogsStrategy savingLogsStrategy)
    {
        HeroName = heroName;
        SavingLogsStrategy = savingLogsStrategy;
    }
    
    public List<string> GetAllLogs()
    {
        return SavingLogsStrategy.Load().Reverse().ToList();
    }
    
    //with logType argument saves a specified log and prints logs
    //with no arguments only print logs
    public void Log(LogType? logType = null, string[]? context = null)
    {
        if (logType != null)
        {
            string message = getMessage(logType.Value, context);
            SavingLogsStrategy.Save($"{DateTime.Now.ToString()} : {message}");
        }
        
        if (renderType)
        {
            Render.RenderAllLogs(GetAllLogs());
        }
        else
        {
            Render.RenderRecentLogs(GetAllLogs().Take(RecentLogsCount).ToList());
        }
    }
    
    public bool renderType = false; //false -> render recent logs, true -> render all logs

    public string GetSavePath()
    {
        return SavingLogsStrategy.SavePath;
    }

    private static int RecentLogsCount = 3;
    
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
            LogType.DefeatedHero => LogTexts.DefeatedHero(HeroName, context != null ? context[0] : "unknown hero"),
            _ => "unknown event"
        };
    }
}


