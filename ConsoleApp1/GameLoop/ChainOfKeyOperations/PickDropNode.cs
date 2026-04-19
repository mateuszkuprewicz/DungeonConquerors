using ConsoleApp1.Logger;
namespace ConsoleApp1.ChainOfKeyOperations;

public class PickDropNode : KeyNode
{

    public PickDropNode(Hero hero, GameMap map) : base(hero, map){}
    
    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.E)
        {
            (int result, Item? item) = MyHero.Equipment.PickItem(MyHero.Position, Map);
            if(result == 1)
            {
                Render.RenderInfo(Map, MyHero);
                Render.RenderMenu(MyHero, Map);
                
                EventLog el = EventLog.GetEventLog();
                el.Log(LogType.ItemPick, [item.Name]);
            }
            if (result == 0) Render.RenderAnnouncement("No items are lying here!");
            if(result == -1)
            {
                Render.RenderAnnouncement("Full inventory! Max number of items is 10.");
            }
        }
        else if (keyInfo == ConsoleKey.Q)
        {
            if(MyHero.Equipment.DropItem(MyHero.Position, Map))
            {
                Render.RenderInfo(Map, MyHero);
                Render.RenderMenu(MyHero, Map);
            }
            else
            {
                Render.RenderAnnouncement("You have empty equipment!");
            }
        }
        else
        { 
            NextKeyNode.HandleKey(keyInfo);
        }
    }
}