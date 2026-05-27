using ConsoleApp1.Shared.ClientServerCommunication.ServerRequests;

namespace ConsoleApp1.NetworkController;
using Shared.ShallowModel;

public class DeserialisingDtoFactory
{
    private GameState _state;
    private Render _view;

    // Fabryka trzyma mapę i renderer w polach prywatnych
    public DeserialisingDtoFactory(GameState state, Render view)
    {
        _state = state;
        _view = view;
    }

    public IMessageHandler? GetHandler(string type, string text)
    {
        return type switch
        {
            ServerRequestsTypes.ActualiseMap => new InitHandler(_state, text, _view),
            ServerRequestsTypes.PlayerCreation => new PlayerCreationHandler(_state, text, _view),
            _ => null
        };
    }
    
}