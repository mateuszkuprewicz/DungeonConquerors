using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.View;
using ConsoleApp1;
using ConsoleApp1.GameState;
using ConsoleApp1.LoopState;
namespace ConsoleApp1.GameState;

public class HeroStateContext
{
    public IHeroState HeroState { get; set; }

    public HeroStateContext()
    {
        HeroState = new ExplorationState(this);
    }
    
    public void Update((int X, int Y) position, GameMap map)
    {
        HeroState.Update(position, map);
    }
    
}