namespace ConsoleApp1.SoundPropagation.SoundMediation;

public interface ISoundSubscribtion
{
    public void Subscribe(ISoundHearer hearer);
    public void Unsubscribe(ISoundHearer hearer);
}