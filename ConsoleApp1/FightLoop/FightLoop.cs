namespace ConsoleApp1.FightLoop;
using ConsoleApp1.FightLoop.ChainOfKeyOperations;

public class FightLoop
{
    private Hero _hero;
    private Enemy _enemy;
    private CancellationTokenSource _cts;
    private Render _render;

    public FightLoop(Hero hero, Enemy enemy, Render render)
        => (_hero, _enemy, _render) = (hero, enemy,  render);

    public void Loop()
    {
        _cts = new CancellationTokenSource();
        InstructionBuilder instruction = new InstructionBuilder(_hero);
        KeyNode hit = new HitNode(_hero, _enemy, _cts, _render);
        KeyNode run = new LeaveNode(_hero, _enemy,  _cts, _render);
        KeyNode sentinel = new Sentinel(_hero, _enemy, _cts);
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