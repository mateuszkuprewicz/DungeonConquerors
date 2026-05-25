using System.Collections.Concurrent;
using System.Text;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.View.ViewCommand;

namespace ConsoleApp1.Server.View;

public class ViewWriter
{
    private BlockingCollection<IViewCommand> _viewCommands;
    private ISocketClientState _socketClientState;
    private CancellationTokenSource _cts;

    public ViewWriter(BlockingCollection<IViewCommand> viewCommands, ISocketClientState socketClientState, CancellationTokenSource cts)
    {
        _viewCommands = viewCommands;
        _socketClientState = socketClientState;
        _cts = cts;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("[ViewWriter] Wątek wysyłający uruchomiony...");

        foreach (var command in _viewCommands.GetConsumingEnumerable(_cts.Token))
        {
            try
            {
                if (command.TargetId == ServerConsts.BroadcastTargetId)
                {
                    // sendToAll
                }
                else
                {
                    await SendToClient(command.TargetId, command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ViewWriter] Błąd wysyłania do klienta {command.TargetId}: {ex.Message}");
            }
        }
    }

    private async Task SendToClient(int clientId, IViewCommand command)
    {
        var client = _socketClientState.GetTcpClient(clientId);
        
        if (client != null && client.Connected)
        {
            string payload = $"{(int)command.Type}|{command.Text}\n";
            byte[] data = Encoding.UTF8.GetBytes(payload);

            var stream = client.GetStream();
            await stream.WriteAsync(data, 0, data.Length, _cts.Token);
            await stream.FlushAsync(_cts.Token); 
            
            Console.WriteLine($"[ViewWriter] Wysłano {(ViewCommandType)command.Type} do gracza {clientId}.");
        }
    }
}