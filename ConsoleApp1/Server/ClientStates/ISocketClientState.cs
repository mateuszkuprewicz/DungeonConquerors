using System.Net.Sockets;

namespace ConsoleApp1.Server.ClientStates;

public interface ISocketClientState
{
    public TcpClient? GetTcpClient(int id);
    public bool IsClientInitialised(int id);
}