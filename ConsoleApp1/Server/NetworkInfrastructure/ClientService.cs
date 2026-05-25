using System.Net.Sockets;

namespace ConsoleApp1.Server;

public class ClientService
{
    private int _id;
    private TcpClient _client;
    private ClientGameInitialiser _clientGameInitialiser;

    public ClientService(int id, TcpClient client, ClientGameInitialiser clientGameInitialiser)
    {
        _id = id;
        _client = client;
    }

    public async Task HandleCLient(CancellationToken token)
    {
        _clientGameInitialiser.Initialise(_id, _client, token);
        
        // NetworkController clientNetworkController = new NetworkController(_id, _client);
        // NetworkRenderer clientNetworkRenderer = new NetworkRenderer(_id, _client);

        //await Task.WhenAll(clientNetworkController.Control(), clientNetworkRenderer.Render());
    }
}