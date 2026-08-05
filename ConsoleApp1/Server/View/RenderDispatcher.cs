using System.Collections.Concurrent;
using System.Text;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.View.ViewCommand;

namespace ConsoleApp1.Server.View;

public class RenderDispatcher
{
    private BlockingCollection<IViewCommand> _globalViewCommands;
    private Dictionary<int, ClientView> _clientViews;
    private ISocketClientStates _socketClientStates;
    private CancellationTokenSource _cts;

    public RenderDispatcher(BlockingCollection<IViewCommand> globalViewCommands, ISocketClientStates socketClientStates, CancellationTokenSource cts)
    {
        _globalViewCommands = globalViewCommands;
        _socketClientStates = socketClientStates;
        _clientViews = new Dictionary<int, ClientView>();
        _cts = cts;
    }

    public void Subscribe(int id, ClientView clientView)
    {
        _clientViews.Add(id, clientView);
    }

    public void UnSubscribe(int id)
    {
        _clientViews[id].Dispose();
        _clientViews.Remove(id);
    }

    public async Task Dispatch()
    {
        Console.WriteLine("[ViewDispatcher] Wątek wysyłający uruchomiony...");
        
        foreach (var command in _globalViewCommands.GetConsumingEnumerable(_cts.Token))
        {
            try
            {
                if (command.TargetId == ServerConsts.BroadcastId)
                {
                    foreach (var clientView in _clientViews.Values)
                    {
                        if (command.CanSend(_socketClientStates, clientView.Id))
                        {
                            clientView.Enqueue(command);
                        }
                    }
                }
                else
                {
                    if(command.CanSend(_socketClientStates, command.TargetId))
                        _clientViews[command.TargetId].Enqueue(command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ViewWriter] Błąd wysyłania do klienta komendy {command.Type}; {command.TargetId}: {ex.Message}");
            }
        }
    }
}