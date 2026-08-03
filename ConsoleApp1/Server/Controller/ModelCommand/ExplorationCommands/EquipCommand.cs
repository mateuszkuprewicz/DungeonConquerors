using System.Collections.Concurrent;
using ConsoleApp1.Server.Model;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.Controller.Command;

public class EquipCommand : AbstractExplorationCommand, IModelCommand
{
    private int _itemNumber;

    public EquipCommand(int id, GameContext gameContext, int itemNumber)
    {
        Id = id;
        _gameContext = gameContext;
        _itemNumber = itemNumber;
    }

    public void Execute(BlockingCollection<IViewCommand> viewCommands)
    {
        if (_gameContext.Map == null)
        {
            Console.WriteLine("[KRYTYCZNY BŁĄD] Obiekt _map w EquipCommand jest NULLEM! Sprawdź konstruktor i fabrykę.");
            return;
        }
        
        var map = _gameContext.Map;

        for (int i = 0; i < ModelConsts.MapHeight; i++)
        {
            for (int j = 0; j < ModelConsts.MapWidth; j++)
            {
                if (map.heroes[i, j] != null && map.heroes[i, j]!.Id == Id)
                {
                    var hero = map.heroes[i, j];
                    
                    var result = hero.Hands.EquipWeapon(hero, _itemNumber);
                    
                    if (result.completion)
                    {
                        DeltaUpdateMessage deltaUpdateMessage = new DeltaUpdateMessage();
                        deltaUpdateMessage.Deltas = new List<MapDelta>();
                        deltaUpdateMessage.UpdatedHeroes = new List<(int, ShallowHero?)>();
                        
                        deltaUpdateMessage.UpdatedHeroes.Add((hero.Id, hero.ToShallowHero()));
                        
                        viewCommands.Add(new MapDeltaCommand(deltaUpdateMessage));
                        return;
                    }
                }
            }
        }
    }
}