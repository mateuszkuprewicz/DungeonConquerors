using ConsoleApp1.Dungeon_Themes;

namespace ConsoleApp1;

public class MapDirector
{
    private readonly MapBuilder _builder;
    private readonly IDungeonTheme _dungeonTheme;
    public MapDirector(MapBuilder builder, IDungeonTheme dungeonTheme)
    {
        this._builder = builder;
        this._dungeonTheme = dungeonTheme;
    }

    public void CreateDungeon()
    {
        switch (_dungeonTheme.DungeonType)
        {
            case DungeonTypes.BasicDungeon:
                BasicDungeon();
                break;
            case DungeonTypes.CorridorDungeon:
                CorridorDungeon();
                break;
            case  DungeonTypes.ChamberDungeon:
                ChamberDungeon();
                break;
        }

        for (int i = 0; i < _dungeonTheme.ItemCount; i++)
        {
            _builder.AddItem(_dungeonTheme.Items.ToArray());
        }
        _builder.AddItem(_dungeonTheme.Artifact);

        for (int i = 0; i < _dungeonTheme.EnemyCount; i++)
        {
            _builder.AddEnemy(_dungeonTheme.EnemyNames.ToArray());
        }

        Render.RenderAnnouncement(_dungeonTheme.Message);

    }

    private void BasicDungeon()
    {
        _builder.GenerateFullDungeon();
        for (int i = 0; i < 15; i++)
            _builder.AddCorridor();
            
        for(int i = 0; i < 5; i++)
            _builder.AddChamber();
        
        _builder.AddCentralRoom();
        
    }
    
    private void ChamberDungeon()
    {
        _builder.GenerateFullDungeon();
        for (int i = 0; i < 15; i++)
            _builder.AddCorridor();
        
        for(int i = 0; i < 20; i++)
            _builder.AddChamber();
        
        _builder.AddCentralRoom();
    }

    private void CorridorDungeon()
    {
        _builder.GenerateFullDungeon();
        for (int i = 0; i < 20; i++)
            _builder.AddCorridor();
        
        for(int i = 0; i < 5; i++)
            _builder.AddChamber();
    }
}