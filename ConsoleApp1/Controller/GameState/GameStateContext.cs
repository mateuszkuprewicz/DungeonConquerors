using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.View;
using ConsoleApp1;
using ConsoleApp1.GameState;
using ConsoleApp1.LoopState;
namespace ConsoleApp1.GameState;

public class GameStateContext
{
    public IGameState GameState { private get; set; }

    public GameStateContext(GameMap map, Hero hero, Render render, LogRenderer logRenderer)
    {
        GameState = new ExplorationState(map,  hero, render, logRenderer, this);
    }
    
    public void HandleInput(ConsoleKey key)
    {
        GameState.HandleInput(key);
    }

    public void Update()
    {
        GameState.Update();
    }

    public void Render()
    {
        GameState.Render();
    }
}