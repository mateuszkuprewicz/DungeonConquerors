namespace ConsoleApp1;

public class StrengthBoostDecorator : AbstractWeaponDecorator
{
    public StrengthBoostDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Silny") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Strength += ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Strength -= ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}

public class StrengthWeakenDecorator : AbstractWeaponDecorator
{
    public StrengthWeakenDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Słaby") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Strength -= ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Strength += ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}

public class AgilityBoostDecorator : AbstractWeaponDecorator
{
    public AgilityBoostDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Zręczny") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Agility += ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Agility -= ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}

public class AgilityWeakenDecorator : AbstractWeaponDecorator
{
    public AgilityWeakenDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Śliski") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Agility -= ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Agility += ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}


public class LuckBoostDecorator : AbstractWeaponDecorator
{
    public LuckBoostDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Szczęśliwy") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Luck += ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Luck -= ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}

public class LuckWeakenDecorator : AbstractWeaponDecorator
{
    public LuckWeakenDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Pechowy") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Luck -= ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Luck += ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}


public class AggressivenessBoostDecorator : AbstractWeaponDecorator
{
    public AggressivenessBoostDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Agresywny") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Agressiveness += ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Agressiveness -= ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}

public class AggressivenessWeakenDecorator : AbstractWeaponDecorator
{
    public AggressivenessWeakenDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"tchórzliwy") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Agressiveness -= ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Agressiveness += ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}


public class WisdomBoostDecorator : AbstractWeaponDecorator
{
    public WisdomBoostDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Mądry") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Wisdom += ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Wisdom -= ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}

public class WisdomWeakenDecorator : AbstractWeaponDecorator
{
    public WisdomWeakenDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Głupi") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Wisdom -= ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Wisdom += ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}


public class HealthBoostDecorator : AbstractWeaponDecorator
{
    public HealthBoostDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Mocny") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Health += ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Health -= ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}

public class HealthWeakenDecorator : AbstractWeaponDecorator
{
    public HealthWeakenDecorator(AbstractWeapon inner, int bonusModifier = 5, int bonusDamage = 1)
        : base(inner, bonusModifier, bonusDamage,"Słaby") { }

    public override void ApplyModifier(Hero hero)
    {
        hero.Stats.Health -= ModifierValue;
    }

    public override void RemoveModifier(Hero hero)
    {
        hero.Stats.Health += ModifierValue;
        InnerWeapon.RemoveModifier(hero);
    }
}