using ConsoleApp1.Logger;

namespace ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.Logger;

public class MoveNode : KeyNode
{
    private Render _render;
    public MoveNode(Hero hero, GameMap map, Render render) : base(hero, map){_render = render;}
    
    public override void HandleKey(ConsoleKey keyInfo)
    {
        (int, int) Position = MyHero.Position;
        switch (keyInfo)
        {
            
            case ConsoleKey.W:
                if (MyHero.Move(Direction.Up, Map))
                {
                    _render.ActualiseAfterHeroMove(Position);
                    _render.RenderInfo();
                    _render.RenderEnemies();
                }
                else
                {
                    Render.RenderAnnouncement("Cant move into a wall");
                    EventLog el = EventLog.GetEventLog();
                    el.Log(LogType.WallHit);
                }
                return;
            case ConsoleKey.A:
                if (MyHero.Move(Direction.Left, Map))
                {
                    _render.ActualiseAfterHeroMove(Position);
                    _render.RenderInfo();
                    _render.RenderEnemies();
                }
                else
                {
                    Render.RenderAnnouncement("Cant move into a wall");
                    EventLog el = EventLog.GetEventLog();
                    el.Log(LogType.WallHit);
                }
                return;
            case ConsoleKey.S:
                if (MyHero.Move(Direction.Down, Map))
                {
                    _render.ActualiseAfterHeroMove(Position);
                    _render.RenderEnemies();
                    _render.RenderInfo();
                }
                else
                {
                    Render.RenderAnnouncement("Cant move into a wall");
                    EventLog el = EventLog.GetEventLog();
                    el.Log(LogType.WallHit);
                }
                return;
            case ConsoleKey.D:
                if (MyHero.Move(Direction.Right, Map))
                {
                    _render.ActualiseAfterHeroMove(Position);
                    _render.RenderEnemies();
                    _render.RenderInfo();
                }
                else
                {
                    Render.RenderAnnouncement("Cant move into a wall");
                    EventLog el = EventLog.GetEventLog();
                    el.Log(LogType.WallHit);
                }
                return;
        }
        NextKeyNode.HandleKey(keyInfo);
    }
}