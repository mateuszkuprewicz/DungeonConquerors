using System.Collections.Concurrent;
using ConsoleApp1.Server.Controller.Command;
using System;
using ConsoleApp1.Server.Controller.ModelCommand.WorldAiCommands;

namespace ConsoleApp1.Server.WorldAIController;

public class EnemyLoop
{
    private GameMap _map;
    private BlockingCollection<IModelCommand>  _commands;
    private Random rnd;
    private CancellationToken _cts;

    public EnemyLoop(GameMap map, CancellationToken cts, BlockingCollection<IModelCommand> commands)
    {
        _map = map;
        _commands = commands;
        _cts = cts;
        rnd =  new Random();
    }

    public void Run()
    {
        while (_cts.IsCancellationRequested == false)
        {
            Thread.Sleep(rnd.Next(500));
            int enemyCount = 0;
            foreach (var enemy in _map.enemies)
            {
                if (enemy != null) enemyCount++;
            }

            if (enemyCount == 0) return ;
            int chosen = rnd.Next(enemyCount);
            enemyCount = 0;
            Enemy? movingEnemy = null;
            foreach (var enemy in _map.enemies)
            {
                if (enemy != null)
                {
                    if (enemyCount == chosen)
                    {
                        movingEnemy = enemy;
                        break;
                    }
                    enemyCount++;
                }
            }

            _commands.Add(new EnemyMoveCommand(movingEnemy!.Id, _map));

        }
        
    }
}