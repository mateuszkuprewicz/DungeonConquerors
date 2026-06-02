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
    private Shared.ShallowModel.GameState _state;
    private Render _render;

    public HitNode(TcpClient client, Shared.ShallowModel.GameState state, Render render)
    {
        _client = client;
        _state = state;
        _render = render;
    }

    public override async Task HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == KeyConsts.Hit.key)
        {
            string type = ClientRequestsTypes.ClientHit;
            ClientHit clientHit = new ClientHit();

            InstructionRender instructionRender = new InstructionRender();
            instructionRender.PrintAttackInstruction();
            
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
                    instructionRender.Clear();
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
    }
    
}