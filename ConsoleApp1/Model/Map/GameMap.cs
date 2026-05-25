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
        public int ExistingFiels;

        public GameMap()
        {
            map = new Stack<Item>?[ModelConsts.MapHeight, ModelConsts.MapWidth];
            enemies = new Enemy?[ModelConsts.MapHeight, ModelConsts.MapWidth];
            ExistingFiels = 0;
        }
    }
}
