using ConsoleApp1.Logger;
namespace ConsoleApp1.ChainOfKeyOperations;

public class WeaponEquipmentNode : KeyNode
{
    private Render _render;
    public WeaponEquipmentNode(Hero hero, GameMap map, Render render) : base(hero, map){_render = render;}
    
    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.F)
        {
            (bool succes, Item? item) = MyHero.Hands.EquipWeapon(MyHero);
            if (succes)
            {
                _render.RenderHeroHands();
                _render.RenderEquipment();
                _render.RenderStats();

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
                _render.RenderHeroHands();
                _render.RenderEquipment();
                _render.RenderInfo();
                _render.RenderStats();
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