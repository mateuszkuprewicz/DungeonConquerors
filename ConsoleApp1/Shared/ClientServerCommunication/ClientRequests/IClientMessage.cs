namespace ConsoleApp1.DTO.ClientRequests;

public interface IClientMessage
{
    
}

public enum ClientMessageType
{
    ClientMove,
    ClientPickUp,
    ClientDrop,
    ClientEquip,
    ClientUnequip,
    ClientHit
}