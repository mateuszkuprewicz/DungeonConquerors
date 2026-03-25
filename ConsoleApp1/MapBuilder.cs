namespace ConsoleApp1;

public class MapBuilder
{
    private GameMap Map { get; set;}
    private Random rnd;
    internal MapBuilder(GameMap map)
    {
        Map = map;
        Map.ExistingFiels = 0;
        rnd = new Random((int)DateTime.Now.Ticks);
    }
    
    public void GenerateEmptyDungeon()
    {
        for (int i = 0; i < GameMap.MapHeight; i++)
            for (int j = 0; j < GameMap.MapWidth; j++)
            {
                Map.map[i, j] = new Stack<Item>();
            }
        Map.map[0, 0] = new Stack<Item>();
        Map.ExistingFiels = GameMap.MapHeight * GameMap.MapWidth;
    }

    public void GenerateFullDungeon()
    {
        for (int i = 0; i < GameMap.MapHeight; i++)
        for (int j = 0; j < GameMap.MapWidth; j++)
        {
            Map.map[i, j] = null;
        }
        Map.map[0, 0] = new Stack<Item>();
        Map.ExistingFiels = 1;
    }

    public void AddCorridor()
    {
        bool vertical = rnd.Next(100) % 2 == 0;
        int position;
        if (vertical)
        {
            position = rnd.Next(GameMap.MapWidth);
            for (int i = 0; i < GameMap.MapHeight; i++)
            {
                if (Map.map[i, position] == null)
                {
                    Map.map[i, position] = new Stack<Item>();
                    Map.ExistingFiels++;
                }
            }
        }
        else
        {
            position = rnd.Next(GameMap.MapHeight);
            for (int i = 0; i < GameMap.MapWidth; i++)
            {
                if (Map.map[position, i] == null)
                {
                    Map.map[position, i] = new Stack<Item>();
                    Map.ExistingFiels++;
                }
            }
        }
        
    }

    private const int ChamberSize = 3;
    public void AddChamber()
    {
        int x, y;
        x = rnd.Next(GameMap.MapWidth - ChamberSize);
        y = rnd.Next(GameMap.MapHeight - ChamberSize);
        
        for(int i = 0; i < ChamberSize; i++)
        for (int j = 0; j < ChamberSize; j++)
        {
            if (Map.map[y + i, x + j] == null)
            {
                Map.map[y + i, x + j] =  new Stack<Item>();
                Map.ExistingFiels++;
            }
        }
    }
    
    private const int CentralRoomSize = 6;
    public void AddCentralRoom()
    {
        int x, y;
        y = (GameMap.MapHeight - CentralRoomSize)/2;
        x = (GameMap.MapWidth - CentralRoomSize)/2;
        
        for(int i = 0; i < CentralRoomSize; i++)
        for (int j = 0; j < CentralRoomSize; j++)
        {
            if (Map.map[y + i, x + j] == null)
            {
                Map.map[y + i, x + j] = new Stack<Item>();
                Map.ExistingFiels++;
            }
        }
    }

    public void AddUsellesItems(int n)
    {
        bool leave; ;
        for (int ii = 0; ii < n; ii++)
        {
            leave = false;
            int itemField = rnd.Next(Map.ExistingFiels);
            int existingFieldsCount = 0;
            for(int i =  0; i < GameMap.MapHeight && !leave; i++)
            for (int j = 0; j < GameMap.MapWidth; j++)
            {
                if (Map.map[i, j] != null)
                {
                    if (existingFieldsCount == itemField)
                    {
                        Map.map[i,j].Push(new UselessItem("Useless Item"));
                        leave = true;
                        break;
                    }
                    existingFieldsCount++;
                }
            }
        }
        
    }

    public void AddWeapons()
    {
        int nrToWeaponType = rnd.Next(Enum.GetValues(typeof(WeaponType)).Length);
        WeaponType weaponType = (WeaponType)nrToWeaponType;
        int itemField = rnd.Next(Map.ExistingFiels);
        int existingFieldsCount = 0;
        for(int i =  0; i < GameMap.MapHeight; i++)
        for (int j = 0; j < GameMap.MapWidth; j++)
        {
            if (Map.map[i, j] != null)
            {
                if (existingFieldsCount == itemField)
                {
                    Weapon item;
                    switch (weaponType)
                    {
                        case WeaponType.OneHanded:
                            item = new Weapon("Sword", weaponType);
                            Map.map[i, j].Push(item);
                            break;
                        case WeaponType.TwoHanded:
                            item = new Weapon("Big Sword", weaponType);
                            Map.map[i, j].Push(item);
                            break;
                        case WeaponType.Shield:
                            item = new Weapon("Wooden Shield", weaponType);
                            Map.map[i, j].Push(item);
                            break;
                    }
                    return;
                }
                existingFieldsCount++;
            }
        }
    }
}