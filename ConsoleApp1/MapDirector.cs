namespace ConsoleApp1;

public class MapDirector
{
    private MapBuilder builder;
    public MapDirector(MapBuilder builder)
    {
        this.builder = builder;
    }

    public void BasicDungeon()
    {
        builder.GenerateFullDungeon();
        for (int i = 0; i < 10; i++)
            builder.AddCorridor();
            
        for(int i = 0; i < 3; i++)
            builder.AddChamber();
        
        builder.AddCentralRoom();
        
        builder.AddUsellesItems(10);

        for(int i = 0; i < 10; i++)
            builder.AddWeapons();
    }
    
    public void BasicDungeonWithNoItems()
    {
        builder.GenerateFullDungeon();
        for (int i = 0; i < 10; i++)
            builder.AddCorridor();
            
        for(int i = 0; i < 3; i++)
            builder.AddChamber();
        
        builder.AddCentralRoom();
    }
    
    public void BasicDungeonWithNoWeapons()
    {
        builder.GenerateFullDungeon();
        for (int i = 0; i < 10; i++)
            builder.AddCorridor();
            
        for(int i = 0; i < 3; i++)
            builder.AddChamber();
        
        builder.AddCentralRoom();
        
        builder.AddUsellesItems(10);
    }

    public void ChamberDungeon()
    {
        builder.GenerateFullDungeon();
        for (int i = 0; i < 5; i++)
            builder.AddCorridor();
        
        for(int i = 0; i < 20; i++)
            builder.AddChamber();
        
        builder.AddCentralRoom();
        
        builder.AddUsellesItems(10);

        for(int i = 0; i < 10; i++)
            builder.AddWeapons();
    }

    public void CorridorDungeon()
    {
        builder.GenerateFullDungeon();
        for (int i = 0; i < 20; i++)
            builder.AddCorridor();
        
        for(int i = 0; i < 5; i++)
            builder.AddChamber();
        
        builder.AddUsellesItems(10);

        for(int i = 0; i < 10; i++)
            builder.AddWeapons();
    }
    
    
}