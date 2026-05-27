using ConsoleApp1.GameState;

namespace ConsoleApp1.Server.ClientStates;

public interface IControllerClientState
{
    public GameStateContext? GetClientGameContext(int id);
    public bool InitClientGame(int id, GameMap map);
}