using System.Windows.Markup;
using ConsoleApp1.ChainOfKeyOperations;

namespace ConsoleApp1
{
    internal class GameLoop
    {
        static void Main(string[] args)
        {
            //System.Threading.Thread.Sleep(5000);

            Hero myHero = new Hero();
            GameMap map = new GameMap();
            MapBuilder builder = new MapBuilder(map);
            MapDirector mapDirector = new MapDirector(builder);
            mapDirector.BasicDungeon();
            //builder.AddUsellesItems();
            InstructionBuilder instructionBuilder = new InstructionBuilder(map, myHero);
            
            Render.RenderMap(myHero, map);
            Render.RenderMenu(myHero, map);

            ConsoleKeyInfo key;
            KeyNode move = new MoveNode(myHero, map);
            KeyNode pick = new PickDropNode(myHero, map);
            KeyNode weaponEquip = new WeaponEquipmentNode(myHero, map);
            KeyNode  scroll = new EquipmentScrollNode(myHero);
            KeyNode sentinel = new Sentinel();
            move.SetNextHandler(pick);
            pick.SetNextHandler(weaponEquip);
            weaponEquip.SetNextHandler(scroll);
            scroll.SetNextHandler(sentinel);
            while (true)
            {
                instructionBuilder.PrintInstruction();
                key = Console.ReadKey(true);
                move.HandleKey(key.Key);
            }
        }
    }
}
