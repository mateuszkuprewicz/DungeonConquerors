using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.View;

namespace ConsoleApp1.Server;
using System.Globalization;
using System.Net;
using System.Net.Sockets;


public class ClientLifeManager
{
    private int _port;
    private SemaphoreSlim _connections;
    private IAcceptClientState _states;
    private QueueClientRequest _clientsQueuer;
    private RenderDispatcher _renderDispatcher;
    private CancellationTokenSource cts;

    public ClientLifeManager(int Port, QueueClientRequest clientsQueuer, IAcceptClientState states, RenderDispatcher renderDispatcher, CancellationTokenSource cts)
    {
        _port = Port;
        _connections = new SemaphoreSlim(ServerConsts.MaxConnections, ServerConsts.MaxConnections);
        _states = states;
        _clientsQueuer = clientsQueuer;
        _renderDispatcher = renderDispatcher;
        this.cts = cts;
    }
    
    public async Task Run()
    {
        var ipEndPoint = new IPEndPoint(IPAddress.Any, _port);
        using var listener = new TcpListener(ipEndPoint);
        await AcceptClients(listener, cts.Token);
    }

    private async Task AcceptClients(TcpListener listener, CancellationToken token = default)
    {
        listener.Start(ServerConsts.MaxConnections);
        var clients = new List<Task>();
        try
        {
            while (!token.IsCancellationRequested)
            {
                await _connections.WaitAsync(token);
                TcpClient client = await listener.AcceptTcpClientAsync(token);
                Console.WriteLine("New client connected\n");
                
                int id = _states.Connect(client);
                clients.Add(StartClientAndCleanup(client, id, token));
                clients.RemoveAll(t => t.IsCompleted);
            }
        }
        catch (ObjectDisposedException)
        {
        }
        finally
        {
            await Task.WhenAll(clients);
        }
    }

    private async Task StartClientAndCleanup(TcpClient client, int id, CancellationToken token)
    {
        try
        {
            var clientView = new ClientView(id, client);
            _renderDispatcher.Subscribe(id, clientView);
            _clientsQueuer.Initialise(id, client, token);
            ClientReader cr = new ClientReader(id, client, _clientsQueuer);
            await cr.HandleCLient(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _renderDispatcher.UnSubscribe(id);
            _states.Disconnect(id);
            _connections.Release();
            client.Dispose();
        }
    }
}