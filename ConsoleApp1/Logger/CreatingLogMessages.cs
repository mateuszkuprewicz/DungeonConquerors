namespace ConsoleApp1.Logger;

public enum LogType
{
    WallHit,
    ButtonHit,
    ItemPick,
    WeaponEquip,
    HeroHits,
    EnemyHits,
    DefeatedEnemy,
    DefeatedHero
}

public static class LogTexts
{
    public static string WallHit(string heroName)
    {
        return $"{heroName} hits wall";
    }

    public static string ButtonHit(string  heroName)
    {
        return $"{heroName} hits button";
    }

    public static string ItemPick(string heroName, string itemName)
    {
        return $"{heroName} picks {itemName}";
    }

    public static string WeaponEquip(string heroName, string weaponName)
    {
        return $"{heroName} equips {weaponName}";
    }

    public static string HeroHits(string heroName, string[] enemyNameAndDamage)
    {
        return $"{heroName} hits {enemyNameAndDamage[0]} with {enemyNameAndDamage[1]} damage";
    }

    public static string EnemyHits(string heroName, string[] enemyNameAndDamage)
    {
        return $"{enemyNameAndDamage[0]} hits {heroName} with {enemyNameAndDamage[1]} damage";
    }

    public static string DefeatedEnemy(string heroName, string enemyName)
    {
        return $"{enemyName} dies from the hand of {heroName}";
    }

    public static string DefeatedHero(string heroName, string enemyName)
    {
        return $"{heroName} dies from hand  of {enemyName}";
    }
}