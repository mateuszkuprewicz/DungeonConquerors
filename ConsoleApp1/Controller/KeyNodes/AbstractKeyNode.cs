namespace ConsoleApp1.ChainOfKeyOperations;

public abstract class AbstractKeyNode
{
    protected Hero Hero;
    protected AbstractKeyNode NextKeyNode;
    
    public AbstractKeyNode(Hero hero = null)
    {
        Hero = hero;
    }

    public void SetNextHandler(AbstractKeyNode next)
    {
        NextKeyNode =  next;
    }
    
    public abstract void HandleKey(ConsoleKey keyInfo);
}
