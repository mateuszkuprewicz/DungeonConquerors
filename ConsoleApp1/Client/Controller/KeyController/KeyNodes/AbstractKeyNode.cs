namespace ConsoleApp1.ChainOfKeyOperations;

public abstract class AbstractKeyNode
{   
    protected AbstractKeyNode NextKeyNode;
    

    public void SetNextHandler(AbstractKeyNode next)
    {
        NextKeyNode =  next;
    }
    
    public abstract Task HandleKey(ConsoleKey keyInfo);
}
