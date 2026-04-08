namespace ConsoleApp1;

public class MapBuilder
{
    private GameMap Map { get; set;}
    private readonly Random _rnd;
    internal MapBuilder(GameMap map)
    {
        Map = map;
        Map.ExistingFiels = 0;
        _rnd = new Random((int)DateTime.Now.Ticks);
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
        bool vertical = _rnd.Next(100) % 2 == 0;
        int position;
        if (vertical)
        {
            position = _rnd.Next(GameMap.MapWidth);
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
            position = _rnd.Next(GameMap.MapHeight);
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
        x = _rnd.Next(GameMap.MapWidth - ChamberSize);
        y = _rnd.Next(GameMap.MapHeight - ChamberSize);
        
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
        if (Map.ExistingFiels == 0) return;
        bool leave; 
        for (int ii = 0; ii < n; ii++)
        {
            leave = false;
            int itemField = _rnd.Next(Map.ExistingFiels);
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
        if (Map.ExistingFiels == 0) return;
        int nrToWeaponType = _rnd.Next(3);
        int itemField = _rnd.Next(Map.ExistingFiels);
        int existingFieldsCount = 0;

        for (int i = 0; i < GameMap.MapHeight; i++)
        for (int j = 0; j < GameMap.MapWidth; j++)
        {
            if (Map.map[i, j] != null)
            {
                if (existingFieldsCount == itemField)
                {
                    AbstractWeapon item = nrToWeaponType switch
                    {
                        0 => new OneHandedWeapon("Sword"),
                        1 => new HeavyWeapon("Big Sword"),
                        2 => new MagicalWeapon("Staff"),
                        _ => new OneHandedWeapon("Sword")
                    };

                    item = ApplyRandomDecorators(item);
                    Map.map[i, j].Push(item);
                    return;
                }
                existingFieldsCount++;
            }
        }
    }
    
    private AbstractWeapon ApplyRandomDecorators(AbstractWeapon weapon)
    {
        int decoratorCount = _rnd.Next(4); // 0–3 dekoratorów

        // Dla każdej statystyki śledzimy: 0=nieużyta, 1=boost, -1=weaken
        int[] used = new int[6];

        for (int d = 0; d < decoratorCount; d++)
        {
            int stat = _rnd.Next(6);       // losowa statystyka
            int direction = _rnd.Next(2);  // 0=boost, 1=weaken
            int dirValue = direction == 0 ? 1 : -1;
            
            if (used[stat] != 0 && used[stat] != dirValue)
                continue;
            
            if (used[stat] == dirValue)
                continue;

            used[stat] = dirValue;

            weapon = (stat, direction) switch
            {
                (0, 0) => new StrengthBoostDecorator(weapon),
                (0, 1) => new StrengthWeakenDecorator(weapon),
                (1, 0) => new AgilityBoostDecorator(weapon),
                (1, 1) => new AgilityWeakenDecorator(weapon),
                (2, 0) => new LuckBoostDecorator(weapon),
                (2, 1) => new LuckWeakenDecorator(weapon),
                (3, 0) => new AggressivenessBoostDecorator(weapon),
                (3, 1) => new AggressivenessWeakenDecorator(weapon),
                (4, 0) => new WisdomBoostDecorator(weapon),
                (4, 1) => new WisdomWeakenDecorator(weapon),
                (5, 0) => new HealthBoostDecorator(weapon),
                (5, 1) => new HealthWeakenDecorator(weapon),
                _      => weapon
            };
        }

        return weapon;
    }

    public void AddEnemies(int EnemiesNumber = 5)
    {
        if (Map.ExistingFiels == 0) return;
        for (int i = 0; i < EnemiesNumber; i++)
        {
            int ExistingFieldsCount = 0;
            int EnemyLocalisation =  _rnd.Next(Map.ExistingFiels);
            for(int h = 0; h < GameMap.MapHeight; h++)
            for (int w = 0; w < GameMap.MapWidth; w++)
            {
                if (ExistingFieldsCount == EnemyLocalisation)
                {
                    if(Map.enemies[h,w] != null) continue;
                    var enemy = new Enemy(75, 5, 5, "Goblin", 'G');
                    Map.enemies[h, w] = enemy;
                    ExistingFieldsCount++;
                }
                if (Map.map[h, w] != null) ExistingFieldsCount++;
            }
        }
    }
}