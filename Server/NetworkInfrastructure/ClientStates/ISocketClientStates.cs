using System.Net.Sockets;

namespace ConsoleApp1.Server.ClientStates;

public interface ISocketClientStates
{
    public TcpClient? GetTcpClient(int id);
    public bool IsClientInitialised(int id);
    public void InitialiseClientGame(int id);

}