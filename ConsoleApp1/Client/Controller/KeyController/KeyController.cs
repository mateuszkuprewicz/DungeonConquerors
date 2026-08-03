using System.Net.Sockets;
using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.Client.Controller.KeyController.KeyNodes.ChainOfFight.ChainOfKeyOperations;
using ConsoleApp1.View;

namespace ConsoleApp1.KeyController;

public class KeyController
{
    private AbstractKeyNode _root;
    private Shared.ShallowModel.GameState _state;

    public KeyController(TcpClient client, Render render, LogRenderer logRenderer, Shared.ShallowModel.GameState state)
    {
        _state = state;

        _root = new EquipmentScrollNode(render, _state);
        var _moveNode = new MoveNode(client, state);
        var pickUpNode = new PickDropNode(client, state);
        var equipNode = new WeaponEquipmentNode(client, state);
        var logChangeNode = new LogChangeViewNode(logRenderer, render);
        var hitNode = new HitNode(client, state, render);
        var sentinel = new Sentinel();
        _root.SetNextHandler(_moveNode);
        _moveNode.SetNextHandler(pickUpNode);
        pickUpNode.SetNextHandler(equipNode);
        equipNode.SetNextHandler(logChangeNode);
        logChangeNode.SetNextHandler(hitNode);
        hitNode.SetNextHandler(sentinel);
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