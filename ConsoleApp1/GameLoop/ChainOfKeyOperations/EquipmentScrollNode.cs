namespace ConsoleApp1.ChainOfKeyOperations;

public class EquipmentScrollNode : KeyNode
{
    public EquipmentScrollNode(Hero hero) : base(hero){}

    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.UpArrow)
        {
            Render.EquipmentScroll(MyHero, ConsoleKey.UpArrow);
            
        }
        else if (keyInfo == ConsoleKey.DownArrow)
        {
            Render.EquipmentScroll(MyHero, ConsoleKey.DownArrow);
        }
        else
        {
            NextKeyNode.HandleKey(keyInfo);
        }
    }
}