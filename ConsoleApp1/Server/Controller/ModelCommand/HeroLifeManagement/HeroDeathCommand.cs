using System.Collections.Concurrent;
using ConsoleApp1.Server.Model;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.Controller.Command;

public class HeroDeathCommand : IModelCommand
{
    public int Id { get; }
    private GameContext _gameContext;

    public HeroDeathCommand(int id, GameContext gameContext)
    {
        Id = id;
        _gameContext = gameContext;
    }

    public bool CanExecute()
    {
        return true;
    }

    public void Execute(BlockingCollection<IViewCommand> viewCommands)
    {
        for(int i = 0; i < ModelConsts.MapHeight; i++)
            for(int j = 0; j < ModelConsts.MapWidth; j++)
            {
                var tHero = _gameContext.Map.heroes[i, j];
                if (tHero != null && tHero.Id == Id)
                {
                    _gameContext.Map.heroes[i, j] = null;
                    
                    DeltaUpdateMessage deltaUpdateMessage = new DeltaUpdateMessage();
                    deltaUpdateMessage.Deltas = new List<MapDelta>();
                    List<(int,ShallowHero?)> deltaHeroes = new List<(int,ShallowHero?)>();
                    deltaHeroes.Add((tHero.Id, tHero.ToShallowHero()));
                    deltaUpdateMessage.UpdatedHeroes = deltaHeroes;
                    viewCommands.Add(new MapDeltaCommand(deltaUpdateMessage));
                    viewCommands.Add(new SendLogCommand(Id, new LogMessege() { Text = $"Player died"}));
                    Console.WriteLine($"[GameLoop] Uśmiercono gracza {Id}");
                    return;
                }
            }
        
        
    }
}