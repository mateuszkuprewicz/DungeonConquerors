using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.View;
using ConsoleApp1;
using ConsoleApp1.GameState;

namespace ConsoleApp1.LoopState;

public class ExplorationState : IGameState
{
    private GameMap _map;
    private GameStateContext _stateContext;

    public ExplorationState(GameMap map, GameStateContext stateContext)
    {
        _map = map;
        _stateContext = stateContext;
    }
    
    public void Update((int X, int Y) position)
    {
        if (_map.enemies[position.Y, position.X] != null)
        {
            _stateContext.GameState = new ExplorationState(_map, _stateContext);
        }
    }
}