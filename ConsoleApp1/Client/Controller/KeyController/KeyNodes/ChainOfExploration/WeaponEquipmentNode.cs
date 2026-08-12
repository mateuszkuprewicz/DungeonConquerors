using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.Shared.ShallowModel;
using ConsoleApp1.View;

namespace ConsoleApp1.ChainOfKeyOperations;

public class WeaponEquipmentNode : AbstractKeyNode
{
    private TcpClient _client;
    private Shared.ShallowModel.GameState _state;

    public WeaponEquipmentNode(TcpClient client, Shared.ShallowModel.GameState state) 
    {
        _client = client;
        _state = state;
    }
    
    public override async Task HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == KeyConsts.EquipWeapon.key)
        {
            string type = ClientRequestsTypes.ClientEquip;
            ClientEquip equip = new ClientEquip();
            
            if (_state.Hero?.Equipment != null)
            {
                equip.ItemNumber = _state.Hero.Equipment.EquipmentPointer;
            }
            
            await SendMessageAsync(_client, type, equip);
        }
        else if (keyInfo == KeyConsts.UnequipWeapon.key)
        {
            await SendMessageAsync(_client, ClientRequestsTypes.ClientUnequip, new ClientUnequip());
        }
        else
        {
            await NextKeyNode.HandleKey(keyInfo);
        }
    }
}