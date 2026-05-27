using System.Collections.Concurrent;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.GameState;
using ConsoleApp1.LoopState;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1.Server.Controller.Command;

public class MovePlayerCommand : IModelCommand
{
    public int Id { get; set; }
    private GameMap _map;
    private IControllerClientState _clientStates;
    private DungeonSoundManager _soundManager;
    private Direction _direction;

    public MovePlayerCommand(int id, Direction direction, GameMap map, DungeonSoundManager manager)
    {
        Id = id;
        _direction = direction;
        _map = map;
        _soundManager = manager;
    }

    public bool CanExecute(GameStateContext context)
    {
        if (context.GameState is CombatState)
        {
            return false;
        }

        return true;
    }

    public void Execute(GameStateContext context, BlockingCollection<IViewCommand> viewCommands)
    {
        if (_map == null)
        {
            Console.WriteLine("[KRYTYCZNY BŁĄD] Obiekt _map w MovePlayerCommand jest NULLEM! Sprawdź konstruktor i fabrykę.");
            return;
        }
    
        if (_map.heroes == null)
        {
            Console.WriteLine("[KRYTYCZNY BŁĄD] Tablica _map.heroes jest NULLEM! Dodaj 'new Hero[wysokość, szerokość]' w klasie GameMap.");
            return;
        }
        for (int i = 0; i < ModelConsts.MapHeight; i++)
        {
            for (int j = 0; j < ModelConsts.MapWidth; j++) 
            {
                if (_map.heroes[i, j] != null && _map.heroes[i, j].Id == Id)
                {
                    var hero = _map.heroes[i, j];

                    if (hero.Move(_direction, _map))
                    {
                        _map.heroes[i, j] = null; 
                    
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

                        _map.heroes[newI, newJ] = hero;

                        var shallowMap = _map.MapShallower(); 
                        viewCommands.Add(new SendMapViewCommand(ServerConsts.BroadcastId, shallowMap));

                        return; 
                    }
                }
            }
        }
    }

}