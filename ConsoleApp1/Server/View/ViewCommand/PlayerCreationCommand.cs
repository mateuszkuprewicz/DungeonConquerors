using System.Text.Json;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Shared.ClientServerCommunication.ServerRequests;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.View.ViewCommand;

public class PlayerCreationCommand : IViewCommand
{
    public string Type => ServerRequestsTypes.PlayerCreation;
    public int TargetId { get; set; }
    public string Text { get; }

    public PlayerCreationCommand(NewPlayer newPlayer)
    {
        TargetId = ServerConsts.BroadcastId;
        var options = new JsonSerializerOptions { WriteIndented = false };
        Text = JsonSerializer.Serialize(newPlayer, options);
    }

    public bool CanSend(ISocketClientStates clientStates, int id)
    {
        return clientStates.IsClientInitialised(id);
    }
}