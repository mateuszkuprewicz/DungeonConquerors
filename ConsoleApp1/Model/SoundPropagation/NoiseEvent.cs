namespace ConsoleApp1.SoundPropagation;

public class NoiseEvent
{
    public (int X, int Y) Source { get; }
    public int Range { get; }
    private Dictionary<(int X, int Y), (int X, int Y)> _cameFrom;

    public NoiseEvent((int, int) source, int range, GameMap map) 
    {
        Source = source;
        Range = range;
        _cameFrom = CalculateSoundSpread(source, range, map);
    }

    public bool HasReached((int, int) targetPosition) 
    {
        return _cameFrom.ContainsKey(targetPosition);
    }
    
    public Queue<(int X, int Y)> GetPathToSource((int X, int Y) listenerPosition)
    {
        var path = new Queue<(int X, int Y)>();
        var current = listenerPosition;

        while (current != Source)
        {
            current = _cameFrom[current]; 
            path.Enqueue(current);
        }
        return path;
    }
    
    private Dictionary<(int X, int Y), (int X, int Y)> CalculateSoundSpread((int X, int Y) start, int range, GameMap map)
    {
        var cameFrom = new Dictionary<(int X, int Y), (int X, int Y)>();
        var queue = new Queue<((int X, int Y) Position, int Distance)>();

        queue.Enqueue((start, 0));
        cameFrom[start] = start;

        var directions = new (int dX, int dY)[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var pos = current.Position;
            var dist = current.Distance;

            if (dist >= range) continue;

            foreach (var dir in directions)
            {
                int nextX = pos.X + dir.dX;
                int nextY = pos.Y + dir.dY;
                var nextPos = (nextX, nextY);

                if (nextX >= 0 && nextX < GameMap.MapWidth && nextY >= 0 && nextY < GameMap.MapHeight)
                {
                    if (map.map[nextY, nextX] != null) 
                    {
                        if (!cameFrom.ContainsKey(nextPos))
                        {
                            cameFrom[nextPos] = pos;
                            queue.Enqueue((nextPos, dist + 1));
                        }
                    }
                }
            }
        }
        return cameFrom;
    }
}