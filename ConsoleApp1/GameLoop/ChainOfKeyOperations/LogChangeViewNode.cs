using ConsoleApp1.Logger;

namespace ConsoleApp1.ChainOfKeyOperations;

public class LogChangeViewNode : KeyNode
{
    public LogChangeViewNode(){}

    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.J)
        {
            EventLog eventLog = EventLog.GetEventLog();
            eventLog.renderType = !eventLog.renderType;
            eventLog.Log();
        }
        else
        { 
            NextKeyNode.HandleKey(keyInfo);
        }
    }
}