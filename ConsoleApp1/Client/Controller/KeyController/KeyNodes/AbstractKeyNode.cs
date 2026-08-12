using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ConsoleApp1.ChainOfKeyOperations;

public abstract class AbstractKeyNode
{   
    protected AbstractKeyNode NextKeyNode;
    

    public void SetNextHandler(AbstractKeyNode next)
    {
        NextKeyNode =  next;
    }
    
    protected async Task SendMessageAsync<T>(TcpClient client, string type, T requestObject)
    {
        var options = new JsonSerializerOptions { WriteIndented = false };
        string serialized = JsonSerializer.Serialize(requestObject, options);
        string payload = $"{type}|{serialized}\n";
        byte[] data = Encoding.UTF8.GetBytes(payload);
    
        var stream = client.GetStream();
        await stream.WriteAsync(data);
        await stream.FlushAsync();
    }
    
    public abstract Task HandleKey(ConsoleKey keyInfo);
}
