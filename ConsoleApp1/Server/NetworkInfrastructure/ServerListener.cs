using ConsoleApp1.Server.ClientStates;

namespace ConsoleApp1.Server;
using System.Globalization;
using System.Net;
using System.Net.Sockets;


public class ServerListener
{
    private int _port;
    private SemaphoreSlim _connections;
    private IAcceptClientState _states;
    private ClientGameInitialiser _clientGameInitialiser;
    private CancellationTokenSource cts;

    public ServerListener(int Port, ClientGameInitialiser clientGameInitialiser, IAcceptClientState states, CancellationTokenSource cts)
    {
        _port = Port;
        _connections = new SemaphoreSlim(ServerConsts.MaxConnections, ServerConsts.MaxConnections);
        _states = states;
        _clientGameInitialiser = clientGameInitialiser;
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
        Console.WriteLine("New client connected");
        listener.Start(ServerConsts.MaxConnections);
        var clients = new List<Task>();
        try
        {
            while (!token.IsCancellationRequested)
            {
                await _connections.WaitAsync(token);
                TcpClient client = await listener.AcceptTcpClientAsync(token);
                int id = _states.Connect(client);
                clients.Add(RunClientAndCleanup(client, id, token));
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

    private async Task RunClientAndCleanup(TcpClient client, int id, CancellationToken token)
    {
        try
        {
            ClientService cs = new ClientService(id, client, _clientGameInitialiser);
            await cs.HandleCLient(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _states.Disconnect(id);
            _connections.Release();
            client.Dispose();
        }
    }
}