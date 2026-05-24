using ConsoleApp1.View;

namespace ConsoleApp1.ChainOfKeyOperations;

public class LeaveNode : AbstractKeyNode
{
    private Render _render;
    private Enemy _enemy;
    private GameMap _map;

    public LeaveNode(Hero hero, Enemy enemy, GameMap map, Render render)
        : base(hero)
    {
        _render = render;
        _enemy = enemy;
        _map = map;
    }

    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == KeyConsts.Leave.key)
        {
            Hero.Stats.Health -= _enemy.Damage;
            _render.RenderStats();
            if (Hero.Stats.Health <= 0)
            {
                Render.RenderGameOver();
            }
            else
            {
                if(Hero.Position.Y + 1 < GameMap.MapHeight && _map.map[Hero.Position.Y + 1, Hero.Position.X] != null && _map.enemies[Hero.Position.Y + 1, Hero.Position.X] == null)
                {
                    Hero.Move(Direction.Down, _map);
                    Render.RenderAnnouncement("You run from the fight");
                }
                else if(Hero.Position.Y - 1 >= 0 && _map.map[Hero.Position.Y - 1, Hero.Position.X] != null && _map.enemies[Hero.Position.Y - 1, Hero.Position.X] == null)
                {
                    Hero.Move(Direction.Up, _map);
                    Render.RenderAnnouncement("You run from the fight");
                }
                else if(Hero.Position.X + 1 < GameMap.MapWidth && _map.map[Hero.Position.Y, Hero.Position.X + 1] != null && _map.enemies[Hero.Position.Y, Hero.Position.X + 1] == null)
                {
                    Hero.Move(Direction.Right, _map);
                    Render.RenderAnnouncement("You run from the fight");
                }
                else if(Hero.Position.X - 1 >= 0 && _map.map[Hero.Position.Y, Hero.Position.X - 1] != null && _map.enemies[Hero.Position.Y, Hero.Position.X - 1] == null)
                {
                    Hero.Move(Direction.Left, _map);
                    Render.RenderAnnouncement("You run from the fight");
                }
                else
                {
                    Render.RenderAnnouncement("There is nowhere to run!");
                }
            }
        }
        else NextKeyNode.HandleKey(keyInfo);
    }
}