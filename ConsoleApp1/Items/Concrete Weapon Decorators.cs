namespace ConsoleApp1;

public class StrengthBoostDecorator : AbstractWeaponDecorator
{
    public StrengthBoostDecorator(AbstractWeapon inner, int bonus = 5)
        : base(inner, bonus, "Silny") { }

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
    public StrengthWeakenDecorator(AbstractWeapon inner, int penalty = 5)
        : base(inner, penalty, "Słaby") { }

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
    public AgilityBoostDecorator(AbstractWeapon inner, int bonus = 5)
        : base(inner, bonus, "Zwinny") { }

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
    public AgilityWeakenDecorator(AbstractWeapon inner, int penalty = 5)
        : base(inner, penalty, "Ociężały") { }

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
    public LuckBoostDecorator(AbstractWeapon inner, int bonus = 5)
        : base(inner, bonus, "Szczęśliwy") { }

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
    public LuckWeakenDecorator(AbstractWeapon inner, int penalty = 5)
        : base(inner, penalty, "Pechowy") { }

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
    public AggressivenessBoostDecorator(AbstractWeapon inner, int bonus = 5)
        : base(inner, bonus, "Agresywny") { }

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
    public AggressivenessWeakenDecorator(AbstractWeapon inner, int penalty = 5)
        : base(inner, penalty, "Spokojny") { }

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
    public WisdomBoostDecorator(AbstractWeapon inner, int bonus = 5)
        : base(inner, bonus, "Mądry") { }

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
    public WisdomWeakenDecorator(AbstractWeapon inner, int penalty = 5)
        : base(inner, penalty, "Głupi") { }

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
    public HealthBoostDecorator(AbstractWeapon inner, int bonus = 10)
        : base(inner, bonus, "Ochronny") { }

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
    public HealthWeakenDecorator(AbstractWeapon inner, int penalty = 10)
        : base(inner, penalty, "Przeklęty") { }

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