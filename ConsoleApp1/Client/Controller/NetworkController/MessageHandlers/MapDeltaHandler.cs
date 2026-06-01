using System.Text.Json;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;

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
        _deltaUpdateMessage = JsonSerializer.Deserialize<DeltaUpdateMessage>(serialisedObject);
    }
    public void Handle()
    {
        foreach (var delta in _deltaUpdateMessage.Deltas)
        {
            _state.Map.Map[delta.Y][delta.X] = delta.Item;
            _state.Map.Enemies[delta.Y][delta.X] = delta.Enemy;
            
            _render.UpdateSingleTile(delta.X, delta.Y);
        }

        foreach (var hero in _deltaUpdateMessage.UpdatedHeroes)
        {
            int id = hero.Id;
            if (id == _state.Map.PlayerId)
            {
                var prevHeroPos = _state.Hero?.Pos;
                _state.Hero = hero;
                
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
                        var prevPos = enemyHero.Pos;
                        _state.Map.Heroes[i] = hero;
                        if (prevPos.X != hero.Pos.X || prevPos.Y != hero.Pos.Y)
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