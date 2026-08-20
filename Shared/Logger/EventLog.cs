using ConsoleApp1.Shared.Logger;

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
    public void Log(string context = null)
    {
            string message = context;
            SavingLogsStrategy.Save($"{DateTime.Now.ToString()} : {message}");
     }
    
    public string GetSavePath()
    {
        return SavingLogsStrategy.SavePath;
    }
}