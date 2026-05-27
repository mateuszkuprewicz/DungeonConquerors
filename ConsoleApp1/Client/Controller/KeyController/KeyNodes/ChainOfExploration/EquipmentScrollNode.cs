using ConsoleApp1.View;

namespace ConsoleApp1.ChainOfKeyOperations;

public class EquipmentScrollNode : AbstractKeyNode
{
    private readonly Render _render;

    public EquipmentScrollNode(Render render)
    {
        _render = render;
    }

    public override Task HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == ConsoleKey.UpArrow || keyInfo == ConsoleKey.DownArrow)
        {
            _render.EquipmentScroll(keyInfo);
        }
        else
        {
            NextKeyNode?.HandleKey(keyInfo);
        }
        return Task.CompletedTask;
    }
}
