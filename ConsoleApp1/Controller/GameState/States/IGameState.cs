namespace ConsoleApp1.LoopState;

public interface IGameState
{
    void HandleInput(ConsoleKey key);
    void Update(); 
    void Render();
}