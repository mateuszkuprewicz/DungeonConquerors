using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.View;
using ConsoleApp1;
using ConsoleApp1.GameState;
using ConsoleApp1.LoopState;
namespace ConsoleApp1.GameState;

public class ActionStateContext
{
    public IActionState ActionState { get; set; }

    public ActionStateContext()
    {
        ActionState = new ExplorationState(this);
    }
    
    public void Update((int X, int Y) position, GameMap map)
    {
        ActionState.Update(position, map);
    }
    
}