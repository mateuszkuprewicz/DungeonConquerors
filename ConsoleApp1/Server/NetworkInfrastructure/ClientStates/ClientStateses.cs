using System.Net.Sockets;
using ConsoleApp1.GameState;

namespace ConsoleApp1.Server.ClientStates;

public class ClientStateses : IAcceptClientState, IControllerClientState, ISocketClientStates
{
    private (bool connected, bool hasGameInititialised, TcpClient? client, GameStateContext? context)[] _globalStates;

    public ClientStateses()
    {
        _globalStates =
            new (bool connected, bool hasGameInititialised, TcpClient? client, GameStateContext?)[ServerConsts
                .MaxConnections];
    }

    public int Connect(TcpClient client)
    {
        lock (_globalStates)
        {
            for (int i = 0; i < ServerConsts.MaxConnections; i++)
            {
                if (!_globalStates[i].connected)
                {
                    _globalStates[i].connected = true;
                    _globalStates[i].hasGameInititialised = false; 
                    _globalStates[i].client = client;
                    return i;
                }
            }
        }
        return -1;
    }

    public void Disconnect(int id)
    {
        lock (_globalStates)
        {
            _globalStates[id].connected = false;
            _globalStates[id].hasGameInititialised = false;
            _globalStates[id].client = null;
            _globalStates[id].context = null;
        }
    }

    public GameStateContext? GetClientGameContext(int id)
    {
        lock (_globalStates)
        {
            Console.WriteLine($"[DEBUG CONTEXT] Żądanie dla ID {id}. Connected: {_globalStates[id].connected}, Init: {_globalStates[id].hasGameInititialised}, ContextIsNull: {_globalStates[id].context == null}");
            if (_globalStates[id].connected == false)
                return null;
            return _globalStates[id].context!;
        }
    }

    public bool InitClientGame(int id, GameMap map)
    {
        lock (_globalStates)
        {
            if (_globalStates[id].connected == false)
                return false;
            _globalStates[id].hasGameInititialised = true;
            _globalStates[id].context = new GameStateContext(map);
            return true;
        }
    }

    public TcpClient? GetTcpClient(int id)
    {
        lock (_globalStates)
        {
            if (_globalStates[id].connected == false || _globalStates[id].client == null)
                return null;
            return _globalStates[id].client!;
        }
    }

    public bool IsClientInitialised(int id)
    {
        lock (_globalStates)
        {
            return _globalStates[id].hasGameInititialised;
        }
    }

    public void InitialiseClientGame(int id)
    {
        lock (_globalStates)
        {
            _globalStates[id].hasGameInititialised = true;
        }
    }
    
}