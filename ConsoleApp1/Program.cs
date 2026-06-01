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

        string mode = "";
        string targetAddress = ""; // Dla serwera to sam port, dla klienta ip:port
        bool isDetached = args.Contains("--detached");

        // --- 1. SPRAWDZANIE ARGUMENTÓW LUB MENU INTERAKTYWNE ---
        // Jeśli przekazano argumenty (i pierwszy z nich to nie flaga --detached), parsowanie z wiersza poleceń
        if (args.Length > 0 && args[0].ToLower() != "--detached")
        {
            if (args[0].ToLower() == "--help")
            {
                PrintHelp();
                return;
            }
            mode = args[0].ToLower();
            if (args.Length > 1)
            {
                // Zakładamy, że format to np. `--client 192.168.1.1:5555 --detached`
                // adres to wtedy drugi element tablicy args[1]
                targetAddress = args[1];
            }
        }
        else if (!isDetached)
        {
            // Tryb interaktywny (brak argumentów startowych)
            Console.WriteLine("Wybierz tryb uruchomienia:");
            Console.WriteLine("1. Serwer");
            Console.WriteLine("2. Klient");
            Console.Write("Twój wybór (1/2): ");
            
            string choice = Console.ReadLine()?.Trim();

            if (choice == "1")
            {
                mode = "--server";
                Console.Write($"Podaj port (wciśnij Enter dla domyślnego {defaultPort}): ");
                string portInput = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(portInput))
                    targetAddress = portInput;
            }
            else if (choice == "2")
            {
                mode = "--client";
                Console.Write($"Podaj adres IP:PORT (wciśnij Enter dla domyślnego {defaultIp}:{defaultPort}): ");
                string ipInput = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(ipInput))
                    targetAddress = ipInput;
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór. Zamykanie programu.");
                return;
            }
        }

        // --- 2. AUTOMATYCZNE OTWIERANIE NOWEGO OKNA DLA KLIENTA ---
        if (mode == "--client" && !isDetached)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Budujemy argumenty dla nowego okna
                string passArguments = $"--client {targetAddress} --detached";
                
                // Pobieramy ścieżkę do obecnie uruchomionego pliku .exe, zamiast polegać na 'dotnet run'
                string currentExecutable = Environment.ProcessPath;

                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    // Używamy /k żeby okno się nie zamykało przy błędach, 
                    // a ścieżkę .exe otaczamy cudzysłowami na wypadek spacji w nazwie folderu
                    Arguments = $"/k \"\"{currentExecutable}\" {passArguments}\"",
                    CreateNoWindow = false,
                    UseShellExecute = true
                };

                Process.Start(startInfo);
                
                // Kończymy działanie w obecnej konsoli – klient żyje już w nowym oknie
                return; 
            }
        }

        // --- 3. GŁÓWNA LOGIKA URUCHAMIANIA ---
        try
        {
            if (mode == "--server")
            {
                // Parsowanie portu serwera
                int port = defaultPort;
                if (!string.IsNullOrEmpty(targetAddress) && targetAddress != "--detached")
                {
                    if (!int.TryParse(targetAddress, out port))
                    {
                        Console.WriteLine("Błędny format portu. Używam domyślnego.");
                        port = defaultPort;
                    }
                }
                
                Console.WriteLine($"[BOOT] Uruchamianie SERWERA na porcie {port}...");
                var server = new Server.ServerProgram();
                await server.Run(port);
            }
            else if (mode == "--client")
            {
                // Parsowanie ip:port klienta
                string ip = defaultIp;
                int port = defaultPort;

                if (!string.IsNullOrEmpty(targetAddress) && targetAddress != "--detached")
                {
                    var parts = targetAddress.Split(':');
                    ip = parts[0];
                    if (parts.Length > 1 && int.TryParse(parts[1], out int parsedPort))
                    {
                        port = parsedPort;
                    }
                }

                Console.WriteLine($"[BOOT] Uruchamianie KLIENTA łączącego się z {ip}:{port}...");
                var client = new Client.Program();
                await client.Run(ip, port);
            }
            else
            {
                Console.WriteLine("Nieznany tryb działania.");
                PrintHelp();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[CRITICAL ERROR] {ex.Message}");
            Console.ResetColor();
            Console.ReadLine(); // Zatrzymuje zamykanie konsoli przy błędzie krytycznym
        }
    }

    static void PrintHelp()
    {
        Console.WriteLine("\nUżycie:");
        Console.WriteLine("  Możesz uruchomić program bez argumentów, aby skorzystać z menu interaktywnego.");
        Console.WriteLine("  --server [port]              - uruchamia serwer (domyślnie 5555)");
        Console.WriteLine("  --client [ip:port]           - uruchamia klienta (domyślnie 127.0.0.1:5555)");
        Console.WriteLine("\nPrzykłady:");
        Console.WriteLine("  dotnet run -- --server 8080");
        Console.WriteLine("  dotnet run -- --client 192.168.1.10:8080");
    }
}