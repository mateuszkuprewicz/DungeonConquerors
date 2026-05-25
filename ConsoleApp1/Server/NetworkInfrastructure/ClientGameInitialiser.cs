using System.Collections.Concurrent;
using System.Net.Sockets;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.Controller.Command;

namespace ConsoleApp1.Server;

public class ClientGameInitialiser
{
    private TcpClient _client;
    //Queue Initialiser
    private BlockingCollection<IModelCommand> _modelCommands;
    private IControllerClientState _controllerClientState;
    private GameMap _map;
    
    public ClientGameInitialiser(GameMap map, BlockingCollection<IModelCommand> modelCommands, IControllerClientState controllerClientState)
    {
        _map = map;
        _client = new TcpClient();
        _modelCommands = modelCommands;
        _controllerClientState = controllerClientState;
    }

    public void Initialise(int id, TcpClient client, CancellationToken token)
    {
        _modelCommands.Add(new InitHeroCommand(id, _map, _controllerClientState), token);
    }
    
}