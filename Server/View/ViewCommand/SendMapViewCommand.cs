using System.Text.Json;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Shared.ClientServerCommunication.ServerRequests;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.View.ViewCommand;

public class SendMapViewCommand : IViewCommand
{
    public string Type => ServerRequestsTypes.ActualiseMap;
    public int TargetId { get; set; }
    public string Text { get; }

    public SendMapViewCommand(int targetId, ShallowMap map)
    {
        TargetId = targetId;
        var options = new JsonSerializerOptions { WriteIndented = false };
        Text = JsonSerializer.Serialize(map, options);
    }

    private void OnSend(ISocketClientStates clientStates, int id)
    {
        clientStates.InitialiseClientGame(id);
    }
    
    public bool CanSend(ISocketClientStates clientStates, int id)
    {
        OnSend(clientStates, id);
        return (clientStates.IsClientInitialised(id));
    }
}
