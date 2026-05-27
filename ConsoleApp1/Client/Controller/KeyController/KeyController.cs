using System.Net.Sockets;
using ConsoleApp1.ChainOfKeyOperations;

namespace ConsoleApp1.KeyController;

public class KeyController
{
    private AbstractKeyNode _root;
    private TcpClient _client;
    private Render _render;
    private Shared.ShallowModel.GameState _state;

    public KeyController(TcpClient client, Render render, Shared.ShallowModel.GameState state)
    {
        _client = client;
        _render = render;
        _state = state;

        _root = new EquipmentScrollNode(render);
        var _moveNode = new MoveNode(client, state);
        var sentinel = new Sentinel();
        _root.SetNextHandler(_moveNode);
        _moveNode.SetNextHandler(sentinel);
    }

    public async Task Run()
    {
        while (true)
        {
            var keyInfo = Console.ReadKey(intercept: true).Key; 
            HandleKey(keyInfo);
        }
    }
    
    private void HandleKey(ConsoleKey key)
    {
        _root.HandleKey(key);
    }
}