using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1.Client.NetworkInfrastructure;
using ConsoleApp1.NetworkController;
public class Reader
{
    private TcpClient _client;
    private DeserialisingDtoFactory _deserialisingDtoFactory;

    public Reader(TcpClient client, DeserialisingDtoFactory deserialisingDtoFactory)
    {
        _client = client;
        _deserialisingDtoFactory = deserialisingDtoFactory;
    }

    public async Task ReadLoop()
    {
        using var stream = _client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        while (_client.Connected)
        {
            string? line = await reader.ReadLineAsync();
            if (line == null) break; 
            
            HandleServerMessage(line);
        }
    }
    
    private void HandleServerMessage(string rawMessage)
    {
        // Format: Typ|Tekst (np: 0|{"TestMapMessage": "..."})
        var parts = rawMessage.Split('|', 2);
        if (parts.Length < 2) return;

        string type = parts[0];
        string payload = parts[1];
        
        var command = _deserialisingDtoFactory.GetHandler(type, payload);
        if (command == null) throw new Exception("Command not found");
        
        command.Handle();
    }
}