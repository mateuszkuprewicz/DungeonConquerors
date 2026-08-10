using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.View;
using ConsoleApp1;
using ConsoleApp1.GameState;

namespace ConsoleApp1.LoopState;

public class ExplorationState : IActionState
{
    private ActionStateContext _stateContext;

    public ExplorationState(ActionStateContext stateContext)
    {
        _stateContext = stateContext;
    }
    
    public void Update((int X, int Y) position, GameMap map)
    {
        if (map.enemies[position.Y, position.X] != null)
        {
            _stateContext.ActionState = new CombatState(_stateContext);
        }
    }
}