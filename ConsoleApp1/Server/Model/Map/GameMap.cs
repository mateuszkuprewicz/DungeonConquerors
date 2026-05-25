using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp1.Shared;

namespace ConsoleApp1
{
    public class GameMap
    {
        public Stack<Item>?[,] map;
        public Enemy?[,] enemies;
        public Hero?[,] heroes;
        public int ExistingFiels;

        public GameMap()
        {
            map = new Stack<Item>?[ModelConsts.MapHeight, ModelConsts.MapWidth];
            enemies = new Enemy?[ModelConsts.MapHeight, ModelConsts.MapWidth];
            heroes = new Hero?[ModelConsts.MapHeight, ModelConsts.MapWidth];
            ExistingFiels = 0;
        }
        
        public (int X, int Y) GetRandomFreePosition()
        {
            Random rnd = new Random();
            int x, y;
            do
            {
                x = rnd.Next(ModelConsts.MapWidth);
                y = rnd.Next(ModelConsts.MapHeight);
            } 
            while (map[y, x] != null || enemies[y, x] != null); 
    
            return (x, y);
        }
    }
}
