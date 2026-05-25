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
        while (_cts.IsCancellationRequested == false)
        {
            while (_commands.TryTake(out IModelCommand? command))
            {
                var context = _clientStates.GetClientGameContext(command.Id);
                if(command.CanExecute(context))
                    command.Execute(context, _viewCommands);
            }
            
            Thread.Sleep(16);
        }
    }
}