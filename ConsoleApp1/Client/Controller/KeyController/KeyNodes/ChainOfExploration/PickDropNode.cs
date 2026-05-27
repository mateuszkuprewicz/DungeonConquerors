// using ConsoleApp1.Logger;
// using ConsoleApp1.View;
//
// namespace ConsoleApp1.ChainOfKeyOperations;
//
// public class PickDropNode : AbstractKeyNode
// {
//     private Render _render;
//     private GameMap _map;
//     public PickDropNode(Hero hero, GameMap map, Render render) : base(hero) => (_render, _map) = (render, map);
//     
//     public override void HandleKey(ConsoleKey keyInfo)
//     {
//         if (keyInfo == KeyConsts.PickItem.key)
//         {
//             (int result, Item? item) = Hero.Equipment.PickItem(Hero.Position, _map);
//             if(result == 1)
//             {
//                 _render.RenderInfo();
//                 _render.RenderMenu();
//                 
//                 EventLog el = EventLog.GetEventLog();
//                 el.Log(LogType.ItemPick, [item.Name]);
//             }
//             if (result == 0) Render.RenderAnnouncement("No items are lying here!");
//             if(result == -1)
//             {
//                 Render.RenderAnnouncement("Full inventory! Max number of items is 10.");
//             }
//         }
//         else if (keyInfo == KeyConsts.DropItem.key)
//         {
//             if(Hero.Equipment.DropItem(Hero.Position, _map))
//             {
//                 _render.RenderInfo();
//                 _render.RenderMenu();
//             }
//             else
//             {
//                 Render.RenderAnnouncement("You have empty equipment!");
//             }
//         }
//         else
//         { 
//             NextKeyNode.HandleKey(keyInfo);
//         }
//     }
// }