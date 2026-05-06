namespace ConsoleApp1.SoundPropagation.SoundMediation;

public class DungeonSoundManager : ISoundPublisher, ISoundSubscribtion
{
    private HashSet<ISoundHearer> _hearers = new HashSet<ISoundHearer>();
    private GameMap _map;
    
    public DungeonSoundManager(GameMap map)
    {
        _map = map;
        foreach (var e in map.enemies)
        {
            if(e!=null) _hearers.Add(e);
        }
    }
    
    public void Notify((int, int) source, int range)
    {
        var sound = new NoiseEvent(source, range, _map);
        foreach (var e in _hearers)
            e.Hear(sound);
    }

    public void Subscribe(ISoundHearer hearer)
    {
        _hearers.Add(hearer);
    }

    public void Unsubscribe(ISoundHearer hearer)
    {
        _hearers.Remove(hearer);
    }
    
}