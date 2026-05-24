namespace ConsoleApp1.FightLoop.ChainOfKeyOperations;

public class LeaveNode : KeyNode
{
    private Render _render;
    public LeaveNode(Hero hero, Enemy enemy, CancellationTokenSource cts, Render render)
        :base(hero, enemy, cts) {_render = render;}

    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.L)
        {
            Hero.Stats.Health -= Enemy.Damage;
            _render.RenderStats();
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