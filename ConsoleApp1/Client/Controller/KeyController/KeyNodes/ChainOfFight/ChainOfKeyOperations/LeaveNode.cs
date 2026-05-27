// using ConsoleApp1.View;
//
// namespace ConsoleApp1.ChainOfKeyOperations;
// using ConsoleApp1.Logger;
// using ConsoleApp1.Shared;
//
// public class LeaveNode : AbstractKeyNode
// {
//     private Render _render;
//     private Enemy _enemy;
//     private GameMap _map;
//
//     public LeaveNode(Hero hero, Enemy enemy, GameMap map, Render render)
//         : base(hero)
//     {
//         _render = render;
//         _enemy = enemy;
//         _map = map;
//     }
//
//     public override void HandleKey(ConsoleKey keyInfo)
//     {
//         if (keyInfo == KeyConsts.Leave.key)
//         {
//             Hero.Stats.Health -= _enemy.Damage;
//             _render.RenderStats();
//             if (Hero.Stats.Health <= 0)
//             {
//                 Render.RenderGameOver();
//                 
//                 EventLog el =  EventLog.GetEventLog();
//                 el.Log(LogType.DefeatedHero, [_enemy.Name]);
//                 Thread.Sleep(1000);
//                 Environment.Exit(0);
//             }
//             else
//             {
//                 if(Hero.Position.Y + 1 < ModelConsts.MapHeight && _map.map[Hero.Position.Y + 1, Hero.Position.X] != null && _map.enemies[Hero.Position.Y + 1, Hero.Position.X] == null)
//                 {
//                     Hero.Move(Direction.Down, _map);
//                     Render.RenderAnnouncement("You run from the fight");
//                     _render.ActualiseAfterHeroMove((Hero.Position.X, Hero.Position.Y - 1));
//                 }
//                 else if(Hero.Position.Y - 1 >= 0 && _map.map[Hero.Position.Y - 1, Hero.Position.X] != null && _map.enemies[Hero.Position.Y - 1, Hero.Position.X] == null)
//                 {
//                     Hero.Move(Direction.Up, _map);
//                     Render.RenderAnnouncement("You run from the fight");
//                     _render.ActualiseAfterHeroMove((Hero.Position.X, Hero.Position.Y + 1));
//                     
//                 }
//                 else if(Hero.Position.X + 1 < ModelConsts.MapWidth && _map.map[Hero.Position.Y, Hero.Position.X + 1] != null && _map.enemies[Hero.Position.Y, Hero.Position.X + 1] == null)
//                 {
//                     Hero.Move(Direction.Right, _map);
//                     Render.RenderAnnouncement("You run from the fight");
//                     _render.ActualiseAfterHeroMove((Hero.Position.X - 1, Hero.Position.Y));
//                 }
//                 else if(Hero.Position.X - 1 >= 0 && _map.map[Hero.Position.Y, Hero.Position.X - 1] != null && _map.enemies[Hero.Position.Y, Hero.Position.X - 1] == null)
//                 {
//                     Hero.Move(Direction.Left, _map);
//                     Render.RenderAnnouncement("You run from the fight");
//                     _render.ActualiseAfterHeroMove((Hero.Position.X + 1, Hero.Position.Y));
//                 }
//                 else
//                 {
//                     Render.RenderAnnouncement("There is nowhere to run!");
//                 }
//             }
//         }
//         else NextKeyNode.HandleKey(keyInfo);
//     }
// }