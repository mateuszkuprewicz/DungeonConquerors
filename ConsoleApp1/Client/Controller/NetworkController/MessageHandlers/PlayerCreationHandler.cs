using System.Diagnostics;
using System.Text.Json;
using ConsoleApp1.Logger;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.NetworkController;

public class PlayerCreationHandler : IMessageHandler
{
    private Render _render;
    private Shared.ShallowModel.GameState _state;
    private NewPlayer _newPlayerDto;

    public PlayerCreationHandler(Shared.ShallowModel.GameState state, string serialisedObject, Render render)
    {
        if (serialisedObject == null) throw new Exception("serialisedObject is null");
        _newPlayerDto = JsonSerializer.Deserialize<NewPlayer>(serialisedObject);
        _state = state;
        _render = render;
    }

    public void Handle()
    {
        var logger = EventLog.GetEventLog();
        if (_newPlayerDto.Id == _state.Map.PlayerId)
        {
            _state.Hero = new ShallowHero(_state.Map.PlayerId, (_newPlayerDto.X, _newPlayerDto.Y));
            logger.Log("My hero initialised");
            _render.RenderAll();
        }
        else
        {
            _state.Map.Heroes.Add(new ShallowHero(_newPlayerDto.Id, (_newPlayerDto.X, _newPlayerDto.Y)));
            logger.Log("Someone else hero initialised");
            _render.RenderAll();
        }
    }
}