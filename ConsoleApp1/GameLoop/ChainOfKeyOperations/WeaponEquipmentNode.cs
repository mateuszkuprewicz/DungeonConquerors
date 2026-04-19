using ConsoleApp1.Logger;
namespace ConsoleApp1.ChainOfKeyOperations;

public class WeaponEquipmentNode : KeyNode
{

    public WeaponEquipmentNode(Hero hero, GameMap map) : base(hero, map){}
    
    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.F)
        {
            (bool succes, Item? item) = MyHero.Hands.EquipWeapon(MyHero);
            if (succes)
            {
                Render.RenderHeroHands(MyHero);
                Render.RenderEquipment(MyHero);
                Render.RenderStats(MyHero);

                EventLog el = EventLog.GetEventLog();
                el.Log(LogType.WeaponEquip, [item.Name]);
            }
            else
            {
                Render.RenderAnnouncement("Cannot wear this now!");
            }
        }
        else if (keyInfo == ConsoleKey.R)
        {
            if (MyHero.Hands.UnequipWeapon(MyHero, Map))
            {
                Render.RenderHeroHands(MyHero);
                Render.RenderEquipment(MyHero);
                Render.RenderInfo(Map, MyHero);
                Render.RenderStats(MyHero);
            }
            else
            {
                Render.RenderAnnouncement("You dont wear onything on yourself!");
            }
        }
        else
        {
            NextKeyNode.HandleKey(keyInfo);
        }
    }
}