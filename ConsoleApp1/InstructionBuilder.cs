namespace ConsoleApp1;

internal class InstructionBuilder
{
    private (int, int) CursorForInstruction;
    private GameMap Map;
    private Hero MyHero;

    public InstructionBuilder(Hero myHero, GameMap map = null)
    {
        Map = map;
        MyHero = myHero;
        CursorForInstruction = Render.Instruction;
    }

    void Clear()
    {
        for (int i = Render.Instruction.Item2; i < Render.DefaultCursorPosition.Item2; i++)
        {
            Console.SetCursorPosition(Render.Instruction.Item1, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }
    
    bool HeroMove()
    {
        int posX = MyHero.Position.X;
        int posY = MyHero.Position.Y;
        if ((posX + 1 >=GameMap.MapWidth || Map.map[posY, posX+1] == null) && 
            (posX == 0 || Map.map[posY, posX-1] == null) && 
            (posY + 1 >= GameMap.MapHeight || Map.map[posY +1, posX] == null) &&
             (posY == 0 || Map.map[posY - 1, posX] == null))
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
        int posX = MyHero.Position.X;
        int posY = MyHero.Position.Y;
        if (Map.map[posY, posX] != null && Map.map[posY, posX].Count > 0)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write("E - pick item, Q - drop item, use arrows to scroll through equipment.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
            return true;
        }
        return false;
    }

    bool ThrowItems()
    {
        if (MyHero.Equipment.EquipmentList.Count > 0)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write("Q - drop item, use arrows to scroll through equipment.");
            return true;
        }
        return false;
    }
    bool EquipingWeapons()
    {
        int pointer = MyHero.Equipment.EquipmentPointer;
        if (MyHero.Equipment.EquipmentList.Count != 0 && MyHero.Equipment.EquipmentList[pointer].ItemType == ItemType.Weapon)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write("F - equip weapon from equipment, R - unequip weapon.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
            return true;
        }
        return false;
    }

    bool UnequipingWeapons()
    {
        if (MyHero.Hands.RightHand != null)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write("R - unequip weapon.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
            return true;
        }
        return false;
    }
    
    bool Fighting()
    {
        int posX = MyHero.Position.X;
        int posY = MyHero.Position.Y;
        if (Map.enemies[posY, posX] != null)
        {
            Render.RenderEnemyStats(Map.enemies[posY, posX]);
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write("P - start fight.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
            return true;
        }
        Render.ClearEnemyStats();
        return false;
    }
    
    void HitEnemy()
    {
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write("H - hit enemy.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }

    void RunAway()
    {
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write("L - run away.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }

    public void PrintInstructionInGameLoop()
    {
        Clear();
        CursorForInstruction = Render.Instruction;
        HeroMove();
        bool picking = PickingItems() || ThrowItems();
        bool equiping = EquipingWeapons() || UnequipingWeapons();
        Fighting();
    }

    public void PrintInstructionInFightLoop()
    {
        Clear();
        CursorForInstruction = Render.Instruction;
        //bool equiping = EquipingWeapons() || UnequipingWeapons();
        HitEnemy(); 
        RunAway();
    }
    
    public void PrintAttackInstruction()
    {
        Clear();
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write("1 - normal attack, 2 - stealth attack, 3 - magic attack.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }
}