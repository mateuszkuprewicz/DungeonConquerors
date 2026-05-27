using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using ConsoleApp1.Server.View.ViewCommand;

public class ClientView : IDisposable
{
    public int Id { get; }
    
    private readonly BlockingCollection<IViewCommand> _outbox = new();
    private readonly CancellationTokenSource _cts = new();
    private readonly TcpClient _client;

    public ClientView(int id, TcpClient client)
    {
        Id = id;
        _client = client;
        Task.Run(() => SendLoop(_cts.Token));
    }

    public void Enqueue(IViewCommand cmd) => _outbox.Add(cmd);

    private async Task SendLoop(CancellationToken token)
    {
        try
        {
            using var stream = _client.GetStream();
            foreach (var cmd in _outbox.GetConsumingEnumerable(token))
            {
                if (!_client.Connected) break;
                await SendToClient(cmd);
            }
        }
        catch (OperationCanceledException) {}
    }

    public void Dispose()
    {
        _cts.Cancel();
        _outbox.CompleteAdding();
    }
    
    private async Task SendToClient(IViewCommand command)
    {
        if (_client.Connected)
        {
            string payload = $"{command.Type}|{command.Text}\n";
            byte[] data = Encoding.UTF8.GetBytes(payload);

            var stream = _client.GetStream();
            await stream.WriteAsync(data, 0, data.Length, _cts.Token);
            await stream.FlushAsync(_cts.Token); 
            
            Console.WriteLine($"[ViewWriter] Wysłano {command.Type} do gracza {Id}.");
        }
    }
}