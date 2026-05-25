namespace ConsoleApp1.View;

public class InstructionRender
{
    private (int, int) CursorForInstruction;

    public InstructionRender()
    {
        CursorForInstruction = Render.Instruction;
    }
    
    public void Clear()
    {
        for (int i = Render.Instruction.Item2; i < Render.DefaultCursorPosition.Item2; i++)
        {
            Console.SetCursorPosition(Render.Instruction.Item1, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
        CursorForInstruction =  Render.Instruction;
    }

    public void HowToMove()
    {
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write($"Use {KeyConsts.MoveLeft.letter} {KeyConsts.MoveUp.letter} {KeyConsts.MoveRight.letter} {KeyConsts.MoveDown.letter} to move.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }

    public void HowToPickItems()
    {
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write($"{KeyConsts.PickItem.letter} - pick item, {KeyConsts.DropItem.letter} - drop item, use arrows to scroll through equipment.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }

    public void HowToDropItems()
    {
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write($"{KeyConsts.DropItem.letter} - drop item, use arrows to scroll through equipment.");
    }

    public void HowToEquipItems()
    {
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write($"{KeyConsts.EquipWeapon.letter} - equip weapon from equipment, {KeyConsts.UnequipWeapon.letter} - unequip weapon.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }

    public void HowToUnequipItems()
    {
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write($"{KeyConsts.UnequipWeapon.letter} - unequip weapon.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }

    public void HowToHitEnemy()
    {
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write($"{KeyConsts.Hit.letter} - hit enemy.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }

    public void HowToRunAway()
    {
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write($"{KeyConsts.Leave.letter} - run away.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }
    
    public void PrintAttackInstruction()
    {
        Clear();
        Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
        CursorForInstruction.Item2++;
        Console.Write($"{KeyConsts.NormalAttack.letter} - normal attack, {KeyConsts.StealthAttack.letter} - stealth attack, {KeyConsts.MagicAttack.letter} - magic attack.");
        Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
    }
    
}