namespace ConsoleApp1.FightLoop.ChainOfKeyOperations;

public class HitNode : KeyNode
{
    public HitNode(Hero hero, Enemy enemy, CancellationTokenSource cts) : 
        base(hero, enemy, cts){}
    
    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.H)
        {
            
            return;
        }
        NextKeyNode.HandleKey(keyInfo);
    }
}