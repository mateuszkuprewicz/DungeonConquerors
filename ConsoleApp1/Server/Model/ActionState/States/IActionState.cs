namespace ConsoleApp1.LoopState;

public interface IActionState
{
    void Update((int X, int Y) position, GameMap map);
    
}