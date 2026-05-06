namespace ConsoleApp1.MovingAI;

public interface IMovingEnemy
{
    public AbstractMovingState MovingState { set; }
    public (int X, int Y) Position { get; set; }
    public void Move();
}