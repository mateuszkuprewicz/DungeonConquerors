// using ConsoleApp1.FightLoop.Visitor.AttackTypesVisitor;
// using ConsoleApp1.View;
//
// namespace ConsoleApp1.ChainOfKeyOperations;
// using ConsoleApp1.Logger;
//
// public class HitNode : AbstractKeyNode
// {
//     private Render _render;
//     private Enemy _enemy;
//
//     public HitNode(Hero hero, Enemy enemy, Render render) :
//         base(hero) => (_render, _enemy) = (render, enemy);
//
//     public override void HandleKey(ConsoleKey keyInfo)
//     {
//         if (keyInfo == KeyConsts.Hit.key)
//         {
//             while (true)
//             {
//                 InstructionRender instructionRender = new InstructionRender();
//                 instructionRender.PrintAttackInstruction();
//                 var attackType = Console.ReadKey(true);
//
//                 IAttackVisitor? visitor = attackType.Key switch
//                 {
//                     ConsoleKey.D1 => new NormalAttack(),
//                     ConsoleKey.D2 => new StealthAttack(),
//                     ConsoleKey.D3 => new MagicAttack(),
//                     _ => null
//                 };
//
//                 if (visitor == null)
//                 {
//                     Render.RenderAnnouncement("Key not recognised");
//                     continue;
//                 }
//
//                 int damage = CalculateTotal(visitor, true);
//                 int defence = CalculateTotal(visitor, false);
//
//                 _enemy.ReceiveDamage(damage);
//                 EventLog el = EventLog.GetEventLog();
//                 el.Log(LogType.HeroHits, [_enemy.Name, damage.ToString()]);
//                 
//                 int damageNetto = _enemy.Damage - defence;
//                 damageNetto = damageNetto > 0 ? damageNetto : 0;
//                 Hero.Stats.Health -= damageNetto;
//                 el.Log(LogType.EnemyHits, [_enemy.Name, damageNetto.ToString()]);
//                 break;
//             }
//
//             Render.RenderEnemyStats(_enemy);
//             _render.RenderStats();
//             if (Hero.Stats.Health <= 0)
//             {
//                 Render.RenderGameOver();
//                 
//                 EventLog el =  EventLog.GetEventLog();
//                 el.Log(LogType.DefeatedHero, [_enemy.Name]);
//                 Thread.Sleep(1000);
//                 Environment.Exit(0);
//                 //return;
//             }
//             else if(_enemy.Hp <= 0)
//             {
//                 _enemy.Die();
//                 Render.RenderAnnouncement("Enemy defeated");
//                 
//                 EventLog el =  EventLog.GetEventLog();
//                 el.Log(LogType.DefeatedEnemy, [_enemy.Name]);
//             }
//             return;
//         }
//         NextKeyNode.HandleKey(keyInfo);
//     }
//
//     private int CalculateTotal(IAttackVisitor visitor, bool isDamage)
//     {
//         var left = Hero.Hands.LeftHand;
//         var right = Hero.Hands.RightHand;
//
//         if (left == null && right == null)
//         {
//             return isDamage
//                 ? visitor.CalculateDefaultDamage(Hero.Stats)
//                 : visitor.CalculateDefaultDefence(Hero.Stats);
//         }
//
//         if (left == right)
//             return isDamage
//                 ? left.AcceptDamage(visitor, Hero.Stats)
//                 : left.AcceptDefense(visitor, Hero.Stats);
//
//         int total = 0;
//         if (left != null)
//             total += isDamage
//                 ? left.AcceptDamage(visitor, Hero.Stats)
//                 : left.AcceptDefense(visitor, Hero.Stats);
//         if (right != null)
//             total += isDamage
//                 ? right.AcceptDamage(visitor, Hero.Stats)
//                 : right.AcceptDefense(visitor, Hero.Stats);
//         
//         return total;
//     }
// }