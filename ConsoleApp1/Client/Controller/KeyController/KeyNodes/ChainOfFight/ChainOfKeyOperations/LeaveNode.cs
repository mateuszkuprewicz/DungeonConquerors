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
            await SendMessageAsync(_client, ClientRequestsTypes.ClientRunAway, new ClientRunAway());
        }
        else
        {
            if (NextKeyNode != null)
                await NextKeyNode.HandleKey(keyInfo);
        }
    }
}