using System.Text.Json;
using ConsoleApp1.Logger;
using ConsoleApp1.Shared.Logger;

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
        ISavingLogsStrategy savingLogsStrategy = new SavingLogs(Path.Combine("C:\\", "Users", "mateu", "Desktop", "Studia", "SEM4", "Projektowanie Obiektowe", "Gra", "Logs"), $"Client {_receivedMap.PlayerId}");
        var logger = EventLog.GetEventLog();
        logger.Initialise($"PLayer {_receivedMap.PlayerId}", savingLogsStrategy);
    }
    
    public void Handle()
    {
        if (_receivedMap == null) return;
        _state.Map = _receivedMap;
        _render.RenderAll();
         var logger = EventLog.GetEventLog();
         logger.Log("Game initialised");
        // _render.RenderDeltas();
    }
}