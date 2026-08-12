using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.View;

namespace ConsoleApp1.Client.Controller.KeyController.KeyNodes.ChainOfFight.ChainOfKeyOperations;

public class LeaveNode : AbstractKeyNode
{
    private TcpClient _client;

    public LeaveNode(TcpClient client)
    {
        _client = client;
    }

    public override async Task HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == KeyConsts.Leave.key)
        {
            string type = ClientRequestsTypes.ClientRunAway;
            ClientRunAway clientRunAway = new ClientRunAway();
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = false;
            string serialized = JsonSerializer.Serialize(clientRunAway, options);
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