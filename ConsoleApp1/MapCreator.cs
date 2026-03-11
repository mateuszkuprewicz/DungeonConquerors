using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    internal static class MapCreator
    {
        public static void FillMap(GameMap map)
        {
            Item burbulator = new UselessItem("Burbulator");
            Item tenteges = new UselessItem("Tenteges");

            map.map[0, 2].Push(new Coin());
            map.map[0, 2].Push(new Coin());
            map.map[0, 2].Push(tenteges);
            map.map[0, 5].Push(tenteges);
            map.map[18, 38].Push(tenteges);
            map.map[0,2].Push(burbulator);
            map.map[0, 5].Push(burbulator);
            map.map[18, 38].Push(burbulator);

            map.map[12, 32].Push(burbulator);
            map.map[14, 30].Push(burbulator);
            map.map[10, 5].Push(burbulator);
            map.map[14, 30].Push(tenteges);
            map.map[12, 32].Push(tenteges);
            map.map[10, 5].Push(tenteges);

            map.map[0, 4].Push(new Gold());
            map.map[0, 2].Push(new Coin());
            map.map[18,34].Push(new Gold());
            map.map[18, 38].Push(new Gold());
            map.map[18, 38].Push(new Gold());

            map.map[0, 2].Push(new Weapon("Katana", WeaponType.TwoHanded));
            map.map[0, 5].Push(new Weapon("Dagger", WeaponType.OneHanded));
            map.map[0,4].Push(new Weapon("Shield", WeaponType.Shield));

        }
    }
}
