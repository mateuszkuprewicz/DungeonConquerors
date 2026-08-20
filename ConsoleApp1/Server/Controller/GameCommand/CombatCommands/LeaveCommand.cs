using System.Collections.Concurrent;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.Items.Weapon;
using ConsoleApp1.Server.Controller.Command;
using ConsoleApp1.Server.Controller.Command.CombatCommands;
using ConsoleApp1.Server.Model;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.Controller.ModelCommand.CombatCommands;

public class LeaveCommand : AbstractCombatCommand, IModelCommand
{
    public LeaveCommand(int id, GameContext gameContext)
    {
        Id = id;
        _gameContext = gameContext;
    }

    public void Execute(BlockingCollection<IViewCommand> viewCommands)
    {
        var map = _gameContext.Map;

        Hero? hero = null;
        Enemy? enemy = null;
        for (int i = 0; i < ModelConsts.MapHeight; i++)
        {
            for (int j = 0; j < ModelConsts.MapWidth; j++)
            {
                if (map.heroes[i, j] != null && map.heroes[i, j]!.Id == Id)
                {
                    hero = map.heroes[i, j];
                    enemy = map.enemies[i, j];
                    goto SKIP;
                }
            }
        }
        SKIP: ;
        if (enemy == null)
        {
            Console.Error.WriteLine("[Error] Bug in changing hero's state");
            return;
        }
        if (hero == null)
        {
            Console.Error.WriteLine("[Error] hero is null");
            return;
        }
        
        int damageNetto = enemy.Damage;
        damageNetto = damageNetto > 0 ? damageNetto : 0;
        hero.Stats.Health -= damageNetto;
        if (hero.Stats.Health <= 0)
        {
            map.heroes[hero.Position.Y, hero.Position.X] = null;
            hero.Position = (-1, -1);
        }
        
        viewCommands.Add(new SendLogCommand(Id, new LogMessege() {Text = $"You received {damageNetto} damage from {enemy.Name}"}));
        
        List<(int,int)> deltas = new List<(int,int)>(){(-1, 0), (1, 0), (0, -1), (0, 1)};
        foreach (var (i, j) in deltas)
        {
            int Y = hero.Position.Y + j;
            int X = hero.Position.X + i;
            Console.WriteLine($"[LeaveCommand] Y: {Y}, X: {X}]");
            if (Hero.IsPositionValid((X, Y), map) && map.enemies[Y, X] == null)
            {
                Console.WriteLine("[GameLoop] New Position is valid");
                Direction direction = Direction.Down;
                switch ((i, j))
                {
                    case (-1, 0):  
                        direction = Direction.Left;
                        break;
                    case (1, 0):
                        direction = Direction.Right;
                        break;
                    case (0, -1):
                        direction = Direction.Up;
                        break;
                    case (0, 1):
                        direction = Direction.Down;
                        break;
                }

                int prevY = hero.Position.Y;
                int prevX = hero.Position.X;
                
                if (hero.Move(direction, map))
                {
                    Console.Error.WriteLine("after Hero Move");
                    
                    map.heroes[prevY, prevX] = null;
                    map.heroes[Y, X] = hero;
                    
                    Console.WriteLine($"[LeftCommand] Player {hero.Id} moved to {hero.Position}");
                    DeltaUpdateMessage deltaUpdateMessage = new DeltaUpdateMessage();
                    deltaUpdateMessage.Deltas = new List<MapDelta>();
                    List<(int,ShallowHero?)> deltaHeroes = new List<(int,ShallowHero?)>();
                    deltaHeroes.Add((hero.Id, hero.ToShallowHero()));
                    deltaUpdateMessage.UpdatedHeroes = deltaHeroes;
                        
                    viewCommands.Add(new MapDeltaCommand(deltaUpdateMessage));
                    viewCommands.Add(new SendLogCommand(Id, new LogMessege() { Text = $"Player run from enemy to {hero.Position}"}));
                }
                
                return;
            }
        }
        
    }
}