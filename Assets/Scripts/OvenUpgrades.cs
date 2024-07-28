using System.Collections.Generic;


public class OvenUpgrades
{
    Oven oven;
    public float TimeMultiplier { get; private set; } = 1f;
    public float AmountMultiplier { get; private set; } = 1f;
    public int AmountAdder { get; private set; } = 0;
    public List<IOvenUpgrade> Upgrades { get; private set; } = new List<IOvenUpgrade>();
    public OvenUpgrades(Oven oven)
    {
        this.oven = oven;
    }
    public void BuyUpgrade(IOvenUpgrade upgrade)
    {
        if (upgrade.CostOfUpgrade() <= Pizzeria.Instance.Money)
        {
            Pizzeria.Instance.DeductMoney(upgrade.CostOfUpgrade());
            AddUpgrade(upgrade);
        }
    }
    private void AddUpgrade(IOvenUpgrade upgrade)
    {
        Upgrades.Add(upgrade);
        upgrade.SetOven(this);
        upgrade.Upgrade();
    }
    public void MultiplyTime(float amount)
    {
        TimeMultiplier *= amount;
    }
    public void MultiplyAmount(float amount)
    {
        AmountMultiplier *= amount;
    }
    public void AddAmount(int amount)
    {
        AmountAdder += amount;
    }
    public int AmountOfUpgrades(UpgradeCategory type)
    {
        int i = 0;
        foreach (IOvenUpgrade upgrade in Upgrades)
        {
            if (upgrade.Type == type)
                i++;
        }
        return i;
    }
}

public interface IOvenUpgrade
{
    UpgradeCategory Type { get; }
    public void Upgrade();
    public void SetOven(OvenUpgrades oven);
    public int CostOfUpgrade();
    IOvenUpgrade Clone();

}
public enum UpgradeCategory
{
    TimeMulti,
    AmountAdder,
    AmountMultiplayer
}
public static class AvailbleOvenUpgrades
{
    public static Dictionary<UpgradeNames, IOvenUpgrade> OvenUpsDictionary = new Dictionary<UpgradeNames, IOvenUpgrade>()
    {
        { UpgradeNames.FastOne,new BakeTimeUpgrade(0.1f) },
        { UpgradeNames.FastTwo,new BakeTimeUpgrade(0.2f) },
        { UpgradeNames.StackerOne, new BakeAmountAdderUpgrade(1) },
        { UpgradeNames.StackerTwo, new BakeAmountAdderUpgrade(2) },
        { UpgradeNames.MultiOne, new BakeAmountMultiUpgrade(0.1f) },
        { UpgradeNames.MultiTwo, new BakeAmountMultiUpgrade(0.2f) }
    };

    public static IOvenUpgrade GetOvenUpgrade(UpgradeNames name)
    {
        return OvenUpsDictionary[name].Clone();
    }

    public enum UpgradeNames
    {
        FastOne,
        FastTwo,
        StackerOne,
        StackerTwo,
        MultiOne,
        MultiTwo,
    }
}


public class BakeTimeUpgrade : IOvenUpgrade
{
    OvenUpgrades ovenUpgrades;
    public float fraction { get; }
    readonly UpgradeCategory type = UpgradeCategory.TimeMulti;

    UpgradeCategory IOvenUpgrade.Type => type;

    public BakeTimeUpgrade(OvenUpgrades ovenUpgrades, float fraction)
    {
        this.ovenUpgrades = ovenUpgrades;
        this.fraction = fraction;
    }
    public BakeTimeUpgrade(float fraction)
    {
        this.fraction = fraction;
    }
    public void SetOven(OvenUpgrades ovenUpgrades)
    {
        this.ovenUpgrades = ovenUpgrades;
    }
    public void Upgrade()
    {
        ovenUpgrades.MultiplyTime(1 - fraction);
    }
    public int CostOfUpgrade()
    {
        return ovenUpgrades.AmountOfUpgrades(type) * (int)(10 * fraction);
    }
    public IOvenUpgrade Clone()
    {
        return new BakeTimeUpgrade(fraction);
    }
}

public class BakeAmountMultiUpgrade : IOvenUpgrade
{
    OvenUpgrades ovenUpgrades;
    public float fractionAdded { get; }
    readonly UpgradeCategory type = UpgradeCategory.AmountMultiplayer;

    UpgradeCategory IOvenUpgrade.Type => type;

    public BakeAmountMultiUpgrade(OvenUpgrades ovenUpgrades, float fraction)
    {
        this.ovenUpgrades = ovenUpgrades;
        fractionAdded = fraction;
    }
    public BakeAmountMultiUpgrade(float fraction)
    {
        fractionAdded = fraction;
    }
    public void SetOven(OvenUpgrades ovenUpgrades)
    {
        this.ovenUpgrades = ovenUpgrades;
    }
    public void Upgrade()
    {
        ovenUpgrades.MultiplyAmount(fractionAdded + 1);
    }
    public int CostOfUpgrade()
    {
        return ovenUpgrades.AmountOfUpgrades(type) * (int)(10 * fractionAdded);
    }
    public IOvenUpgrade Clone()
    {
        return new BakeAmountMultiUpgrade(fractionAdded);
    }
}
public class BakeAmountAdderUpgrade : IOvenUpgrade
{
    OvenUpgrades ovenUpgrades;
    public int AmountAdded { get; }
    readonly UpgradeCategory type = UpgradeCategory.AmountAdder;

    UpgradeCategory IOvenUpgrade.Type => type;

    public BakeAmountAdderUpgrade(OvenUpgrades ovenUpgrades, int amount)
    {
        this.ovenUpgrades = ovenUpgrades;
        AmountAdded = amount;
    }
    public BakeAmountAdderUpgrade(int amount)
    {
        AmountAdded = amount;
    }
    public void SetOven(OvenUpgrades ovenUpgrades)
    {
        this.ovenUpgrades = ovenUpgrades;
    }
    public void Upgrade()
    {
        ovenUpgrades.AddAmount(AmountAdded);
    }
    public int CostOfUpgrade()
    {
        return ovenUpgrades.AmountOfUpgrades(type) * (int)(10 * AmountAdded);
    }
    public IOvenUpgrade Clone()
    {
        return new BakeAmountAdderUpgrade(AmountAdded);
    }
}