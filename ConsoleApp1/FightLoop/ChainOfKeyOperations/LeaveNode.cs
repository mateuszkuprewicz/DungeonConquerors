namespace ConsoleApp1.FightLoop.ChainOfKeyOperations;

public class LeaveNode : KeyNode
{
    public LeaveNode(Hero hero, Enemy enemy, CancellationTokenSource cts)
        :base(hero, enemy, cts) {}

    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.L)
        {
            Hero.Stats.Health -= Enemy.Damage;
            Render.RenderStats(Hero);
            if (Hero.Stats.Health <= 0)
            {
                Render.RenderGameOver();
                _cts.Cancel();
            }
            else
            {
                Render.RenderAnnouncement("You run from the fight");
                _cts.Cancel();
            }
            return;
        }
        NextKeyNode.HandleKey(keyInfo);
    }
}