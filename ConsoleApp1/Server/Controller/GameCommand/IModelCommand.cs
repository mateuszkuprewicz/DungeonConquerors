using System.Collections.Concurrent;
using ConsoleApp1.GameState;
using ConsoleApp1.Server.View.ViewCommand;

namespace ConsoleApp1.Server.Controller.Command;

public interface IModelCommand
{
    public void Execute(BlockingCollection<IViewCommand> viewCommands);
    public bool CanExecute();
    public int Id { get; }
}