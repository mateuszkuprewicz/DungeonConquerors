using System.Net.Sockets;
using ConsoleApp1.GameState;

namespace ConsoleApp1.Server.ClientStates;

public class ClientStates : IAcceptClientState, ISocketClientStates
{
    private (bool connected, bool hasGameInititialised, TcpClient? client)[] _globalStates;

    public ClientStates()
    {
        _globalStates =
            new (bool connected, bool hasGameInititialised, TcpClient? client)[ServerConsts
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
                    return i + 1;
                }
            }
        }
        return -1;
    }

    public void Disconnect(int id)
    {
        id--;
        lock (_globalStates)
        {
            _globalStates[id].connected = false;
            _globalStates[id].hasGameInititialised = false;
            _globalStates[id].client = null;
        }
    }
    
    public TcpClient? GetTcpClient(int id)
    {
        id--;
        lock (_globalStates)
        {
            if (_globalStates[id].connected == false || _globalStates[id].client == null)
                return null;
            return _globalStates[id].client!;
        }
    }

    public bool IsClientInitialised(int id)
    {
        id--;
        lock (_globalStates)
        {
            return _globalStates[id].hasGameInititialised;
        }
    }

    public void InitialiseClientGame(int id)
    {
        id--;
        lock (_globalStates)
        {
            _globalStates[id].hasGameInititialised = true;
        }
    }
    
}