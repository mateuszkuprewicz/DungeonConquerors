namespace ConsoleApp1.FightLoop;
using ConsoleApp1.FightLoop.ChainOfKeyOperations;

public class FightLoop
{
    private Hero Hero;
    private Enemy Enemy;
    private CancellationTokenSource _cts;

    public FightLoop(Hero hero, Enemy enemy)
        => (Hero, Enemy) = (hero, enemy);

    public void Loop()
    {
        _cts = new CancellationTokenSource();
        InstructionBuilder instruction = new InstructionBuilder(Hero);
        KeyNode hit = new HitNode(Hero, Enemy, _cts);
        KeyNode run = new LeaveNode(Hero, Enemy,  _cts);
        KeyNode sentinel = new Sentinel(Hero, Enemy, _cts);
        hit.SetNextHandler(run);
        run.SetNextHandler(sentinel);
        
        while (!_cts.IsCancellationRequested)
        {
            instruction.PrintInstructionInFightLoop();
            var key = Console.ReadKey(true);
            hit.HandleKey(key.Key);
        }
    }
}