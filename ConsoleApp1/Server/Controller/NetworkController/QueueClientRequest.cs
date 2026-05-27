using System.Collections.Concurrent;
using System.Net.Sockets;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.Controller.Command;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1.Server;

public class QueueClientRequest
{
    private BlockingCollection<IModelCommand> _modelCommands;
    private IControllerClientState _controllerClientState;
    private ModelCommandFactory _modelCommandFactory;
    private GameMap _map;
    private DungeonSoundManager _soundManager;
    
    public QueueClientRequest(GameMap map, BlockingCollection<IModelCommand> modelCommands, DungeonSoundManager manager ,IControllerClientState controllerClientState)
    {
        _map = map;
        _modelCommands = modelCommands;
        _controllerClientState = controllerClientState;
        _soundManager = manager;
        _modelCommandFactory = new ModelCommandFactory(_map, _soundManager);
    }

    public void Initialise(int id, TcpClient client, CancellationToken token)
    {
        _controllerClientState.InitClientGame(id, _map);
        _modelCommands.Add(new InitHeroCommand(id, _map, _controllerClientState), token);
    }

    public void AddCommand(int id, string type, string SerialisedClientRequest, CancellationToken token)
    {
        var command = _modelCommandFactory.GetModelCommend(id, type, SerialisedClientRequest);
        if (command == null) return;
        _modelCommands.Add(command, token);
        Console.WriteLine(_modelCommands.Count);
        Console.WriteLine($"[ClientService] Dodano polecenie do kolejki: {type}");
        
    }
    
}