using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class GameMap
    {
        public const int MapHeight = 20;
        public const int MapWidth = 40;
        public Stack<Item>?[,] map;
        public int ExistingFiels;

        public GameMap()
        {
            map = new Stack<Item>?[MapHeight, MapWidth];
            ExistingFiels = 0;
        }
        public GameMap(string mapFilePath)
        {
            using StreamReader reader = new StreamReader(mapFilePath);
            //if cell is wall, Stack is null
            
            char c;
            for(int i = 0; i < MapHeight; i++)
            {
                for(int j = 0; j < MapWidth; j++)
                {
                    c = (char)reader.Read();
                    if (c == ' ')
                    {
                        map[i, j] = new Stack<Item>();
                    }
                }
                reader.ReadLine();
            }
        }
    }
}
