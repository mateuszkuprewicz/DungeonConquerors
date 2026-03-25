namespace ConsoleApp1.ChainOfKeyOperations;

public class WeaponEquipmentNode : KeyNode
{

    public WeaponEquipmentNode(Hero hero, GameMap map) : base(hero, map){}
    
    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.F)
        {
            if (MyHero.Hands.EquipWeapon(MyHero))
            {
                Render.RenderHeroHands(MyHero);
                Render.RenderEquipment(MyHero);
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