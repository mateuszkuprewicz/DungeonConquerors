using System;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1.Client
{
    class Program
    {
        private string ServerIp;
        private  int ServerPort = 8080;

        public async Task Run(string ip, int port)
        {
            ServerIp = ip;
            ServerPort = port;
            
            Console.Title = "Dungeon Crawler Client";
            Console.WriteLine("--- Łączenie z serwerem ---");

            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(ServerIp, ServerPort);
                
                using var stream = client.GetStream();
                using var reader = new StreamReader(stream, Encoding.UTF8);
                using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                Console.WriteLine("Połączono! Oczekiwanie na dane mapy...");

                // Wątek odbierający dane
                while (client.Connected)
                {
                    // Czytamy linię aż do napotkania '\n'
                    string? line = await reader.ReadLineAsync();
                    if (line == null) break; // Serwer zamknął połączenie

                    HandleServerMessage(line);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nBłąd: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nRozłączono. Naciśnij dowolny klawisz, aby zamknąć...");
            Console.ReadKey();
        }

        private static void HandleServerMessage(string rawMessage)
        {
            // Format: Typ|Tekst (np: 0|{"TestMapMessage": "..."})
            var parts = rawMessage.Split('|', 2);
            if (parts.Length < 2) return;

            int type = int.Parse(parts[0]);
            string payload = parts[1];

            switch (type)
            {
                case 0: // sendMap (ViewCommandType.sendMap)
                    RenderMap(payload);
                    break;
                case 1: // playerCreation
                    Console.WriteLine($"[INFO] Nowy gracz dołączył: {payload}");
                    break;
                default:
                    Console.WriteLine($"[UNKNOWN] Typ: {type}, Dane: {payload}");
                    break;
            }
        }

        private static void RenderMap(string jsonMap)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== OTRZYMANO MAPĘ Z SERWERA ===");
            Console.ResetColor();
            
            // Na razie wypisujemy surowy JSON, aby sprawdzić czy rura działa.
            // Gdy dopiszesz DTO, tutaj zrobisz: var map = JsonSerializer.Deserialize<ShallowMap>(jsonMap);
            Console.WriteLine(jsonMap);
            
            Console.WriteLine("\n================================");
            Console.WriteLine("System: Oczekiwanie na ruch...");
        }
    }
}