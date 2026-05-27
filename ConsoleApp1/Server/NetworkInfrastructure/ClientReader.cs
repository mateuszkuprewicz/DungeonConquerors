using System.Net.Sockets;
using System.Text;

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
        using var stream = _client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        try
        {
            while (!token.IsCancellationRequested && _client.Connected)
            {
                string? line = await reader.ReadLineAsync(token);
                if (line == null)
                {
                    break; 
                }
                
                var parts = line.Split('|', 2);
                if (parts.Length == 2)
                {
                    string type = parts[0];
                    string serialisedClientObject = parts[1];

                    _queuer.AddCommand(_id, type, serialisedClientObject, token);
                    Console.WriteLine($"[ClientService] wywołano QueueClientRequest.AddCommand: {type}");
                }

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