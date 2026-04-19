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
        for (int i = 0; i < 5; i++)
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