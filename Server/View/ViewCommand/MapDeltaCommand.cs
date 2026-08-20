using System.Text.Json;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Shared.ClientServerCommunication.ServerRequests;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.View.ViewCommand;

public class MapDeltaCommand : IViewCommand
{
    public string Type => ServerRequestsTypes.MapDelta;
    public int TargetId { get; set; }
    public string Text { get; }
    
    public MapDeltaCommand(DeltaUpdateMessage mapDelta)
    {
        TargetId = ServerConsts.BroadcastId;
        var options = new JsonSerializerOptions
        {
            WriteIndented = false,
            IncludeFields = true,
        };
        Text = JsonSerializer.Serialize(mapDelta, options);
    }
    
    public bool CanSend(ISocketClientStates clientStates, int id)
    {
        return (clientStates.IsClientInitialised(id));
    }
}