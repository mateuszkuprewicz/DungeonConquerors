namespace ConsoleApp1;

internal class InstructionBuilder
{
    private (int, int) CursorForInstruction;
    private GameMap Map;

    public InstructionBuilder(GameMap map)
    {
        Map = map;
        CursorForInstruction = Render.Instruction;
    }
    
    bool HeroMove()
    {
        if (Map.map[0, 1] == null && Map.map[1, 0] == null && Map.map[1, 1] == null)
        {
            return false;
        }
        
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write("Use AWDS to move.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
        return true;
    }

    bool PickingItems()
    {
        for(int i = 0; i < GameMap.MapHeight; i++)
        for (int j = 0; j < GameMap.MapWidth; j++)
        {
            if (Map.map[i, j] != null && Map.map[i, j].Count > 0)
            {
                Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
                CursorForInstruction.Item2++;
                Console.Write("E - pick item, Q - drop item, use arrows to scroll through equipment.");
                Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
                return true;
            }
        }
        return false;
    }

    bool EquipingWeapons()
    {
        
        for(int i = 0; i < GameMap.MapHeight; i++)
        for (int j = 0; j < GameMap.MapWidth; j++)
        {
            if (Map.map[i, j] != null && Map.map[i, j].TryPeek(out var temp) && temp.ItemType == ItemType.Weapon)
            {
                Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
                CursorForInstruction.Item2++;
                Console.Write("F - equip weapon from equipment, R - unequip weapon.");
                Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
                return true;
            }
        }
        return false;
    }

    public void PrintInstruction()
    {
        if (HeroMove() && PickingItems() && EquipingWeapons()) return;
    }
}