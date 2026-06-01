using System.Collections.Concurrent;
using ConsoleApp1.Server.Model;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.Controller.Command;

public class DropCommand : AbstractExplorationCommand, IModelCommand
{
    private int _equipmentPointer;
    public DropCommand(int id, GameContext gameContext, int equipmentPointer)
    {
        Id = id;
        _gameContext = gameContext;
        _equipmentPointer = equipmentPointer;
    }

    public void Execute(BlockingCollection<IViewCommand> viewCommands)
    {
        if (_gameContext.Map == null)
        {
            Console.WriteLine("[KRYTYCZNY BŁĄD] Obiekt _map w DropCommand jest NULLEM! Sprawdź konstruktor i fabrykę.");
            return;
        }
        
        var map = _gameContext.Map;

        for (int i = 0; i < ModelConsts.MapHeight; i++)
        {
            for (int j = 0; j < ModelConsts.MapWidth; j++)
            {
                if (map.heroes[i, j] != null && map.heroes[i, j].Id == Id)
                {
                    var hero = map.heroes[i, j];
                    if (map.map.Length == 0) return;
                    
                    bool success = hero.Equipment.DropItem((j, i), map, _equipmentPointer);
                    
                    LogMessege temp = new LogMessege();
                    temp.Text = success ? "Player dropped an item" : "Player couldn't drop an item";
                    var sendLogCommand = new SendLogCommand(Id, temp);
                    
                    if (success)
                    {
                        Console.Error.WriteLine($"Player {hero.Id} dropped an item");
                        
                        DeltaUpdateMessage deltaUpdateMessage = new DeltaUpdateMessage();
                        deltaUpdateMessage.Deltas = new List<MapDelta>();
                        deltaUpdateMessage.UpdatedHeroes = new List<ShallowHero>();
                        
                        MapDelta tyleDelta = new MapDelta();
                        tyleDelta.X = j;
                        tyleDelta.Y = i;
                        
                        var newItem = map.map[i,j] == null || map.map[i,j]!.Count == 0 ? null : map.map[i,j]!.Peek();
                        ShallowItem? shallowItem = null;
                        if(newItem == null) shallowItem = null;
                        else
                        {
                            shallowItem = new ShallowItem();
                            shallowItem.Name = newItem.Name;
                            shallowItem.Symbol = newItem.Symbol;
                        }
                        tyleDelta.Item = shallowItem;
                        
                        deltaUpdateMessage.Deltas.Add(tyleDelta);
                        deltaUpdateMessage.UpdatedHeroes.Add(hero.ToShallowHero());
                        
                        viewCommands.Add(new MapDeltaCommand(deltaUpdateMessage));
                        viewCommands.Add(sendLogCommand);
                        return;
                    }
                }
            }
        }
    }
}