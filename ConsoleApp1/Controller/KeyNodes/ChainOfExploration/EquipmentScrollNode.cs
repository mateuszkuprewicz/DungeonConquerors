using ConsoleApp1.View;

namespace ConsoleApp1.ChainOfKeyOperations;

public class EquipmentScrollNode : AbstractKeyNode
{
    Render _render;
    public EquipmentScrollNode(Render render) => _render = render;

    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.UpArrow)
        {
            _render.EquipmentScroll(KeyConsts.ScrollUp.key);
            
        }
        else if (keyInfo == ConsoleKey.DownArrow)
        {
            _render.EquipmentScroll(KeyConsts.ScrollDown.key);
        }
        else
        {
            NextKeyNode.HandleKey(keyInfo);
        }
    }
}