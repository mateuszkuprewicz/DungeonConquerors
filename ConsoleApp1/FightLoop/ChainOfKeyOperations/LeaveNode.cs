namespace ConsoleApp1.FightLoop.ChainOfKeyOperations;

public class LeaveNode : KeyNode
{
    public LeaveNode(Hero hero, Enemy enemy, CancellationTokenSource cts)
        :base(hero, enemy, cts) {}

    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.L)
        {

            return;
        }
        NextKeyNode.HandleKey(keyInfo);
    }
}