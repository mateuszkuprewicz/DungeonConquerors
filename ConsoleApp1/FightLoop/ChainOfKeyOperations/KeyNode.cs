namespace ConsoleApp1.FightLoop.ChainOfKeyOperations;

public abstract class KeyNode
{
    protected Hero Hero;
    protected Enemy Enemy;
    protected KeyNode NextKeyNode;
    private CancellationTokenSource _cts;

    public KeyNode(Hero hero, Enemy enemy, CancellationTokenSource cts)
        => (Hero, Enemy, _cts) = (hero, enemy, cts);
    
    public void SetNextHandler(KeyNode next)
    {
        NextKeyNode =  next;
    }
    
    public abstract void HandleKey(ConsoleKey keyInfo);
}