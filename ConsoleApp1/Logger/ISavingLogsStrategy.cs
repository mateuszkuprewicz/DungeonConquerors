using System.IO;
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
        File.Create(SavePath).Close();
    }
    
    public void Save(string message)
    {
        using StreamWriter sw = new StreamWriter(SavePath, true);
        sw.WriteLine(message);
    }

    public IEnumerable<string> Load()
    {
        using StreamReader sr = new StreamReader(SavePath);
        while (sr.ReadLine() is { } line)
        {
            yield return line;
        }
    }

}