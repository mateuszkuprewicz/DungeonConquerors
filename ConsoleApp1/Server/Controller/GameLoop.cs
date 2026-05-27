using System.Collections.Concurrent;
using ConsoleApp1.GameState;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.Controller.Command;
using ConsoleApp1.Server.View.ViewCommand;

namespace ConsoleApp1.Server;

public class GameLoop
{
    private IControllerClientState _clientStates;
    private BlockingCollection<IModelCommand> _commands;
    private BlockingCollection<IViewCommand> _viewCommands;
    CancellationTokenSource _cts;

    public GameLoop(IControllerClientState clientStates, BlockingCollection<IModelCommand> commands, BlockingCollection<IViewCommand> viewCommands,CancellationTokenSource cts)
    {
        _clientStates = clientStates;
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
                var context = _clientStates.GetClientGameContext(command.Id);
            
                if (context == null)
                {
                    Console.WriteLine($"[GameLoop ERROR] Pobrano kontekst NULL dla gracza o ID: {command.Id}. Pomijam komendę {command.GetType()}.");
                    continue; // Przejdź do następnej komendy w kolejce, nie wywalaj pętli!
                }
                if (command.CanExecute(context))
                {
                    command.Execute(context, _viewCommands);
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