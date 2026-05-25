namespace ConsoleApp1.LoopState;

public interface IGameState
{
    void Update((int X, int Y) position);
    
}