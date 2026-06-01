namespace ConsoleApp1.LoopState;

public interface IHeroState
{
    void Update((int X, int Y) position, GameMap map);
    
}