using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.View;
using ConsoleApp1;
using ConsoleApp1.GameState;
using ConsoleApp1.LoopState;
namespace ConsoleApp1.GameState;

public class GameStateContext
{
    public IGameState GameState { get; set; }

    public GameStateContext(GameMap map)
    {
        GameState = new ExplorationState(map, this);
    }
    
    public void Update((int X, int Y) position)
    {
        GameState.Update(position);
    }
    
}