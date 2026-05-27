using ConsoleApp1.Server.ClientStates;

namespace ConsoleApp1.Server.View.ViewCommand;

public class SendMapViewCommand : IViewCommand
{
    public ViewCommandType Type => ViewCommandType.sendMap;
    public int TargetId { get; set; }
    public string Text { get; }

    public SendMapViewCommand(int targetId, string text)
    {
        TargetId = targetId;
        Text = text;
    }

    private void OnSend(ISocketClientStates clientStates)
    {
        clientStates.InitialiseClientGame(TargetId);
    }
    
    public bool CanSend(ISocketClientStates clientStates, int id)
    {
        OnSend(clientStates);
        return (clientStates.IsClientInitialised(id));
    }
}
