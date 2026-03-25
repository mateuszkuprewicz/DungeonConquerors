namespace ConsoleApp1.ChainOfKeyOperations;

public class MoveNode : KeyNode
{

    public MoveNode(Hero hero, GameMap map) : base(hero, map){}
    
    public override void HandleKey(ConsoleKey keyInfo)
    {
        (int, int) Position = MyHero.Position;
        switch (keyInfo)
        {
            
            case ConsoleKey.W:
                if (MyHero.Move(Direction.Up, Map))
                {
                    Render.ActualiseAfterHeroMove(MyHero, Position, Map);
                    Render.RenderInfo(Map, MyHero);
                }
                else
                {
                    Render.RenderAnnouncement("Cant move into a wall");
                }
                return;
            case ConsoleKey.A:
                if (MyHero.Move(Direction.Left, Map))
                {
                    Render.ActualiseAfterHeroMove(MyHero, Position, Map);
                    Render.RenderInfo(Map, MyHero);
                }
                else
                {
                    Render.RenderAnnouncement("Cant move into a wall");
                }
                return;
            case ConsoleKey.S:
                if (MyHero.Move(Direction.Down, Map))
                {
                    Render.ActualiseAfterHeroMove(MyHero, Position, Map);
                    Render.RenderInfo(Map, MyHero);
                }
                else
                {
                    Render.RenderAnnouncement("Cant move into a wall");
                }
                return;
            case ConsoleKey.D:
                if (MyHero.Move(Direction.Right, Map))
                {
                    Render.ActualiseAfterHeroMove(MyHero, Position, Map);
                    Render.RenderInfo(Map, MyHero);
                }
                else
                {
                    Render.RenderAnnouncement("Cant move into a wall");
                }
                return;
        }
        NextKeyNode.HandleKey(keyInfo);
    }
}