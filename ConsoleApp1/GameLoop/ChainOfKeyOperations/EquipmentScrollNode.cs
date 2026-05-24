namespace ConsoleApp1.ChainOfKeyOperations;

public class EquipmentScrollNode : KeyNode
{
    Render _render;
    public EquipmentScrollNode(Render render) => _render = render;

    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.UpArrow)
        {
            _render.EquipmentScroll(ConsoleKey.UpArrow);
            
        }
        else if (keyInfo == ConsoleKey.DownArrow)
        {
            _render.EquipmentScroll(ConsoleKey.DownArrow);
        }
        else
        {
            NextKeyNode.HandleKey(keyInfo);
        }
    }
}