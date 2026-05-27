using System.Text.Json;

namespace ConsoleApp1.NetworkController;
using ConsoleApp1.Shared.ShallowModel;

public class InitHandler : IMessageHandler
{
    private GameState _state;
    private Render _render;
    private ShallowMap? _receivedMap;

    public InitHandler(GameState state, string serialisedObject, Render render)
    {
        if (serialisedObject == null) throw new Exception("serialisedObject is null");
        _receivedMap = JsonSerializer.Deserialize<ShallowMap>(serialisedObject);
        _state = state;
        _render = render;
    }
    
    public void Handle()
    {
        if (_receivedMap == null) return;
        _state.Map = _receivedMap;
        _render.RenderMap();
        _render.RenderEnemies();
        
    }
}