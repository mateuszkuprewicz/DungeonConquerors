namespace ConsoleApp1.MovingAI;

public class TargetedMoving : AbstractMovingState
{
    private Queue<(int x, int y)> path;
    public TargetedMoving(IMovingEnemy Enemy, GameMap Map, Queue<(int x, int y)> Target) : base(Enemy, Map)
    {
        path = Target;
    }

    public override (int, int) GetNextMove()
    {
        if (path.Count == 0)
        {
            _enemy.MovingState = new RandomMoving(_enemy, _map);
            return _enemy.Position;
        }
        var nextPos = path.Peek();
        if (_map.enemies[nextPos.y, nextPos.x] != null)
        {
            return _enemy.Position;
        }
        return path.Dequeue();
    }
}