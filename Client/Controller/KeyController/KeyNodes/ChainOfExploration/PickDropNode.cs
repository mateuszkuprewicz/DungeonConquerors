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
            await SendMessageAsync(_client, ClientRequestsTypes.ClientPickUp, new ClientPickUp());
        }
        
        else if (keyInfo == KeyConsts.DropItem.key)
        {
            ClientDrop drop = new ClientDrop();
            drop.ItemNumber = _state.Hero.Equipment.EquipmentPointer; 
            await SendMessageAsync(_client, ClientRequestsTypes.ClientDrop, drop);
        }
        else
        {
            NextKeyNode.HandleKey(keyInfo);
        }
    }
}