using System.IO;
namespace ConsoleApp1.Logger;

public interface ISavingLogsStrategy
{
    public void Save(string  message);
    public IEnumerable<string> Load();
    public string SavePath { get; }
}

public class SavingLogs : ISavingLogsStrategy
{
    public string SavePath { get; private set; }

    public SavingLogs(string savePath, string heroName)
    {
        string fileName = $"{heroName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
        SavePath = Path.Combine(savePath, fileName);
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