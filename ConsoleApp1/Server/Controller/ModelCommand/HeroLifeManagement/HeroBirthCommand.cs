using System.Collections.Concurrent;
using System.Windows.Input;
using ConsoleApp1.GameState;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.Model;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1.Server.Controller.Command;

public class HeroBirthCommand : IModelCommand
{
    public int Id { get; }
    private GameContext _gameContext;

    public HeroBirthCommand(int id, GameContext gameContext)
    {
        Id = id;
        _gameContext = gameContext;
    }

    public bool CanExecute()
    {
        var map = _gameContext.Map;
        for (int i = 0; i < ModelConsts.MapHeight; i++)
        {
            for (int j = 0; j < ModelConsts.MapWidth; j++)
            {
                if (map.map[i, j] != null && map.enemies[i, j] == null && map.heroes[i, j] == null)
                    return true;
            }
        }
        return false;
    }
    
    public void Execute(BlockingCollection<IViewCommand> viewCommands)
    {
        var shallowMap = _gameContext.Map.MapShallower();
        shallowMap.PlayerId = Id;
        viewCommands.Add(new SendMapViewCommand(Id, shallowMap));

        var map = _gameContext.Map;
        Hero hero = new Hero(Id, _gameContext.SoundManager);
        hero.Position = map.GetRandomFreePosition();
        var newClient = new NewPlayer();
        newClient.Id = Id;
        newClient.X = hero.Position.X;
        newClient.Y = hero.Position.Y;
        map.heroes[hero.Position.Y, hero.Position.X] = hero;
        
        viewCommands.Add(new PlayerCreationCommand(newClient));
        
        Console.WriteLine($"[GameLoop] Zainicjalizowano gracza {Id} na pozycji {hero.Position.X}, {hero.Position.Y} i wrzucono mapę do wysyłki.");
    }
    
}