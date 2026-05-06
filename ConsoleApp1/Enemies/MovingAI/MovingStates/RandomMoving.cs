namespace ConsoleApp1.MovingAI;

public class RandomMoving : AbstractMovingState
{
    public override (int, int) GetNextMove()
    {
        (int x, int y) = _enemy.Position;
        List<(int, int)> potentialPositions = new List<(int, int)>();
        potentialPositions.Add((x,y));
        if(x < GameMap.MapWidth - 1 && _map.map[y, x + 1]!=null)potentialPositions.Add((x + 1, y));
        if(x > 0 && _map.map[y, x - 1]!=null)potentialPositions.Add((x - 1, y));
        if(y < GameMap.MapHeight - 1 && _map.map[y + 1, x]!=null)potentialPositions.Add((x, y + 1));
        if(y > 0 && _map.map[y - 1, x]!=null)potentialPositions.Add((x, y - 1));
        var rnd = new Random(DateTime.Now.Millisecond);
        int index = rnd.Next(potentialPositions.Count);

        (int x, int y) nextPos = potentialPositions[index];
        if (_map.enemies[nextPos.y, nextPos.x] != null)
        {
            return _enemy.Position;
        }
        return nextPos;
    }

    public RandomMoving(IMovingEnemy Enemy, GameMap Map) : base(Enemy, Map)
    {}
}