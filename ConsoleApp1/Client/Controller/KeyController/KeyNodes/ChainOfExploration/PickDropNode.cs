using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.View;

namespace ConsoleApp1.ChainOfKeyOperations;

public class PickDropNode : AbstractKeyNode
{
    private TcpClient _client;
    private Shared.ShallowModel.GameState _state;

    public PickDropNode(TcpClient client, Shared.ShallowModel.GameState state) 
    {
        _client = client;
        _state = state;
    }

    public override async Task HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == KeyConsts.PickItem.key)
        {
            string type = ClientRequestsTypes.ClientPickUp;
            ClientPickUp pickUp = new ClientPickUp();

            string serialized = JsonSerializer.Serialize(pickUp);
            string payload = $"{type}|{serialized}\n";
            byte[] data = Encoding.UTF8.GetBytes(payload);
            
            var writer = _client.GetStream();
            await writer.WriteAsync(data);
            await writer.FlushAsync();
        }
        
        else if (keyInfo == KeyConsts.DropItem.key)
        {
            string type = ClientRequestsTypes.ClientDrop;
            ClientDrop drop = new ClientDrop();
            
            drop.ItemNumber = _state.Hero.Equipment.EquipmentPointer; 

            string serialized = JsonSerializer.Serialize(drop);
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