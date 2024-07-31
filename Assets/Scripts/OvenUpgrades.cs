using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class OvenUpgrades
{
    Oven oven;
    public int Exp { get; private set; } = 0;
    public int Level { get; private set; } = 1;
    //IOvenUpgrade[] upgradesToChoose = new IOvenUpgrade[2];
    List<IOvenUpgrade> upgradesToChoose = new List<IOvenUpgrade>();
    public float TimeMultiplier { get; private set; } = 1f;
    public float AmountMultiplier { get; private set; } = 1f;
    public int AmountAdder { get; private set; } = 0;
    public List<IOvenUpgrade> Upgrades { get; private set; } = new List<IOvenUpgrade>();

    private Canvas UpgradeHUD;
    private TextMeshProUGUI up1;
    private TextMeshProUGUI up2;
    public OvenUpgrades(Oven oven, Canvas UpgradeHUD)
    {
        this.oven = oven;
        SetUpgradeHUDCanvas(UpgradeHUD);
    }
    public void SetUpgradeHUDCanvas(Canvas canvas)
    {
        UpgradeHUD = canvas;
        HideUpgradeHUD();
        GenerateUpgradeToChooseList();
        up1 = UpgradeHUD.transform.Find("Upgrade1I").transform.Find("Upgrade1T").GetComponent<TextMeshProUGUI>();
        up2 = UpgradeHUD.transform.Find("Upgrade2I").transform.Find("Upgrade2T").GetComponent<TextMeshProUGUI>();

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
    public int LevelThreshold()
    {
        return (int)Mathf.Pow(Level + 1, 2); //4, 9, 16, 25, 36, 49, 64, 81, 100
    }
    public void AddExp(int e)
    {
        if (e <= 0)
            return;
        Exp += e;
        if (Exp >= LevelThreshold())
        {
            Exp -= LevelThreshold();
            Level++;
        }
    }
    public int UpgradesToAdd()
    {
        return Level - Upgrades.Count - 1;
    }
    public void ShowUpgradeHUD()
    {
        UpgradeHUD.gameObject.SetActive(true);
    }
    public void HideUpgradeHUD()
    {
        UpgradeHUD.gameObject.SetActive(false);
    }
    public void ManageUpgradeHUD()
    {
        if (UpgradeHUD == null)
        { return; }

        if (UpgradesToAdd() > 0)
        {
            up1.text = $"Press 1:\n" +
                $"{upgradesToChoose[0].Description()}";
            up2.text = $"Press 2:\n" +
                $"{upgradesToChoose[1].Description()}";

            IOvenUpgrade chosenUpgrade = null;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                if (Pizzeria.Instance.CanPayWithMoney(upgradesToChoose[0].CostOfUpgrade()))
                chosenUpgrade = upgradesToChoose[0];
            if (Input.GetKeyDown(KeyCode.Alpha2))
                if (Pizzeria.Instance.CanPayWithMoney(upgradesToChoose[1].CostOfUpgrade()))
                    chosenUpgrade = upgradesToChoose[1];
            if(Input.GetKeyDown(KeyCode.Keypad5))
            {
                GenerateUpgradeToChooseList();
                return;
            }

            if (chosenUpgrade != null)
            {
                GenerateUpgradeToChooseList();
                AddUpgrade(chosenUpgrade);
                HideUpgradeHUD();
            }
        }
    }
    void GenerateUpgradeToChooseList()
    {
        upgradesToChoose.Clear();
        upgradesToChoose.Add(AvailbleOvenUpgrades.GetRandomUpgrade());
        upgradesToChoose.Add(AvailbleOvenUpgrades.GetRandomUpgrade());
        upgradesToChoose[0].SetOven(this);
        upgradesToChoose[1].SetOven(this);
    }
}
public interface IOvenUpgrade
{
    UpgradeCategory Type { get; }
    public void Upgrade();
    public void SetOven(OvenUpgrades oven);
    public int CostOfUpgrade();
    IOvenUpgrade Clone();
    public string Description();

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
    public static IOvenUpgrade GetRandomUpgrade()
    {
        UpgradeNames[] upgrades = (UpgradeNames[])Enum.GetValues(typeof(UpgradeNames));
        UpgradeNames random = upgrades[UnityEngine.Random.Range(0, upgrades.Length)];
        return GetOvenUpgrade(random);
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

    public string Description()
    {
        return $"shorten bake time\n" +
            $"(multiplicatively)\n" +
            $"by {fraction}\n" +
            $"cost: {CostOfUpgrade()}";
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
    public string Description()
    {
        return $"increase pizzas per bake\n" +
            $"(multiplicatively)\n" +
            $"by: {fractionAdded}\n" +
            $"cost: {CostOfUpgrade()}";
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
    public string Description()
    {
        return $"increase pizzas per bake\n" +
            $"(additively)\n" +
            $"by: {AmountAdded}\n" +
            $"cost: {CostOfUpgrade()}";
    }

    public IOvenUpgrade Clone()
    {
        return new BakeAmountAdderUpgrade(AmountAdded);
    }
}