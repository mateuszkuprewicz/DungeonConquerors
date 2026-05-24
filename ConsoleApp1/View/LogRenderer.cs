using ConsoleApp1.Logger;

namespace ConsoleApp1.View;

public class LogRenderer
{
    private static readonly (int, int) LogsTableStart = (78, 10); 
    private const int MaxVisibleLogLines = 6;
    private const int LastLogsNum = 4;
    public EventLog LogSource { get; init; }
    
    public bool IsRenderingAllLogs { get; set; }

    private Lock _renderLock;
    
    public LogRenderer(EventLog logSource, Lock rLock)
    {
        LogSource = logSource;
        _renderLock = rLock;
        IsRenderingAllLogs = false;
    }

    public void RenderLast()
    {
        List<string> logFileContent = LogSource.GetAllLogs().ToList();
        
        List<string> recentLogs = logFileContent.TakeLast(LastLogsNum).ToList();
        
        lock (_renderLock)
        {
            ClearLogArea();

            List<string> linesToPrint = GetWrappedLines(recentLogs);
        
            Console.SetCursorPosition(LogsTableStart.Item1, LogsTableStart.Item2);
            Console.Write("=== OSTATNIE ZDARZENIA ===");
            Console.ResetColor();

            int count = Math.Min(linesToPrint.Count, MaxVisibleLogLines - 1);
            for (int i = 0; i < count; i++)
            {
                Console.SetCursorPosition(LogsTableStart.Item1, LogsTableStart.Item2 + 1 + i);
                Console.Write(linesToPrint[i]);
            }
            
            Console.SetCursorPosition(RenderConsts.DefaultCursorPosition.Item1, RenderConsts.DefaultCursorPosition.Item2);
        }
    }

    public void RenderAll()
    {
        List<string> allLogs = LogSource.GetAllLogs().ToList();
        
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
    
    private static void ClearLogArea()
    {
        for (int i = 0; i < MaxVisibleLogLines; i++)
        {
            Console.SetCursorPosition(LogsTableStart.Item1, LogsTableStart.Item2 + i);
            int spacesToClear = Math.Max(0, Console.WindowWidth - LogsTableStart.Item1 - 1);
            Console.Write(new string(' ', spacesToClear));
        }
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
                // Jeśli słowo + spacja nie mieszczą się w linii
                if ((currentLine + word).Length > maxWidth)
                {
                    if (!string.IsNullOrEmpty(currentLine))
                        wrappedLines.Add(currentLine.TrimEnd());

                    // Jeśli samo słowo jest szersze niż cała kolumna (bardzo długie nazwy)
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