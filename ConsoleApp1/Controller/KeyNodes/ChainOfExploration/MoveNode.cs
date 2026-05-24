using ConsoleApp1.View;

namespace ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.Logger;

public class MoveNode : AbstractKeyNode
{
    private Render _render;
    private GameMap _map;
    public MoveNode(Hero hero, GameMap map, Render render) : base(hero) => (_render, _map) = (render, map);
    
    public override void HandleKey(ConsoleKey keyInfo)
    {
        (int, int) Position = Hero.Position;
        if (keyInfo == KeyConsts.MoveUp.key)
        {
            if (Hero.Move(Direction.Up, _map))
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
        }


        if (KeyConsts.MoveLeft.key == keyInfo)
        {
            if (Hero.Move(Direction.Left, _map))
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
        }

        if (KeyConsts.MoveDown.key == keyInfo)
        {
            if (Hero.Move(Direction.Down, _map))
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

        if (KeyConsts.MoveRight.key == keyInfo)
        {
            if (Hero.Move(Direction.Right, _map))
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