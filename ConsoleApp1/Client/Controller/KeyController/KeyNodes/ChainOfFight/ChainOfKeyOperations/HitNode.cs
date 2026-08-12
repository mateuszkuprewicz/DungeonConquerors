using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.View;

namespace ConsoleApp1.Client.Controller.KeyController.KeyNodes.ChainOfFight.ChainOfKeyOperations;

public class HitNode : AbstractKeyNode
{
    private TcpClient _client;

    public HitNode(TcpClient client)
    {
        _client = client;
    }

    public override async Task HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == KeyConsts.Hit.key)
        {
            string type = ClientRequestsTypes.ClientHit;
            ClientHit clientHit = new ClientHit();

            lock (Render.ConsoleLock)
            {
                for (int i = Render.Instruction.Item2; i < Render.DefaultCursorPosition.Item2; i++)
                {
                    Console.SetCursorPosition(Render.Instruction.Item1, i);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                Console.SetCursorPosition(Render.Instruction.Item1, Render.Instruction.Item2);
                Console.Write($"{KeyConsts.NormalAttack.letter} - normal, {KeyConsts.StealthAttack.letter} - stealth, {KeyConsts.MagicAttack.letter} - magic.");
                Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
            }

            while (true)
            {
                
                var attackType = Console.ReadKey(true);

                HitType? temp_type = attackType.Key switch
                {
                    ConsoleKey.D1 => HitType.HeavyAttack,
                    ConsoleKey.D2 => HitType.SneakyAttack,
                    ConsoleKey.D3 => HitType.MagicAttack,
                    _ => null
                };

                if (temp_type != null)
                {
                    clientHit.Type = temp_type.Value;
                    break;
                }
            }
            
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = false;
            string serialized = JsonSerializer.Serialize(clientHit, options);
            string payload = $"{type}|{serialized}\n";
            byte[] data = Encoding.UTF8.GetBytes(payload);
            
            var writer = _client.GetStream();
            await writer.WriteAsync(data);
            await writer.FlushAsync();
        }
        else
        {
            if (NextKeyNode != null)
                await NextKeyNode.HandleKey(keyInfo);
        }
    }
}