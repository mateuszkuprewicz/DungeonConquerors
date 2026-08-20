using System.Collections.Concurrent;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.Server.Model;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1.Server.Controller.Command;

public class MoveHeroCommand : AbstractExplorationCommand, IModelCommand
{
    private Direction _direction;

    public MoveHeroCommand(int id, Direction direction, GameContext gameContext)
    {
        Id = id;
        _direction = direction;
        _gameContext = gameContext;
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
                        
                        Console.Error.WriteLine($"[MoveHeroCommand] Player {hero.Id} moved to {hero.Position}");
                        DeltaUpdateMessage deltaUpdateMessage = new DeltaUpdateMessage();
                        deltaUpdateMessage.Deltas = new List<MapDelta>();
                        List<(int,ShallowHero?)> deltaHeroes = new List<(int,ShallowHero?)>();
                        deltaHeroes.Add((hero.Id, hero.ToShallowHero()));
                        deltaUpdateMessage.UpdatedHeroes = deltaHeroes;
                        
                        viewCommands.Add(new MapDeltaCommand(deltaUpdateMessage));
                        viewCommands.Add(new SendLogCommand(Id, new LogMessege() { Text = $"Player moved to {hero.Position}"}));
                        return;
                    }
                    viewCommands.Add(new SendLogCommand(Id, new LogMessege() { Text = $"You cant move into a wall or enemy!"}));
                    
                }
            }
        }
    }

}