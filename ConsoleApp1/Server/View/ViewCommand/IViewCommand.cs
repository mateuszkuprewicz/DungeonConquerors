using ConsoleApp1.Server.ClientStates;

namespace ConsoleApp1.Server.View.ViewCommand;

public interface IViewCommand
{
    public ViewCommandType Type { get; }
    public int TargetId { get; set; } // 1-9 - specific id, ServerConsts.BrodcastId - all
    public bool CanSend(ISocketClientStates clientStates, int id);
    public string Text { get; }
}

