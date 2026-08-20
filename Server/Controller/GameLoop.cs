using System.Collections.Concurrent;
using ConsoleApp1.GameState;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.Controller.Command;
using ConsoleApp1.Server.View.ViewCommand;

namespace ConsoleApp1.Server;

public class GameLoop
{
    private BlockingCollection<IModelCommand> _commands;
    private BlockingCollection<IViewCommand> _viewCommands;
    CancellationTokenSource _cts;

    public GameLoop(BlockingCollection<IModelCommand> commands, BlockingCollection<IViewCommand> viewCommands, CancellationTokenSource cts)
    {
        _commands = commands;
        _viewCommands = viewCommands;
        _cts = cts;
    }

    public void Run()
    {
        Console.WriteLine("[GameLoop] Pętla główna wystartowała.");
        try
        {
            foreach (var command in _commands.GetConsumingEnumerable(_cts.Token))
            {
                if (command.CanExecute())
                {
                    command.Execute(_viewCommands);
                    Console.WriteLine($"[GameLoop] Wykonano polecenie typu {command.GetType()}");
                }
                else
                {
                    Console.WriteLine($"[GameLoop] Odmowa! Nie wykonano polecenia typu {command.GetType()}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("[GameLoop] Zamknięto pętlę gry (Token anulowany).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[KRYTYCZNY BŁĄD GAMELOOP] Pętla gry padła!");
            Console.WriteLine($"Wiadomość: {ex.Message}");
            Console.WriteLine($"Gdzie: {ex.StackTrace}");
        }
    }
}