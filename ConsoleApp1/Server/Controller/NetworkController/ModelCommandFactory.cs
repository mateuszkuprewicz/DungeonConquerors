using System.Text.Json;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.Server.Controller.Command;
using ConsoleApp1.Server.Model;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1.Server;

public class ModelCommandFactory
{
    private GameContext _gameContext;

    public ModelCommandFactory(GameContext gameContext)
    {
        _gameContext = gameContext;
    }

    public IModelCommand GetInit(int id)
    {
        return new InitHeroCommand(id, _gameContext);
    }
    
    public IModelCommand GetModelCommend(int id, string type, string text)
    {
        switch (type)
        {
            case ClientRequestsTypes.ClientMove:
            {
                var move = JsonSerializer.Deserialize<ClientMove>(text);
                return new MovePlayerCommand(id, move.Direction, _gameContext);
            }
            
            default:
                return null;
        }
    }
}