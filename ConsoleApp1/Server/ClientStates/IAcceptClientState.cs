using System.Net.Sockets;

namespace ConsoleApp1.Server.ClientStates;

public interface IAcceptClientState
{
    public int Connect(TcpClient client);
    public void Disconnect(int id);
}