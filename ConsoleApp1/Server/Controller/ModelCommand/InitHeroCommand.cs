using System.Collections.Concurrent;
using System.Windows.Input;
using ConsoleApp1.GameState;
using ConsoleApp1.Server.ClientStates;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.Controller.Command;

public class InitHeroCommand : IModelCommand
{
    public int Id { get; }
    private GameMap _map;
    private IControllerClientState _clientStates;

    public InitHeroCommand(int id, GameMap map, IControllerClientState clientStates)
    {
        Id = id;
        _map = map;
        _clientStates = clientStates;
    }

    public bool CanExecute(GameStateContext context)
    {
        for (int i = 0; i < ModelConsts.MapHeight; i++)
        {
            for (int j = 0; j < ModelConsts.MapWidth; j++)
            {
                if (_map.map[i, j] != null && _map.enemies[i, j] == null && _map.heroes[i, j] == null)
                    return true;
            }
        }
        return false;
    }
    
    public void Execute(GameStateContext context, BlockingCollection<IViewCommand> viewCommands)
    {
        //zamien na tworzenie odpowiedniego DTO
        string mapJson = "{\"TestMapMessage\": \"Zaraz tu bedzie zserializowana mapa DTO\"}";
        
        var map = MapShallower(_map);
        
        viewCommands.Add(new SendMapViewCommand(Id, map));
        Console.WriteLine($"[GameLoop] Zainicjalizowano gracza {Id} i wrzucono mapę do wysyłki.");
        
        //find pos
        //context.Update();
    }

    private ShallowMap MapShallower(GameMap map)
{
    int height = map.map.GetLength(0);
    int width = map.map.GetLength(1);

    var shallowTypes = new TyleType[height][];
    var shallowItems = new ShallowItem?[height][];
    var shallowEnemies = new ShallowEnemy?[height][];
    var shallowHeroes = new List<ShallowAnotherHero>();

    for (int y = 0; y < height; y++)
    {
        shallowTypes[y] = new TyleType[width];
        shallowItems[y] = new ShallowItem?[width];
        shallowEnemies[y] = new ShallowEnemy?[width];

        for (int x = 0; x < width; x++)
        {
            var itemStack = map.map[y, x];
            
            if (itemStack == null)
            {
                shallowTypes[y][x] = TyleType.Wall;
            }
            else
            {
                shallowTypes[y][x] = TyleType.Normal;

                if (itemStack.Count > 0)
                {
                    var topItem = itemStack.Peek();
                    shallowItems[y][x] = new ShallowItem
                    {
                        Name = topItem.Name,
                        Symbol = topItem.Symbol
                    };
                }
            }

            var enemy = map.enemies[y, x];
            if (enemy != null)
            {
                shallowEnemies[y][x] = new ShallowEnemy
                {
                    Id = enemy.Id, 
                    Hp = enemy.Hp,
                    Name = enemy.Name,
                    Symbol = enemy.Symbol,
                    Pos = new Position(x, y)
                };
            }

            var hero = map.heroes[y, x];
            if (hero != null)
            {
                shallowHeroes.Add(new ShallowAnotherHero
                {
                    ID = hero.Id,
                    Name = hero.Id.ToString()[0], 
                    Pos = new Position(x, y)
                });
            }
        }
    }

    return new ShallowMap(shallowItems, shallowTypes, shallowEnemies, shallowHeroes);
}

}