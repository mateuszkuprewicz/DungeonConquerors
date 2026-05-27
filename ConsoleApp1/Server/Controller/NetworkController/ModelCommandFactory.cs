using System.Text.Json;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.Server.Controller.Command;
using ConsoleApp1.SoundPropagation.SoundMediation;

namespace ConsoleApp1.Server;

public class ModelCommandFactory
{
    private GameMap _map;
    private DungeonSoundManager _soundManager;

    public ModelCommandFactory(GameMap map, DungeonSoundManager soundManager)
    {
        _map = map;
        _soundManager = soundManager;
    }
    
    public IModelCommand GetModelCommend(int id, string type, string text)
    {
        switch (type)
        {
            case ClientRequestsTypes.ClientMove:
            {
                var move = JsonSerializer.Deserialize<ClientMove>(text);
                return new MovePlayerCommand(id, move.Direction, _map, _soundManager);
            }
            
            default:
                return null;
        }
    }
}