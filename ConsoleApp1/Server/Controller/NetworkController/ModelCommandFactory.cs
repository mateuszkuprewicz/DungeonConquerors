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
                return new MoveHeroCommand(id, move.Direction, _gameContext);
            }
            case ClientRequestsTypes.ClientPickUp:
            {
                return new PickUpCommand(id, _gameContext);
            }
            case ClientRequestsTypes.ClientDrop:
            {
                int equipmentPointer = JsonSerializer.Deserialize<ClientDrop>(text) != null
                    ? JsonSerializer.Deserialize<ClientDrop>(text).ItemNumber
                    : 0;
                return new DropCommand(id, _gameContext, equipmentPointer);
            }
            case ClientRequestsTypes.ClientEquip:
            {
                int equipmentPointer = JsonSerializer.Deserialize<ClientEquip>(text) != null
                    ? JsonSerializer.Deserialize<ClientEquip>(text).ItemNumber
                    : 0;
                return new EquipCommand(id, _gameContext, equipmentPointer);
            }
            case ClientRequestsTypes.ClientUnequip:
            {
                return new UnequipCommand(id, _gameContext);
            }
            default:
                return null;
        }
    }
}