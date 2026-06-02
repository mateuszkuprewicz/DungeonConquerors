namespace ConsoleApp1;
using ConsoleApp1.Items.Weapon;
using ConsoleApp1.View;
using ConsoleApp1.Shared;

internal class InstructionBuilder
{
    private (int, int) CursorForInstruction;
    private GameMap Map;
    private Hero MyHero;
    private InstructionRender _instructionRender;

    public InstructionBuilder(Hero myHero, GameMap map, InstructionRender instructionRender)
    {
        Map = map;
        MyHero = myHero;
        _instructionRender = instructionRender;
    }

    
    bool HeroMove()
    {
        int posX = MyHero.Position.X;
        int posY = MyHero.Position.Y;
        if ((posX + 1 >=ModelConsts.MapWidth || Map.map[posY, posX+1] == null) && 
            (posX == 0 || Map.map[posY, posX-1] == null) && 
            (posY + 1 >= ModelConsts.MapHeight || Map.map[posY +1, posX] == null) &&
             (posY == 0 || Map.map[posY - 1, posX] == null))
        {
            return false;
        }
        
        return true;
    }

    bool PickingItems()
    {
        int posX = MyHero.Position.X;
        int posY = MyHero.Position.Y;
        if (Map.map[posY, posX] != null && Map.map[posY, posX].Count > 0)
        {
            return true;
        }
        return false;
    }

    bool ThrowItems()
    {
        if (MyHero.Equipment.EquipmentList.Count > 0)
        {
            return true;
        }
        return false;
    }
    // bool EquipingWeapons()
    // {
    //     int pointer = MyHero.Equipment.equipmentPointer;
    //     if (MyHero.Equipment.EquipmentList.Count != 0 && MyHero.Equipment.EquipmentList[pointer] is AbstractWeapon)
    //     {
    //         return true;
    //     }
    //     return false;
    // }

    bool UnequipingWeapons()
    {
        if (MyHero.Hands.RightHand != null)
        {
            return true;
        }
        return false;
    }

    bool Fighting()
    {
        if (Map.enemies[MyHero.Position.Y, MyHero.Position.X] != null) return true;
        return false;
    }
    
    public void PrintInstructionInGameLoop()
    {
        _instructionRender.Clear();
        
        if (Fighting())
        {
            _instructionRender.HowToHitEnemy();
            _instructionRender.HowToRunAway();
        }
        else
        {
            if(HeroMove()) _instructionRender.HowToMove();
            if(PickingItems()) _instructionRender.HowToPickItems();
            else if(ThrowItems()) _instructionRender.HowToDropItems();
            // if(EquipingWeapons()) _instructionRender.HowToEquipItems();
            // else if(UnequipingWeapons()) _instructionRender.HowToUnequipItems();
        }
    }
}