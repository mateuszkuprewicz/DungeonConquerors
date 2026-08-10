using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.GameState;
using ConsoleApp1.View;

namespace ConsoleApp1.LoopState;

public class CombatState : IActionState
{
    private ActionStateContext _stateContext;
    
    public CombatState(ActionStateContext stateContext)
    {
        _stateContext = stateContext;
    }
    
    public void Update((int X, int Y) position, GameMap map)
    {
        if (map.enemies[position.Y, position.X] == null)
        {
            _stateContext.ActionState = new ExplorationState(_stateContext);
        }
    }
}