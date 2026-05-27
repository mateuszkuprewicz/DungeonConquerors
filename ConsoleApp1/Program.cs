using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ConsoleApp1;

class Program
{
    static async Task Main(string[] args)
    {
        // Domyślne wartości
        int defaultPort = 5555;
        string defaultIp = "127.0.0.1";

        if (args.Length == 0 || args[0].ToLower() == "--help")
        {
            PrintHelp();
            return;
        }

        string mode = args[0].ToLower();

        // --- AUTOMATYCZNE OTWIERANIE NOWEGO OKNA DLA KLIENTA ---
        if (mode == "--client" && !args.Contains("--detached"))
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Rekonstruujemy dokładnie te same argumenty, z którymi wywołano program,
                // dorzucając flagę bezpieczeństwa --detached na koniec
                string passArguments = string.Join(" ", args) + " --detached";

                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    // /c odpala okno, wykonuje komendę i po jej zamknięciu przez gracza zwalnia cmd
                    Arguments = $"/c dotnet run -- {passArguments}",
                    CreateNoWindow = false,
                    UseShellExecute = true
                };

                Process.Start(startInfo);
                
                // Kończymy działanie w obecnej konsoli – klient żyje już w nowym oknie
                return; 
            }
        }

        try
        {
            if (mode == "--server")
            {
                // Parsowanie portu serwera
                int port = args.Length > 1 ? int.Parse(args[1]) : defaultPort;
                
                Console.WriteLine($"[BOOT] Uruchamianie SERWERA na porcie {port}...");
                var server = new Server.ServerProgram();
                await server.Run(port);
            }
            else if (mode == "--client")
            {
                // Parsowanie ip:port klienta
                string ip = defaultIp;
                int port = defaultPort;

                if (args.Length > 1)
                {
                    var parts = args[1].Split(':');
                    ip = parts[0];
                    if (parts.Length > 1) port = int.Parse(parts[1]);
                }

                Console.WriteLine($"[BOOT] Uruchamianie KLIENTA łączącego się z {ip}:{port}...");
                var client = new Client.Program();
                await client.Run(ip, port);
            }
            else
            {
                Console.WriteLine("Nieznany argument.");
                PrintHelp();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[CRITICAL ERROR] {ex.Message}");
            Console.ResetColor();
        }
    }

    static void PrintHelp()
    {
        Console.WriteLine("\nUżycie:");
        Console.WriteLine("  --server [port]              - uruchamia serwer (domyślnie 5555)");
        Console.WriteLine("  --client [ip:port]           - uruchamia klienta (domyślnie 127.0.0.1:5555)");
        Console.WriteLine("\nPrzykłady:");
        Console.WriteLine("  dotnet run -- --server 8080");
        Console.WriteLine("  dotnet run -- --client 192.168.1.10:8080");
    }
}