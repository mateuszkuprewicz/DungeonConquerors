using System.Text.Json.Nodes;
using System.IO;

namespace ConsoleApp1.ConfigurationFile;

public class ConfigManager
{
    private readonly string _configPath;

    public ConfigManager(string configPath)
    {
        _configPath = configPath;
    }

    private JsonNode GetRoot()
    {
        if (!File.Exists(_configPath))
            throw new FileNotFoundException("Brak pliku konfiguracyjnego!");

        string jsonString = File.ReadAllText(_configPath);
        return JsonNode.Parse(jsonString)!;
    }

    public string GetHeroName()
    {
        var root = GetRoot();
        return root["PlayerName"]?.ToString() ?? "Unknown Hero";
    }

    public string GetLogPath()
    {
        var root = GetRoot();
        return root["LogSavePath"]?.ToString() ?? "./default_logs/";
    }
}