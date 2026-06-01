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
    private NewClient _newClientDto;

    public PlayerCreationHandler(Shared.ShallowModel.GameState state, string serialisedObject, Render render)
    {
        if (serialisedObject == null) throw new Exception("serialisedObject is null");
        _newClientDto = JsonSerializer.Deserialize<NewClient>(serialisedObject);
        _state = state;
        _render = render;
    }

    public void Handle()
    {
        var logger = EventLog.GetEventLog();
        if (_newClientDto.Id == _state.Map.PlayerId)
        {
            _state.Hero = new ShallowHero(_state.Map.PlayerId, (_newClientDto.X, _newClientDto.Y));
            logger.Log("My hero initialised");
            _render.RenderAll();
        }
        else
        {
            _state.Map.Heroes.Add(new ShallowHero(_newClientDto.Id, (_newClientDto.X, _newClientDto.Y)));
            logger.Log("Someone else hero initialised");
            _render.RenderAll();
        }
    }
}