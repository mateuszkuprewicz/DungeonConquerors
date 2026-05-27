using System.Net.Sockets;

namespace ConsoleApp1.Server;

public class ClientReader
{
    private int _id;
    private TcpClient _client;
    private QueueClientRequest _queuer;

    public ClientReader(int id, TcpClient client, QueueClientRequest queuer)
    {
        _id = id;
        _client = client;
        _queuer = queuer;
    }

    public async Task HandleCLient(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested && _client.Connected)
            {
                await Task.Delay(500, token);
                //odbieranie wiadomości

                if (_client.Client.Poll(0, SelectMode.SelectRead) && _client.Available == 0)
                {
                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            Console.WriteLine($"[ClientService] Zamykanie wątku obsługi gracza {_id}");
        }
        
    }
    
}