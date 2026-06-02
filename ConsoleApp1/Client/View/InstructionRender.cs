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
        lock (Render.ConsoleLock)
        {
            for (int i = Render.Instruction.Item2; i < Render.DefaultCursorPosition.Item2; i++)
            {
                Console.SetCursorPosition(Render.Instruction.Item1, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
            CursorForInstruction = Render.Instruction;
        }
    }

    public void HowToMove()
    {
        lock (Render.ConsoleLock)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write($"Use {KeyConsts.MoveLeft.letter} {KeyConsts.MoveUp.letter} {KeyConsts.MoveRight.letter} {KeyConsts.MoveDown.letter} to move.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
        }
    }

    public void HowToPickItems()
    {
        lock (Render.ConsoleLock)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write($"{KeyConsts.PickItem.letter} - pick item.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
        }
    }

    public void HowToDropItems()
    {
        lock (Render.ConsoleLock)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write($"{KeyConsts.DropItem.letter} - drop item.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
        }
    }

    public void HowToEquipWeapons()
    {
        lock (Render.ConsoleLock)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write($"{KeyConsts.EquipWeapon.letter} - equip weapon.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
        }
    }

    public void HowToUnequipWeapons()
    {
        lock (Render.ConsoleLock)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write($"{KeyConsts.UnequipWeapon.letter} - unequip weapon.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
        }
    }

    public void HowToHitEnemy()
    {
        lock (Render.ConsoleLock)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write($"{KeyConsts.Hit.letter} - hit enemy.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
        }
    }

    public void HowToRunAway()
    {
        lock (Render.ConsoleLock)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write($"{KeyConsts.Leave.letter} - run away.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
        }
    }
    
    public void PrintAttackInstruction()
    {
        Clear();
        lock (Render.ConsoleLock)
        {
            Console.SetCursorPosition(CursorForInstruction.Item1, CursorForInstruction.Item2);
            CursorForInstruction.Item2++;
            Console.Write($"{KeyConsts.NormalAttack.letter} - normal attack, {KeyConsts.StealthAttack.letter} - stealth attack, {KeyConsts.MagicAttack.letter} - magic attack.");
            Console.SetCursorPosition(Render.DefaultCursorPosition.Item1, Render.DefaultCursorPosition.Item2);
        }
    }
}