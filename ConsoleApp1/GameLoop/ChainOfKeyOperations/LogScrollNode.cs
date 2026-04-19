namespace ConsoleApp1.ChainOfKeyOperations;

public class LogScrollNode : KeyNode
{
    public LogScrollNode(){}

    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.O)
        {
            Render.ScrollLogsUp();
        }
        else if (keyInfo == ConsoleKey.L)
        {
            Render.ScrollLogsDown();
        }
    }
}