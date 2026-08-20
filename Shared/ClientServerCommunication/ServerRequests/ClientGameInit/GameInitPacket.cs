namespace ConsoleApp1.Shared.ClientServerCommunication.ServerAnswers.ClientGameInit;
using ConsoleApp1.Shared.ShallowModel;

public class GameInitPacket 
{ 
    public ShallowMap Map { get; set; }
    public Position Pos { get; set; }
    public int PlayerId { get; set; }
}