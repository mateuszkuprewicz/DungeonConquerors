namespace ConsoleApp1.View;

public static class KeyConsts
{
    public static readonly (ConsoleKey key, char letter) ScrollUp = (ConsoleKey.UpArrow, ' ');
    public static readonly (ConsoleKey key, char letter) ScrollDown = (ConsoleKey.DownArrow, ' ');
    public static readonly (ConsoleKey key, char letter) LogView = (ConsoleKey.J, 'J');
    public static readonly (ConsoleKey key, char letter) MoveUp = (ConsoleKey.W, 'W');
    public static readonly (ConsoleKey key, char letter) MoveDown = (ConsoleKey.S, 'S');
    public static readonly (ConsoleKey key, char letter) MoveRight = (ConsoleKey.D, 'D');
    public static readonly (ConsoleKey key, char letter) MoveLeft = (ConsoleKey.A, 'A');
    public static readonly (ConsoleKey key, char letter) PickItem = (ConsoleKey.E, 'E');
    public static readonly (ConsoleKey key, char letter) DropItem = (ConsoleKey.Q, 'Q');
    public static readonly (ConsoleKey key, char letter) EquipWeapon = (ConsoleKey.F, 'F');
    public static readonly (ConsoleKey key, char letter) UnequipWeapon = (ConsoleKey.R, 'R');
    
    public static readonly (ConsoleKey key, char letter) Leave = (ConsoleKey.L, 'L');
    public static readonly (ConsoleKey key, char letter) Hit = (ConsoleKey.H, 'H');
    public static readonly (ConsoleKey key, char letter) NormalAttack = (ConsoleKey.D1, '1');
    public static readonly (ConsoleKey key, char letter) StealthAttack = (ConsoleKey.D2, '2');
    public static readonly (ConsoleKey key, char letter) MagicAttack = (ConsoleKey.D3, '3');
}