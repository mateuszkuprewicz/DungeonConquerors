using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1.Server.Model;

public class GameContext
{
    public GameMap Map;
    public HashSet<Hero> Heroes;
    public HashSet<Enemy> Enemies;
    public DungeonSoundManager SoundManager;
    
    public GameContext(GameMap map, DungeonSoundManager soundManager)
    {
        Map = map;
        Heroes = new HashSet<Hero>();
        foreach(var hero in map.heroes)
            if(hero!=null) Heroes.Add(hero);
        Enemies = new HashSet<Enemy>();
        foreach (var enemy in map.enemies)
            if (enemy != null) Enemies.Add(enemy);
        SoundManager = soundManager;
    }
}