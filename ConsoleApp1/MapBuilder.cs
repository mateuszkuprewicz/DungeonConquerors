namespace ConsoleApp1;

public class Builder
{
    private GameMap Map { get; set;}
    private int _emptyFields;
    internal Builder(GameMap map)
    {
        Map = map;
        _emptyFields = 0;
    }
    
    public void GenerateEmptyDungeon()
    {
        for (int i = 0; i < GameMap.MapHeight; i++)
            for (int j = 0; j < GameMap.MapWidth; j++)
            {
                Map.map[i, j] = new Stack<Item>();
            }
        _emptyFields = GameMap.MapHeight * GameMap.MapWidth;
    }

    public void GenerateFullDungeon()
    {
        for (int i = 0; i < GameMap.MapHeight; i++)
        for (int j = 0; j < GameMap.MapWidth; j++)
        {
            Map.map[i, j] = null;
        }

        _emptyFields = 0;
    }

    public void AddCorridor()
    {
        var rnd = new Random(DateTime.Now.Millisecond);
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
                    _emptyFields++;
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
                    _emptyFields++;
                }
            }
        }
        
    }

    private const int ChamberSize = 3;
    public void AddChamber()
    {
        var rnd = new Random(DateTime.Now.Millisecond);
        int x, y;
        x = rnd.Next(GameMap.MapWidth - ChamberSize);
        y = rnd.Next(GameMap.MapHeight - ChamberSize);
        
        for(int i = 0; i < ChamberSize; i++)
        for (int j = 0; j < ChamberSize; j++)
        {
            if (Map.map[y + i, x + j] == null)
            {
                Map.map[y + i, x + j] =  new Stack<Item>();
                _emptyFields++;
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
            if (Map.map[y + i, x + i] == null)
            {
                Map.map[y + i, x + i] = new Stack<Item>();
                _emptyFields++;
            }
        }
    }

    public void AddUsellesItems()
    {
        var rnd = new Random(DateTime.Now.Millisecond);
        int itemField = rnd.Next(_emptyFields);
        int existingFieldsCount = 0;
        for(int i =  0; i < GameMap.MapHeight; i++)
        for (int j = 0; j < GameMap.MapWidth; j++)
        {
            if (Map.map[i, j] != null)
            {
                if (existingFieldsCount == itemField)
                {
                    Map.map[i,j].Push(new UselessItem("Useless Item"));
                    return;
                }
                existingFieldsCount++;
            }
        }
    }

    public void AddWeapons()
    {
        var rnd = new Random(DateTime.Now.Millisecond);
        int nrToWeaponType =  rnd.Next(Weapon.WeaponTypeCount);
        WeaponType weaponType = (WeaponType)nrToWeaponType;
        int itemField = rnd.Next(_emptyFields);
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
                            break;
                        case WeaponType.Shield:
                            item = new Weapon("shield", weaponType);
                            break;
                    }
                    return;
                }
                existingFieldsCount++;
            }
        }
    }
}