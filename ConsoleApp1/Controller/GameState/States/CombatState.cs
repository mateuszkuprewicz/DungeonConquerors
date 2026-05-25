using ConsoleApp1.ChainOfKeyOperations;
using ConsoleApp1.GameState;
using ConsoleApp1.View;

namespace ConsoleApp1.LoopState;

public class CombatState : IGameState
{
    private Hero _hero;
    private GameMap _map;
    private AbstractKeyNode _inputChain;
    private Render _render;
    private LogRenderer _logRenderer;
    private GameStateContext _stateContext;
    private Enemy _enemy;
    
    public CombatState(Hero hero, GameMap map, Enemy enemy, Render render, LogRenderer logRenderer, GameStateContext stateContext)
    {
        _hero = hero;
        _map = map;
        _render = render;
        _logRenderer = logRenderer;
        _stateContext = stateContext;
        _enemy = enemy;
        
        AbstractKeyNode hit = new HitNode(_hero, enemy, render);
        AbstractKeyNode run = new LeaveNode(_hero, enemy, map, render);
        AbstractKeyNode log = new LogChangeViewNode(logRenderer, render);
        AbstractKeyNode sentinel = new Sentinel();
        hit.SetNextHandler(run);
        run.SetNextHandler(log);
        log.SetNextHandler(sentinel);
        
        _inputChain = hit;
    }

    public void HandleInput(ConsoleKey key)
    {
        _inputChain.HandleKey(key);
    }

    public void Render()
    {
        //_render.RenderMap();
        //_render.RenderEnemies();
        if (_logRenderer.IsRenderingAllLogs) _logRenderer.RenderAll();
        else _logRenderer.RenderLast();
    }

    public void Update()
    {
        HashSet<Enemy> enemies = new HashSet<Enemy>();
        foreach (var enemy in _map.enemies)
        {
            if (enemy != null && enemy !=_enemy) enemies.Add(enemy);
        }

        foreach (var enemy in enemies)
        {
            (int X, int Y) prev_pos = (enemy.Position.X, enemy.Position.Y);
            enemy.Move();
            _render.ActualiseAfterEnemyMove(prev_pos, enemy);
        }
        
        if (_map.enemies[_hero.Position.Y, _hero.Position.X] == null)
        {
            _stateContext.GameState = new ExplorationState(_map, _hero, _render, _logRenderer, _stateContext);
        }
    }
}