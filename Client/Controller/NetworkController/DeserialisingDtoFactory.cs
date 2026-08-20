using ConsoleApp1.Shared.ClientServerCommunication.ServerRequests;
using ConsoleApp1.View;

namespace ConsoleApp1.NetworkController;
using Shared.ShallowModel;

public class DeserialisingDtoFactory
{
    private GameState _state;
    private Render _view;
    private LogRenderer _logRenderer;
    
    public DeserialisingDtoFactory(GameState state, Render view, LogRenderer logRenderer)
    {
        _state = state;
        _view = view;
        _logRenderer = logRenderer;
    }

    public IMessageHandler? GetHandler(string type, string text)
    {
        return type switch
        {
            ServerRequestsTypes.ActualiseMap => new InitHandler(_state, text, _view),
            ServerRequestsTypes.PlayerCreation => new PlayerCreationHandler(_state, text, _view),
            ServerRequestsTypes.MapDelta => new MapDeltaHandler(_state, text, _view),
            ServerRequestsTypes.LogMessage => new LogMessageHandler(text, _logRenderer),
            _ => null
        };
    }
    
}