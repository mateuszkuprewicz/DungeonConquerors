using System;
using System.Net.Sockets;
using System.Text;
using ConsoleApp1.Client.NetworkInfrastructure;
using ConsoleApp1.NetworkController;
using ConsoleApp1.KeyController;
using ConsoleApp1.Logger;
using ConsoleApp1.Shared.ShallowModel;
using ConsoleApp1.View;

namespace ConsoleApp1.Client
{
    class Program
    {
        private string ServerIp;
        private int ServerPort = 8080;

        public async Task Run(string ip, int port)
        {
            Shared.ShallowModel.GameState gameState = new Shared.ShallowModel.GameState();
            Render render = new Render(gameState);
            LogRenderer logRenderer = new LogRenderer();
            DeserialisingDtoFactory deserialisingDtoFactory = new DeserialisingDtoFactory(gameState, render, logRenderer);
            
            ServerIp = ip;
            ServerPort = port;
            
            Console.Title = "Dungeon Crawler Client";
            Console.WriteLine("--- Łączenie z serwerem ---");

            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(ServerIp, ServerPort);
                KeyController.KeyController keyController = new KeyController.KeyController(client, render, logRenderer, gameState);
                Reader reader = new Reader(client, deserialisingDtoFactory);
                List<Task> tasks = new List<Task>();
                tasks.Add(reader.ReadLoop());
                tasks.Add(keyController.Run());
                await Task.WhenAll(tasks);
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
    }
}