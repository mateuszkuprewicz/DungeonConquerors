// using ConsoleApp1.Logger;
// using ConsoleApp1.View;
//
// namespace ConsoleApp1.ChainOfKeyOperations;
//
// public class WeaponEquipmentNode : AbstractKeyNode
// {
//     private Render _render;
//     private GameMap _map;
//     public WeaponEquipmentNode(Hero hero, GameMap map, Render render) : base(hero) => (_render, _map)  = (render, map);
//     
//     public override void HandleKey(ConsoleKey keyInfo)
//     {
//         if (keyInfo == KeyConsts.EquipWeapon.key)
//         {
//             (bool succes, Item? item) = Hero.Hands.EquipWeapon(Hero);
//             if (succes)
//             {
//                 _render.RenderHeroHands();
//                 _render.RenderEquipment();
//                 _render.RenderStats();
//
//                 EventLog el = EventLog.GetEventLog();
//                 el.Log(LogType.WeaponEquip, [item.Name]);
//             }
//             else
//             {
//                 Render.RenderAnnouncement("Cannot wear this now!");
//             }
//         }
//         else if (keyInfo == KeyConsts.UnequipWeapon.key)
//         {
//             if (Hero.Hands.UnequipWeapon(Hero, _map))
//             {
//                 _render.RenderHeroHands();
//                 _render.RenderEquipment();
//                 _render.RenderInfo();
//                 _render.RenderStats();
//             }
//             else
//             {
//                 Render.RenderAnnouncement("You dont wear onything on yourself!");
//             }
//         }
//         else
//         {
//             NextKeyNode.HandleKey(keyInfo);
//         }
//     }
// }