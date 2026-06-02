using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ConsoleApp1.Server.Model;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.ClientServerCommunication.ServerRequests;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.Controller.Command;

public class PickUpCommand : AbstractExplorationCommand, IModelCommand
{
    public PickUpCommand(int id, GameContext gameContext)
    {
        Id = id;
        _gameContext = gameContext;
    }

    public void Execute(BlockingCollection<IViewCommand> viewCommands)
    {
        Action<string> soundLogHandler = (logMsg) => 
        {
            LogMessege temp = new LogMessege();
            temp.Text = logMsg;
            viewCommands.Add(new SendLogCommand(ServerConsts.BroadcastId, temp));
        };

        _gameContext.SoundManager.OnSoundLogGenerated += soundLogHandler;

        try
        {
            if (_gameContext.Map == null) return;
            
            var map = _gameContext.Map;

            for (int i = 0; i < ModelConsts.MapHeight; i++)
            {
                for (int j = 0; j < ModelConsts.MapWidth; j++)
                {
                    if (map.heroes[i, j] != null && map.heroes[i, j].Id == Id)
                    {
                        var hero = map.heroes[i, j];
                        
                        (int compl, Item? item) = hero.Equipment.PickItem((j, i), map);
                        if (compl == 1)
                        {
                            Console.Error.WriteLine($"Player {hero.Id} picked up {item.Name}");
                            
                            DeltaUpdateMessage deltaUpdateMessage = new DeltaUpdateMessage();
                            deltaUpdateMessage.Deltas = new List<MapDelta>();
                            deltaUpdateMessage.UpdatedHeroes = new List<ShallowHero>();
                            
                            MapDelta tyleDelta = new MapDelta();
                            tyleDelta.X = j;
                            tyleDelta.Y = i;
                            
                            var newItem = map.map[i,j] == null || map.map[i,j]!.Count == 0 ? null : map.map[i,j]!.Peek();
                            ShallowItem? shallowItem = null;
                            if(newItem != null)
                            {
                                shallowItem = new ShallowItem();
                                shallowItem.Name = newItem.Name;
                                shallowItem.Symbol = newItem.Symbol;
                            }
                            tyleDelta.Item = shallowItem;
                            
                            deltaUpdateMessage.Deltas.Add(tyleDelta);
                            deltaUpdateMessage.UpdatedHeroes.Add(hero.ToShallowHero());
                            
                            viewCommands.Add(new MapDeltaCommand(deltaUpdateMessage));
                            viewCommands.Add(new SendLogCommand(Id, new LogMessege() { Text = $"Player picked up an item {item.Name}" }));
                            return;
                        }
                    }
                }
            }
        }
        finally
        {
            _gameContext.SoundManager.OnSoundLogGenerated -= soundLogHandler;
        }
    }  
}