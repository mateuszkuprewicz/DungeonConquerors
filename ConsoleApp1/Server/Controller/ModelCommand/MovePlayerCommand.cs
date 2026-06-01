using System.Collections.Concurrent;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.GameState;
using ConsoleApp1.LoopState;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.Model;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1.Server.Controller.Command;

public class MovePlayerCommand : IModelCommand
{
    public int Id { get; set; }
    private GameContext _gameContext;
    private Direction _direction;

    public MovePlayerCommand(int id, Direction direction, GameContext gameContext)
    {
        Id = id;
        _direction = direction;
        _gameContext = gameContext;
    }

    public bool CanExecute()
    {
        //zaimplementuj
        return true;
    }

    public void Execute(BlockingCollection<IViewCommand> viewCommands)
    {
        if (_gameContext.Map == null)
        {
            Console.WriteLine("[KRYTYCZNY BŁĄD] Obiekt _map w MovePlayerCommand jest NULLEM! Sprawdź konstruktor i fabrykę.");
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

                    if (hero.Move(_direction, map))
                    {
                        map.heroes[i, j] = null; 
                    
                        int newI = i;
                        int newJ = j;

                        switch (_direction)
                        {
                            case Direction.Up:
                                newI--;
                                break;
                            case Direction.Down:
                                newI++;
                                break;
                            case Direction.Left:
                                newJ--;
                                break;
                            case Direction.Right:
                                newJ++;
                                break;
                        }

                        map.heroes[newI, newJ] = hero;
                        hero.Position = (newJ, newI);
                        
                        Console.Error.WriteLine($"Player {hero.Id} moved to {hero.Position}");
                        DeltaUpdateMessage deltaUpdateMessage = new DeltaUpdateMessage();
                        deltaUpdateMessage.Deltas = new List<MapDelta>();
                        List<ShallowHero> deltaHeroes = new List<ShallowHero>();
                        deltaHeroes.Add(hero.ToShallowHero());
                        deltaUpdateMessage.UpdatedHeroes = deltaHeroes;
                        

                        var shallowMap = map.MapShallower(); 
                        viewCommands.Add(new MapDeltaCommand(deltaUpdateMessage));

                        return; 
                    }
                }
            }
        }
    }

}