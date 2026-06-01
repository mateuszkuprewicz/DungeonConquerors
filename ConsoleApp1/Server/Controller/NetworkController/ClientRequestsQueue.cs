using System.Collections.Concurrent;
using System.Net.Sockets;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.Controller.Command;
using ConsoleApp1.Server.Model;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1.Server;

public class ClientRequestsQueue
{
    private BlockingCollection<IModelCommand> _modelCommands;
    private ModelCommandFactory _modelCommandFactory;
    
    public ClientRequestsQueue(GameContext gameContext, ModelCommandFactory modelCommandFactory, BlockingCollection<IModelCommand> modelCommands)
    {
        _modelCommandFactory = modelCommandFactory;
        _modelCommands = modelCommands;
    }

    public void Initialise(int id, CancellationToken token)
    {
        _modelCommands.Add(_modelCommandFactory.GetInit(id), token);
        
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