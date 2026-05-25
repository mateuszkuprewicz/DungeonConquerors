using System.Collections.Concurrent;
using ConsoleApp1.GameState;
using ConsoleApp1.Server.View.ViewCommand;

namespace ConsoleApp1.Server.Controller.Command;

public interface IModelCommand
{
    public void Execute(GameStateContext? context, BlockingCollection<IViewCommand> viewCommands);
    public bool CanExecute(GameStateContext? context);
    public int Id { get; }
}