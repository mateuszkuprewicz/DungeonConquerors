using System;
using System.Threading;
using System.Text.Json;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.NetworkController;

public class MapDeltaHandler : IMessageHandler
{
    private Render _render;
    private Shared.ShallowModel.GameState _state;
    private DeltaUpdateMessage _deltaUpdateMessage;

    public MapDeltaHandler(Shared.ShallowModel.GameState state, string serialisedObject, Render render)
    {
        _state = state;
        _render = render;
        if (serialisedObject == null) throw new Exception("serialisedObject is null");
        var options = new JsonSerializerOptions { IncludeFields = true };
        _deltaUpdateMessage = JsonSerializer.Deserialize<DeltaUpdateMessage>(serialisedObject, options);
    }
    
    public void Handle()
    {
        bool menuNeedsUpdate = false;

        if (_deltaUpdateMessage.Deltas.Count > 0)
        {
            foreach (var delta in _deltaUpdateMessage.Deltas)
            {
                _state.Map.Map[delta.Y][delta.X] = delta.Item;
                _state.Map.Enemies[delta.Y][delta.X] = delta.Enemy;
                
                _render.UpdateSingleTile(delta.X, delta.Y);

                if (_state.Hero != null && _state.Hero.Pos.X == delta.X && _state.Hero.Pos.Y == delta.Y)
                {
                    _render.RenderMenu();
                }
            }
        }

        if (_deltaUpdateMessage.UpdatedHeroes.Count > 0)
        {
            foreach (var tuple in _deltaUpdateMessage.UpdatedHeroes)
            {
                int id = tuple.Id;
                var hero =  tuple.Hero;
                if (id == _state.Map!.PlayerId)
                {
                    if (hero == null)
                    {
                        Render.RenderGameOver();
                        Thread.Sleep(500);
                        Environment.Exit(0);
                        return;
                    }
                    
                    int prevEquipPointer = _state.Hero != null ? _state.Hero.Equipment.EquipmentPointer : 0;
                    var prevHeroPos = _state.Hero?.Pos;
                    _state.Hero = hero;
                    
                    _state.Hero.Equipment.EquipmentPointer = prevEquipPointer;
                    
                    if (prevHeroPos != null && (prevHeroPos.X != hero.Pos.X || prevHeroPos.Y != hero.Pos.Y))
                    {
                        _render.UpdateSingleTile(prevHeroPos.X, prevHeroPos.Y);
                    }
                    _render.UpdateSingleTile(hero.Pos.X, hero.Pos.Y);
                    _render.RenderMenu();
                }
                else
                {
                    for (int i = 0; i < _state.Map.Heroes.Count; i++)
                    {
                        var enemyHero = _state.Map.Heroes[i];
                        if (enemyHero.Id == id)
                        {
                            if (hero == null)
                            {
                                _state.Map.Heroes.RemoveAt(i);
                            }
                            var prevPos = enemyHero.Pos;
                            _state.Map.Heroes[i] = hero!;
                            if (prevPos.X != hero!.Pos.X || prevPos.Y != hero.Pos.Y)
                            {
                                _render.UpdateSingleTile(prevPos.X, prevPos.Y);
                            }
                            _render.UpdateSingleTile(hero.Pos.X, hero.Pos.Y);
                            break;
                        }
                    }
                }
            }
        }
    }
}