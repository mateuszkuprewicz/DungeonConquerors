using ConsoleApp1.SoundPropagation.SoundMediation;
using ConsoleApp1.Shared;
namespace ConsoleApp1;

public class MapBuilder
{
    private GameMap Map { get; set;}
    private readonly Random _rnd;
    private ISoundSubscribtion _soundSubscription;
    internal MapBuilder(GameMap map, ISoundSubscribtion soundSubscription)
    {
        Map = map;
        Map.ExistingFiels = 0;
        _rnd = new Random((int)DateTime.Now.Ticks);
        _soundSubscription = soundSubscription;
    }
    
    public void GenerateEmptyDungeon()
    {
        for (int i = 0; i < ModelConsts.MapHeight; i++)
            for (int j = 0; j < ModelConsts.MapWidth; j++)
            {
                Map.map[i, j] = new Stack<Item>();
            }
        Map.map[0, 0] = new Stack<Item>();
        Map.ExistingFiels = ModelConsts.MapHeight * ModelConsts.MapWidth;
    }

    public void GenerateFullDungeon()
    {
        for (int i = 0; i < ModelConsts.MapHeight; i++)
        for (int j = 0; j < ModelConsts.MapWidth; j++)
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
            position = _rnd.Next(ModelConsts.MapWidth);
            for (int i = 0; i < ModelConsts.MapHeight; i++)
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
            position = _rnd.Next(ModelConsts.MapHeight);
            for (int i = 0; i < ModelConsts.MapWidth; i++)
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
        x = _rnd.Next(ModelConsts.MapWidth - ChamberSize);
        y = _rnd.Next(ModelConsts.MapHeight - ChamberSize);
        
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
        y = (ModelConsts.MapHeight - CentralRoomSize)/2;
        x = (ModelConsts.MapWidth - CentralRoomSize)/2;
        
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

    public void AddItem(params Item[] items)
    {
        int item_index = _rnd.Next(items.Length);
        bool leave = false;
        
        int itemField = _rnd.Next(Map.ExistingFiels);
        int existingFieldsCount = 0;
        for(int i =  0; i < ModelConsts.MapHeight && !leave; i++)
        for (int j = 0; j < ModelConsts.MapWidth; j++)
        {
            if (Map.map[i, j] != null)
            {
                if (existingFieldsCount == itemField)
                {
                    Map.map[i,j].Push(items[item_index]);
                    leave = true;
                    break;
                }
                existingFieldsCount++;
            }
        }
    }

    private int _id = 0;
    public void AddEnemy(params string[] Enemies)
    {
        int enemy_name_index = _rnd.Next(Enemies.Length);
        int ExistingFieldsCount = 0;
        int EnemyLocalisation =  _rnd.Next(Map.ExistingFiels);
        for(int h = 0; h < ModelConsts.MapHeight; h++)
        for (int w = 0; w < ModelConsts.MapWidth; w++)
        {
            if (Map.map[h, w] != null)
            {
                if (ExistingFieldsCount == EnemyLocalisation)
                {
                    if(Map.enemies[h,w] != null) continue;
                    var enemy = new Enemy(75, 5, 2, Enemies[enemy_name_index], Map.enemies, (w, h), Map, _soundSubscription);
                    enemy.Id = _id++;
                    Map.enemies[h, w] = enemy;
                    ExistingFieldsCount++;
                }
                else
                {
                    ExistingFieldsCount++;
                }
            }
        }
    }
    
    // public void AddWeapons()
    // {
    //     if (Map.ExistingFiels == 0) return;
    //     int nrToWeaponType = _rnd.Next(3);
    //     int itemField = _rnd.Next(Map.ExistingFiels);
    //     int existingFieldsCount = 0;
    //
    //     for (int i = 0; i < GameMap.MapHeight; i++)
    //     for (int j = 0; j < GameMap.MapWidth; j++)
    //     {
    //         if (Map.map[i, j] != null)
    //         {
    //             if (existingFieldsCount == itemField)
    //             {
    //                 AbstractWeapon item = nrToWeaponType switch
    //                 {
    //                     0 => new OneHandedWeapon("Sword"),
    //                     1 => new HeavyWeapon("Big Sword"),
    //                     2 => new MagicalWeapon("Staff"),
    //                     _ => new OneHandedWeapon("Sword")
    //                 };
    //
    //                 item = ApplyRandomDecorators(item);
    //                 Map.map[i, j].Push(item);
    //                 return;
    //             }
    //             existingFieldsCount++;
    //         }
    //     }
    // }
    //
    // private AbstractWeapon ApplyRandomDecorators(AbstractWeapon weapon)
    // {
    //     int decoratorCount = _rnd.Next(3) + 1; // 1–3 dekoratorów
    //
    //     // 0=nieużyta, 1=boost, -1=weaken
    //     int[] used = new int[6];
    //
    //     for (int d = 0; d < decoratorCount; d++)
    //     {
    //         int stat = _rnd.Next(6);       
    //         int direction = _rnd.Next(2);  
    //         int dirValue = direction == 0 ? 1 : -1;
    //         
    //         if (used[stat] != 0)
    //             continue;
    //         
    //         used[stat] = dirValue;
    //
    //         weapon = (stat, direction) switch
    //         {
    //             (0, 0) => new StrengthBoostDecorator(weapon),
    //             (0, 1) => new StrengthWeakenDecorator(weapon),
    //             (1, 0) => new AgilityBoostDecorator(weapon),
    //             (1, 1) => new AgilityWeakenDecorator(weapon),
    //             (2, 0) => new LuckBoostDecorator(weapon),
    //             (2, 1) => new LuckWeakenDecorator(weapon),
    //             (3, 0) => new AggressivenessBoostDecorator(weapon),
    //             (3, 1) => new AggressivenessWeakenDecorator(weapon),
    //             (4, 0) => new WisdomBoostDecorator(weapon),
    //             (4, 1) => new WisdomWeakenDecorator(weapon),
    //             (5, 0) => new HealthBoostDecorator(weapon),
    //             (5, 1) => new HealthWeakenDecorator(weapon),
    //             _      => weapon
    //         };
    //     }
    //
    //     return weapon;
    // }
}