using ConsoleApp1.LoopState;
using ConsoleApp1.Server.Model;

namespace ConsoleApp1.Server.Controller.Command.CombatCommands;

public class AbstractCombatCommand
{
    public int Id { get; set; }
    protected GameContext _gameContext;

    public bool CanExecute()
    {
        Hero hero = null;
        foreach (var temp in _gameContext.Map.heroes)
        {
            if (temp != null && temp.Id == Id)
            {
                hero = temp;
                break;
            }
        }

        if (hero == null) return false;
        hero.HeroStateContext.Update(hero.Position, _gameContext.Map);

        if (hero.HeroStateContext.HeroState is CombatState) return true;
        return false;
    }
}