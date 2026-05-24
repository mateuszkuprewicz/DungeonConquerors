namespace ConsoleApp1.ChainOfKeyOperations;

public class Sentinel : AbstractKeyNode
{
    public Sentinel(){}

    public override void HandleKey(ConsoleKey keyInfo)
    {
        Render.RenderAnnouncement("Key not recognized");
    }
}