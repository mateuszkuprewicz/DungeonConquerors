namespace ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.FightLoop;

public class FightNode : KeyNode
{
    public FightNode(Hero hero, GameMap map) :  base(hero, map){}
    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.P)
        {
            Enemy? enemy = Map.enemies[MyHero.Position.Y, MyHero.Position.X];
            if (enemy != null)
            {
                var fightLoop = new FightLoop(MyHero, enemy);
                fightLoop.Loop();
                if (MyHero.Stats.Health <= 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                return;
            }
        }
        NextKeyNode.HandleKey(keyInfo);
    }
}