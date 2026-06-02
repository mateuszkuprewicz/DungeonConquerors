using System.Collections.Concurrent;
using ConsoleApp1.DTO.ClientRequests;
using ConsoleApp1.FightLoop.Visitor.AttackTypesVisitor;
using ConsoleApp1.Server.Controller.Command.CombatCommands;
using ConsoleApp1.Server.Model;
using ConsoleApp1.Server.View.ViewCommand;
using ConsoleApp1.Shared;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.Shared.ShallowModel;

namespace ConsoleApp1.Server.Controller.Command;

public class HitCommand : AbstractCombatCommand, IModelCommand
{
    private HitType? _hitType;
    public HitCommand(int id, GameContext gameContext, HitType? hitType)
    {
        Id = id;
        _gameContext = gameContext;
        _hitType = hitType;
    }

    public void Execute(BlockingCollection<IViewCommand> viewCommands)
    {
        if (_gameContext.Map == null)
        {
            Console.WriteLine(
                "[KRYTYCZNY BŁĄD] Obiekt _map w MovePlayerCommand jest NULLEM! Sprawdź konstruktor i fabrykę.");
            return;
        }

        var map = _gameContext.Map;

        Hero? hero = null;
        Enemy? enemy = null;
        for (int i = 0; i < ModelConsts.MapHeight; i++)
        {
            for (int j = 0; j < ModelConsts.MapWidth; j++)
            {
                if (map.heroes[i, j] != null && map.heroes[i, j].Id == Id)
                {
                    hero = map.heroes[i, j];
                    enemy = map.enemies[i, j];
                    goto SKIP;
                }
            }
        }
        SKIP: ;
        if (enemy == null)
        {
            Console.Error.WriteLine("[Error] Bug in changing hero's state");
            return;
        }
        if (hero == null)
        {
            Console.Error.WriteLine("[Error] hero is null");
            return;
        }
        if (_hitType == null)
        {
            Console.Error.WriteLine("[Error] HitType is null");
        }

        IAttackVisitor? visitor = _hitType switch
        {
            HitType.HeavyAttack => new NormalAttack(),
            HitType.SneakyAttack => new StealthAttack(),
            HitType.MagicAttack => new MagicAttack(),
            _ => null
        };

        if (visitor == null) return;
        
        int damage = CalculateTotal(visitor, hero, true);
        int defence = CalculateTotal(visitor,  hero, false);
        
        enemy.ReceiveDamage(damage);
        viewCommands.Add(new SendLogCommand(Id, new LogMessege() {Text = $"{enemy.Name} received {damage} damage"}));

        int damageNetto = enemy.Damage - defence;
        damageNetto = damageNetto > 0 ? damageNetto : 0;
        hero.Stats.Health -= damageNetto;
        viewCommands.Add(new SendLogCommand(Id, new LogMessege() {Text = $"You received {damageNetto} damage from {enemy.Name}"}));

        var position = hero.Position;
        ;
        if (hero.Stats.Health <= 0)
        {
            map.heroes[hero.Position.Y, hero.Position.X] = null;
            hero.Position = (-1, -1);
        }

        if (enemy.Hp <= 0)
        {
            enemy.Die();
            map.enemies[enemy.Position.Y, enemy.Position.X] = null;
            enemy = null;
        }
        
        DeltaUpdateMessage deltaUpdateMessage = new DeltaUpdateMessage();
        deltaUpdateMessage.Deltas = new List<MapDelta>();
        deltaUpdateMessage.UpdatedHeroes = new List<ShallowHero>();
        deltaUpdateMessage.UpdatedHeroes.Add(hero != null ? hero.ToShallowHero() : null);
        deltaUpdateMessage.Deltas.Add(new MapDelta()
        {
            X = position.X,
            Y = position.Y,
            Item = map.map[position.Y, position.X].Count > 0 && map.map[position.Y, position.X].Peek() != null ? new ShallowItem()
            {
                Name = map.map[position.Y, position.X].Peek().Name,
                Symbol = map.map[position.Y, position.X].Peek().Symbol
            } : null,
            Enemy = enemy != null ? new ShallowEnemy()
            {
                Id = enemy.Id,
                Hp = enemy.Hp,
                Name = enemy.Name,
                Symbol = enemy.Symbol
            } : null
        });
        
        viewCommands.Add(new MapDeltaCommand(deltaUpdateMessage));
    }

    private int CalculateTotal(IAttackVisitor visitor, Hero hero, bool isDamage)
    {
        var left = hero.Hands.LeftHand;
        var right = hero.Hands.RightHand;

        if (left == null && right == null)
        {
            return isDamage
                ? visitor.CalculateDefaultDamage(hero.Stats)
                : visitor.CalculateDefaultDefence(hero.Stats);
        }

        if (left == right)
            return isDamage
                ? left.AcceptDamage(visitor, hero.Stats)
                : left.AcceptDefense(visitor, hero.Stats);

        int total = 0;
        if (left != null)
            total += isDamage
                ? left.AcceptDamage(visitor, hero.Stats)
                : left.AcceptDefense(visitor, hero.Stats);
        if (right != null)
            total += isDamage
                ? right.AcceptDamage(visitor, hero.Stats)
                : right.AcceptDefense(visitor, hero.Stats);
        
        return total;
    }
    
}

