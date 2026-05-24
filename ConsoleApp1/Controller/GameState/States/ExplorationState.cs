using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.View;
using ConsoleApp1;
using ConsoleApp1.GameState;

namespace ConsoleApp1.LoopState;

public class ExplorationState : IGameState
{
    private AbstractKeyNode _inputChain;
    private GameMap _map;
    private Hero _hero;
    private Render _render;
    private LogRenderer _logRenderer;
    private GameStateContext _stateContext;

    public ExplorationState(GameMap map, Hero hero,  Render render, LogRenderer logRenderer, GameStateContext stateContext)
    {
        _map = map;
        _hero = hero;
        _render = render;
        _logRenderer = logRenderer;
        _stateContext = stateContext;
        
        AbstractKeyNode move = new MoveNode(_hero, map, render);
        AbstractKeyNode pick = new PickDropNode(_hero, map, render);
        AbstractKeyNode weaponEquip = new WeaponEquipmentNode(_hero, map, render);
        AbstractKeyNode  scroll = new EquipmentScrollNode(render);
        AbstractKeyNode log = new LogChangeViewNode(logRenderer, render);
        AbstractKeyNode sentinel = new Sentinel();
        
        move.SetNextHandler(pick);
        pick.SetNextHandler(weaponEquip);
        weaponEquip.SetNextHandler(scroll);
        scroll.SetNextHandler(log);
        log.SetNextHandler(sentinel);
        
        _inputChain = move;
    }

    public void HandleInput(ConsoleKey key)
    {
        _inputChain.HandleKey(key);
    }

    public void Update()
    {
        Enemy? my_enemy = _map.enemies[_hero.Position.Y, _hero.Position.X]; 
        if (my_enemy != null)
        {
            _stateContext.GameState = new CombatState(_hero, _map, _map.enemies[_hero.Position.Y, _hero.Position.X], _render, _logRenderer, _stateContext);
            ConsoleApp1.Render.RenderAnnouncement("You are in a figtht!");
        }
        
        HashSet<Enemy> enemies = new HashSet<Enemy>();
        foreach (var enemy in _map.enemies)
        {
            if (enemy != null && enemy!=my_enemy) enemies.Add(enemy);
        }
        
        foreach(var enemy in enemies)
            enemy.Move();
    }

    public void Render()
    {
        if (_logRenderer.IsRenderingAllLogs) _logRenderer.RenderAll();
        else _logRenderer.RenderLast();
        _render.RenderMap();
        _render.RenderEnemies();
    }
}