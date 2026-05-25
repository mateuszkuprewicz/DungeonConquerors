namespace ConsoleApp1.Server.View.ViewCommand;

public interface IViewCommand
{
    public ViewCommandType Type { get; }
    public int TargetId { get; set; } // 1-9 - specific id, 0 - broadcast
    public string Text { get; }
}

