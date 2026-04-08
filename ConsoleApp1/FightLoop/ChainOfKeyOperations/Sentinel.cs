namespace ConsoleApp1.FightLoop.ChainOfKeyOperations;

public class Sentinel : KeyNode
{
    public Sentinel(Hero hero = null, Enemy enemy = null, CancellationTokenSource cts = null)
        :base(hero, enemy, cts) {}

    public override void HandleKey(ConsoleKey keyInfo)
    {
        Render.RenderAnnouncement("Key not recognized");
    }
}