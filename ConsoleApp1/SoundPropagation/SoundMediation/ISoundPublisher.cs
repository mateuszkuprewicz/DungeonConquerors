namespace ConsoleApp1.SoundPropagation.SoundMediation;

public interface ISoundPublisher
{
    public void Notify((int, int) source, int range);
}