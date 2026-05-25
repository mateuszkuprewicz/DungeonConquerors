namespace ConsoleApp1.MovingAI;

public abstract class AbstractMovingState
{
    protected IMovingEnemy _enemy;
    protected GameMap _map;
    public abstract (int, int) GetNextMove();

    public AbstractMovingState(IMovingEnemy Enemy, GameMap Map)
    {
        _enemy = Enemy;
        _map = Map;
    }
}