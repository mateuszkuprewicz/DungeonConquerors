namespace ConsoleApp1.Logger;

public interface ISavingLogsStrategy
{
    public void Save(string  message);
    public IEnumerable<string> Load();
}

public class SavingLogs : ISavingLogsStrategy
{
    private string SavePath;

    public SavingLogs(string savePath)
    {
        SavePath = savePath;
    }
    
    public void Save(string message)
    {
        
    }

    public IEnumerable<string> Load()
    {
        
    }

}