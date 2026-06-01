using System.Text.Json;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Shared.ClientServerCommunication.ServerRequests;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;

namespace ConsoleApp1.Server.View.ViewCommand;

public class SendLogCommand : IViewCommand
{
    public string Type => ServerRequestsTypes.LogMessage;
    public int TargetId { get; set; }
    public string Text { get; set; }
    
    public SendLogCommand(int targetId, LogMessege logMessege)
    {
        TargetId = targetId;
        var options = new JsonSerializerOptions { WriteIndented = false };
        Text = JsonSerializer.Serialize(logMessege, options);
    }

    public bool CanSend(ISocketClientStates clientStates, int id)
    {
        return true;
    }
}