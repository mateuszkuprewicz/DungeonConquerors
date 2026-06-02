using ConsoleApp1.Logger;

namespace ConsoleApp1.View;

public class LogRenderer
{
    private static readonly (int, int) LogsTableStart = (78, 10); 
    private const int MaxVisibleLogLines = 6;
    private const int LastLogsNum = 4;
    
    public bool IsRenderingAllLogs { get; set; }

    private Lock _renderLock;
    
    public LogRenderer(Lock rLock)
    {
        _renderLock = rLock;
        IsRenderingAllLogs = false;
    }

    public void RenderLast()
    {
        var logger = EventLog.GetEventLog();
        List<string> logFileContent = logger.GetAllLogs().ToList();
        
        int skipCount = Math.Max(0, logFileContent.Count - LastLogsNum);
        List<string> recentLogs = logFileContent.Skip(skipCount).ToList();
        
        lock (_renderLock)
        {
            List<string> linesToPrint = GetWrappedLines(recentLogs);
            int maxWidth = Console.WindowWidth - LogsTableStart.Item1 - 1;
            
            if (maxWidth <= 0) return;

            Console.SetCursorPosition(LogsTableStart.Item1, LogsTableStart.Item2);
            Console.Write("=== OSTATNIE ZDARZENIA ===".PadRight(maxWidth));
            Console.ResetColor();

            int count = Math.Min(linesToPrint.Count, MaxVisibleLogLines - 1);
            for (int i = 0; i < MaxVisibleLogLines - 1; i++)
            {
                Console.SetCursorPosition(LogsTableStart.Item1, LogsTableStart.Item2 + 1 + i);
                if (i < count)
                {
                    Console.Write(linesToPrint[i].PadRight(maxWidth));
                }
                else
                {
                    Console.Write(new string(' ', maxWidth));
                }
            }
            
            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }

    public void RenderAll()
    {
        var logger = EventLog.GetEventLog();
        List<string> allLogs = logger.GetAllLogs().ToList();
        
        lock (_renderLock)
        {
            int h = 1;
            if(Console.BufferHeight < allLogs.Count + h)
                Console.BufferHeight = allLogs.Count + h;
            
            Console.Clear();
            Console.Write("=== WSZYSTKIE ZDARZENIA ===");
            
            foreach (var log in allLogs)
            {
                Console.SetCursorPosition(1, h);
                Console.WriteLine(log);
                h++;
            }
        }

        IsRenderingAllLogs = true;
    }
    
    private static List<string> GetWrappedLines(List<string> logs)
    {
        int maxWidth = Console.WindowWidth - LogsTableStart.Item1 - 1;
        if (maxWidth <= 0) return new List<string>();

        List<string> wrappedLines = new List<string>();

        foreach (var log in logs)
        {
            if (string.IsNullOrEmpty(log)) continue;

            string[] words = log.Split(' ');
            string currentLine = "";

            foreach (var word in words)
            {
                if ((currentLine + word).Length > maxWidth)
                {
                    if (!string.IsNullOrEmpty(currentLine))
                        wrappedLines.Add(currentLine.TrimEnd());

                    string tempWord = word;
                    while (tempWord.Length > maxWidth)
                    {
                        wrappedLines.Add(tempWord.Substring(0, maxWidth));
                        tempWord = tempWord.Substring(maxWidth);
                    }
                    currentLine = tempWord + " ";
                }
                else
                {
                    currentLine += word + " ";
                }
            }
            if (!string.IsNullOrEmpty(currentLine))
                wrappedLines.Add(currentLine.TrimEnd());
        }
        return wrappedLines;
    }
}