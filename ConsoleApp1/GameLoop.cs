using System.Windows.Markup;

namespace ConsoleApp1
{
    internal class GameLoop
    {
        static void Main(string[] args)
        {
            //System.Threading.Thread.Sleep(5000);

            Hero myHero = new Hero();
            GameMap map = new GameMap("mapa.txt");
            MapCreator.FillMap(map);


            Render.RenderMap(myHero, map);
            Render.RenderMenu(myHero, map);

            ConsoleKeyInfo key;
            (int, int) myPosition = myHero.Position;
            while (true)
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.W:
                        if (myHero.Move(Direction.Up, map))
                        {
                            Render.ActualiseAfterHeroMove(myHero, myPosition, map);
                            Render.RenderInfo(map, myHero);
                        }
                        myPosition = myHero.Position;
                        break;
                    case ConsoleKey.A:
                        if (myHero.Move(Direction.Left, map))
                        {
                            Render.ActualiseAfterHeroMove(myHero, myPosition, map);
                            Render.RenderInfo(map, myHero);
                        }
                        myPosition = myHero.Position;
                        break;
                    case ConsoleKey.S:
                        if (myHero.Move(Direction.Down, map))
                        {
                            Render.ActualiseAfterHeroMove(myHero, myPosition, map);
                            Render.RenderInfo(map, myHero);
                        }
                        myPosition = myHero.Position;
                        break;
                    case ConsoleKey.D:
                        if (myHero.Move(Direction.Right, map))
                        {
                            Render.ActualiseAfterHeroMove(myHero, myPosition, map);
                            Render.RenderInfo(map, myHero);
                        }
                        myPosition = myHero.Position;
                        break;
                    case ConsoleKey.E:
                        int result = myHero.Equipment.PickItem(myPosition, map);
                        if(result == 1)
                        {
                            Render.RenderInfo(map, myHero);
                            Render.RenderMenu(myHero, map);
                        }
                        if (result == 0) break;
                        if(result == -1)
                        {
                            Render.RenderAnnouncement("Full inventory! Max number of items is 10.");
                        }
                        break;
                    case ConsoleKey.Q:
                        if(myHero.Equipment.DropItem(myPosition, map))
                        {
                            Render.RenderInfo(map, myHero);
                            Render.RenderMenu(myHero, map);
                        }
                        break;
                    case ConsoleKey.F:
                        if (myHero.Hands.EquipWeapon(myHero))
                        {
                            Render.RenderHeroHands(myHero);
                            Render.RenderEquipment(myHero);
                        }
                        break;
                    case ConsoleKey.R:
                        if (myHero.Hands.UnequipWeapon(myHero, map))
                        {
                            Render.RenderHeroHands(myHero);
                            Render.RenderEquipment(myHero);
                            Render.RenderInfo(map, myHero);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        Render.EquipmentScroll(myHero, ConsoleKey.DownArrow);
                        break;
                    case ConsoleKey.UpArrow:
                        Render.EquipmentScroll(myHero, ConsoleKey.UpArrow);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
