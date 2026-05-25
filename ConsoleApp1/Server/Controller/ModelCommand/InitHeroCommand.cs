using System.Collections.Concurrent;
using System.Windows.Input;
using ConsoleApp1.GameState;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;

namespace ConsoleApp1.Server.Controller.Command;

public class InitHeroCommand : IModelCommand
{
    public int Id { get; }
    private GameMap _map;
    private IControllerClientState _clientStates;

    public InitHeroCommand(int id, GameMap map, IControllerClientState clientStates)
    {
        Id = id;
        _map = map;
        _clientStates = clientStates;
    }

    public bool CanExecute(GameStateContext context)
    {
        for (int i = 0; i < ModelConsts.MapHeight; i++)
        {
            for (int j = 0; j < ModelConsts.MapWidth; j++)
            {
                if (_map.map[i, j] != null && _map.enemies[i, j] == null && _map.heroes[i, j] == null)
                    return true;
            }
        }
        return false;
    }
    
    public void Execute(GameStateContext context, BlockingCollection<IViewCommand> viewCommands)
    {
        _clientStates.InitClientGame(Id, _map);
        string mapJson = "{\"TestMapMessage\": \"Zaraz tu bedzie zserializowana mapa DTO\"}";
        viewCommands.Add(new SendMapViewCommand(Id, mapJson));
        Console.WriteLine($"[GameLoop] Zainicjalizowano gracza {Id} i wrzucono mapę do wysyłki.");
        
        //find pos
        //context.Update();
    }

}