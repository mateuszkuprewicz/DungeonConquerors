namespace ConsoleApp1.ChainOfKeyOperations;

public class Sentinel : AbstractKeyNode
{
    public Sentinel(){}

    public override Task HandleKey(ConsoleKey keyInfo)
    {
        Render.RenderAnnouncement("Key not recognized");
        return Task.CompletedTask;
    }
}