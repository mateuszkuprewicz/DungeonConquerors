using ConsoleApp1.Shared.ShallowModel;
using ConsoleApp1.View;

namespace ConsoleApp1.ChainOfKeyOperations;

public class EquipmentScrollNode : AbstractKeyNode
{
    private readonly Render _render;
    private readonly Shared.ShallowModel.GameState _state;

    public EquipmentScrollNode(Render render, Shared.ShallowModel.GameState state)
    {
        _render = render;
        _state = state;
    }

    public override async Task HandleKey(ConsoleKey keyInfo)
    {
        var equipment = _state.Hero?.Equipment;
        
        if (equipment?.EquipmentList == null || equipment.EquipmentList.Count == 0)
        {
            if (NextKeyNode != null)
                await NextKeyNode.HandleKey(keyInfo);
            return;
        }

        int maxIndex = equipment.EquipmentList.Count - 1;
        int oldPointer = equipment.EquipmentPointer;

        if (keyInfo == ConsoleKey.UpArrow && equipment.EquipmentPointer > 0)
        {
            equipment.EquipmentPointer--;
            _render.UpdateEquipmentScroll(oldPointer, equipment.EquipmentPointer);
        }
        else if (keyInfo == ConsoleKey.DownArrow && equipment.EquipmentPointer < maxIndex)
        {
            equipment.EquipmentPointer++;
            _render.UpdateEquipmentScroll(oldPointer, equipment.EquipmentPointer);
        }
        else
        {
            if (NextKeyNode != null)
                await NextKeyNode.HandleKey(keyInfo);
        }
    }
}