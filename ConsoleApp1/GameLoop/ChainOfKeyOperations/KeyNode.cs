namespace ConsoleApp1.ChainOfKeyOperations;

public abstract class KeyNode
{
    protected Hero MyHero;
    protected GameMap Map;
    protected KeyNode NextKeyNode;
    
    public KeyNode(Hero hero = null,  GameMap map = null)
    {
        MyHero = hero;
        Map = map;
    }

    public void SetNextHandler(KeyNode next)
    {
        NextKeyNode =  next;
    }
    
    public abstract void HandleKey(ConsoleKey keyInfo);
}
