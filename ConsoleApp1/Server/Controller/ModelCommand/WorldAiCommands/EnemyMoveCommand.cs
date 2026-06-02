using System.Collections.Concurrent;
using System.Collections.Generic;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.Controller.Command.WorldAiCommands;

public class EnemyMoveCommand : IModelCommand
{
    public int Id { get; }
    private GameMap _map;

    public EnemyMoveCommand(int id, GameMap map)
    {
        Id = id;
        _map = map;
    }

    public bool CanExecute()
    {
        return true;
    }

    public void Execute(BlockingCollection<IViewCommand> viewCommands)
    {
        foreach (var enemy in _map.enemies)
        {
            if (enemy != null && enemy.Id == Id)
            {
                var prevPos = enemy.Position;
                enemy.Move();
                var nextPos = enemy.Position;
                
                var deltaMessage = new DeltaUpdateMessage();
                deltaMessage.Deltas = new List<MapDelta>();
                deltaMessage.UpdatedHeroes = new List<ShallowHero>();
                
                var deltaPrevPos = new MapDelta();
                deltaPrevPos.X = prevPos.X;
                deltaPrevPos.Y = prevPos.Y;
                deltaPrevPos.Enemy = null;
                deltaPrevPos.Item = _map.map[deltaPrevPos.Y, deltaPrevPos.X] == null || _map.map[deltaPrevPos.Y, deltaPrevPos.X].Count == 0
                    ? null
                    : new ShallowItem()
                    {
                        Name = _map.map[deltaPrevPos.Y, deltaPrevPos.X].Peek().Name,
                        Symbol = _map.map[deltaPrevPos.Y, deltaPrevPos.X].Peek().Symbol,
                    };
                
                var deltaNextPos = new MapDelta();
                deltaNextPos.X = nextPos.X;
                deltaNextPos.Y = nextPos.Y;
                deltaNextPos.Enemy = new ShallowEnemy()
                {
                    Hp = enemy.Hp,
                    Id = enemy.Id,
                    Name = enemy.Name,
                    Symbol = enemy.Symbol,
                    Pos = new Position(enemy.Position.X, enemy.Position.Y),
                };
                deltaNextPos.Item = _map.map[deltaNextPos.Y, deltaNextPos.X] == null || _map.map[deltaNextPos.Y, deltaNextPos.X].Count == 0
                    ? null
                    : new ShallowItem()
                    {
                        Name = _map.map[deltaNextPos.Y, deltaNextPos.X].Peek().Name,
                        Symbol = _map.map[deltaNextPos.Y, deltaNextPos.X].Peek().Symbol,
                    };
                
                if (prevPos.X != nextPos.X || prevPos.Y != nextPos.Y)
                {
                    deltaMessage.Deltas.Add(deltaPrevPos);
                }
                
                deltaMessage.Deltas.Add(deltaNextPos);
                
                var mapDeltaCommand = new MapDeltaCommand(deltaMessage);
                
                viewCommands.Add(mapDeltaCommand);
                return;
            }
        }
    }
}