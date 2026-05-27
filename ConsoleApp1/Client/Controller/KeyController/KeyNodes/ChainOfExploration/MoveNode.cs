using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.View;

namespace ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.Logger;

public class MoveNode : AbstractKeyNode
{
    private TcpClient _client;
    private Shared.ShallowModel.GameState _state;
    public MoveNode(TcpClient client, Shared.ShallowModel.GameState state) => (_client, _state) = (client, state); 
    
    public override async Task HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == KeyConsts.MoveUp.key || keyInfo == KeyConsts.MoveDown.key || keyInfo == KeyConsts.MoveLeft.key ||
            keyInfo == KeyConsts.MoveRight.key)
        {
            string type = ClientRequestsTypes.ClientMove;
            ClientMove move = new ClientMove();
            
            if(_state.Map == null) return;
            move.Id = _state.Map.PlayerId;
            Console.Error.WriteLine(_state.Map.PlayerId);
            
            Direction direction;
            if(keyInfo == KeyConsts.MoveUp.key)
                direction = Direction.Up;
            else if(keyInfo == KeyConsts.MoveDown.key)
                direction = Direction.Down;
            else if(keyInfo == KeyConsts.MoveLeft.key)
                direction = Direction.Left;
            else
                direction = Direction.Right;
            move.Direction = direction;

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = false;
            
            string serialized = JsonSerializer.Serialize(move);
            string payload = $"{type}|{serialized}\n";
            byte[] data = Encoding.UTF8.GetBytes(payload);
            
            var writer = _client.GetStream();
            await writer.WriteAsync(data);
            await writer.FlushAsync();
            
        }
        else
        {
            NextKeyNode.HandleKey(keyInfo);
        }
    }
}