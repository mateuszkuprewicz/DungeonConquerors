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

            string serialized = JsonSerializer.Serialize(equip);
            string payload = $"{type}|{serialized}\n";
            byte[] data = Encoding.UTF8.GetBytes(payload);
            
            var writer = _client.GetStream();
            await writer.WriteAsync(data);
            await writer.FlushAsync();
        }
        else if (keyInfo == KeyConsts.UnequipWeapon.key)
        {
            string type = ClientRequestsTypes.ClientUnequip;
            ClientUnequip unequip = new ClientUnequip();

            string serialized = JsonSerializer.Serialize(unequip);
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