namespace ConsoleApp1.FightLoop.ChainOfKeyOperations;
using ConsoleApp1.Logger;

public class HitNode : KeyNode
{
    public HitNode(Hero hero, Enemy enemy, CancellationTokenSource cts) : 
        base(hero, enemy, cts) { }

    public override void HandleKey(ConsoleKey keyInfo)
    {
        var instruction = new InstructionBuilder(Hero);
        if (keyInfo == ConsoleKey.H)
        {
            while (true)
            {
                instruction.PrintAttackInstruction();
                var attackType = Console.ReadKey(true);

                IAttackVisitor? visitor = attackType.Key switch
                {
                    ConsoleKey.D1 => new NormalAttack(),
                    ConsoleKey.D2 => new StealthAttack(),
                    ConsoleKey.D3 => new MagicAttack(),
                    _ => null
                };

                if (visitor == null)
                {
                    Render.RenderAnnouncement("Key not recognised");
                    continue;
                }

                int damage = CalculateTotal(visitor, true);
                int defence = CalculateTotal(visitor, false);

                Enemy.ReceiveDamage(damage);
                EventLog el = EventLog.GetEventLog();
                el.Log(LogType.HeroHits, [Enemy.Name, damage.ToString()]);
                
                int damageNetto = Enemy.Damage - defence;
                damageNetto = damageNetto > 0 ? damageNetto : 0;
                Hero.Stats.Health -= damageNetto;
                el.Log(LogType.EnemyHits, [Enemy.Name, damageNetto.ToString()]);
                break;
            }

            Render.RenderEnemyStats(Enemy);
            Render.RenderStats(Hero);
            if (Hero.Stats.Health <= 0)
            {
                Render.RenderGameOver();
                _cts.Cancel();
                
                EventLog el =  EventLog.GetEventLog();
                el.Log(LogType.DefeatedHero, [Enemy.Name]);
                return;
            }
            else if(Enemy.Hp <= 0)
            {
                Enemy.Die();
                _cts.Cancel();
                Render.RenderAnnouncement("Enemy defeated");
                
                EventLog el =  EventLog.GetEventLog();
                el.Log(LogType.DefeatedEnemy, [Enemy.Name]);
            }
            return;
        }
        NextKeyNode.HandleKey(keyInfo);
    }

    private int CalculateTotal(IAttackVisitor visitor, bool isDamage)
    {
        var left = Hero.Hands.LeftHand;
        var right = Hero.Hands.RightHand;

        if (left == null && right == null)
        {
            return isDamage
                ? visitor.CalculateDefaultDamage(Hero.Stats)
                : visitor.CalculateDefaultDefence(Hero.Stats);
        }

        if (left == right)
            return isDamage
                ? left.AcceptDamage(visitor, Hero.Stats)
                : left.AcceptDefense(visitor, Hero.Stats);

        int total = 0;
        if (left != null)
            total += isDamage
                ? left.AcceptDamage(visitor, Hero.Stats)
                : left.AcceptDefense(visitor, Hero.Stats);
        if (right != null)
            total += isDamage
                ? right.AcceptDamage(visitor, Hero.Stats)
                : right.AcceptDefense(visitor, Hero.Stats);
        
        return total;
    }
}