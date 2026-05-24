namespace ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.FightLoop;
using ConsoleApp1.Logger;

public class FightNode : KeyNode
{
    private Render _render;
    public FightNode(Hero hero, GameMap map, Render render) :  base(hero, map){_render = render;}
    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.P)
        {
            Enemy? enemy = Map.enemies[MyHero.Position.Y, MyHero.Position.X];
            if (enemy != null)
            {
                var fightLoop = new FightLoop(MyHero, enemy, _render);
                fightLoop.Loop();
                
                EventLog el = EventLog.GetEventLog();
                el.Log();
                
                if (MyHero.Stats.Health <= 0)
                {
                    Render.RenderAnnouncement(el.GetSavePath());
                    System.Threading.Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                return;
            }
        }
        NextKeyNode.HandleKey(keyInfo);
    }
}