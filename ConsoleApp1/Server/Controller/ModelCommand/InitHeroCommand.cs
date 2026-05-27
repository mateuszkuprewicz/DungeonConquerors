using System.Collections.Concurrent;
using System.Windows.Input;
using ConsoleApp1.GameState;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1.Server.Controller.Command;

public class InitHeroCommand : IModelCommand
{
    public int Id { get; }
    private GameMap _map;
    private IControllerClientState _clientStates;
    private DungeonSoundManager _soundManager;

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
        var map = _map.MapShallower();
        map.PlayerId = Id + 1;
        viewCommands.Add(new SendMapViewCommand(Id, map));

        Hero hero = new Hero(Id, _soundManager);
        hero.Position = _map.GetRandomFreePosition();
        var newClient = new NewClient();
        newClient.Id = Id;
        newClient.X = hero.Position.X;
        newClient.Y = hero.Position.Y;
        _map.heroes[hero.Position.Y, hero.Position.X] = hero;
        
        viewCommands.Add(new PlayerCreationCommand(newClient));
        
        Console.WriteLine($"[GameLoop] Zainicjalizowano gracza {Id} i wrzucono mapę do wysyłki.");
        
        //find pos
        //context.Update();
    }
    
}