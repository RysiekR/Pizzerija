using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Oven : MonoBehaviour, IHasACost, IHasStaticList
{
    public static List<Oven> ovens = new List<Oven>();
    public static int OvenCost => ovens.Count * 100;
    int IHasACost.Cost => OvenCost;
    public OvenInput ovenInput { get; private set; }
    public OvenOutput ovenOutput { get; private set; }
    public int PizzasAmount { get; private set; } = 0;
    public OvenUpgrades Upgrades { get; private set; }
    public OvenBaking Baking { get; private set; }
    public Ingredients Ingredients { get; private set; }

    public OvenGoblinManager OvenGoblinManager { get; private set; }
    public OvenIngredientsManager OvenIngredientsManager { get; private set; }

    public Coroutine coroutine = null;

    public OvenVisuals OvenVisuals;


    private void Awake()
    {
        OvenVisuals = new(this);
        OvenGoblinManager = new(this);
        OvenIngredientsManager = new(this);
        ovenInput = GetComponentInChildren<OvenInput>();
        ovenInput.SetOven(this);
        ovenOutput = GetComponentInChildren<OvenOutput>();
        ovenOutput.SetOven(this);
        Upgrades = new OvenUpgrades(this, transform.Find("UpgradeHUD").GetComponent<Canvas>());
        Baking = new OvenBaking(this);
        Ingredients = new Ingredients();

        OvenVisuals.SetMatOFF(transform.Find("OvenVisualOff").GameObject());
        OvenVisuals.SetMatON(transform.Find("OvenVisualOn").GameObject());
        OvenVisuals.SetDisplayT(transform.Find("Canvas").Find("OvenDisplayT").GetComponent<TextMeshProUGUI>());
        OvenVisuals.SetUpgradeTrigger(transform.Find("UpgradeTrigger").GameObject());
        OvenVisuals.SetHuts(transform.Find("Houses_FirstAge_2_Level1").GameObject(),
            transform.Find("Houses_FirstAge_2_Level2").GameObject(),
            transform.Find("workhut3").GameObject());
    }
    void IHasStaticList.Initiate()
    {
        if (!ovens.Contains(this))
        {
            ovens.Add(this);
        }
        ResetTriggers();
        OvenVisuals.UpgradeBuildingVisual();

    }
    private void Start()
    {
        OvenVisuals.UpdateDisplay();
    }
    private void Update()
    {
        if (!Baking.IsBaking && Ingredients.HasAtLeastOneOfEach() && coroutine == null)
            coroutine = StartCoroutine(Baking.GetBake());

        OvenVisuals.UpdateVis();

        OvenGoblinManager.UpdateLogic();

        //Debug.Log(Ingredients.DoughAmount + ", " + Ingredients.SauceAmount + ", " + Ingredients.ToppingsAmount);
        //Debug.Log($"Pizzas ready: {PizzasAmount}");
    }

    private void OnDestroy()
    {
        ovens.Remove(this);
    }
    public void RemovePizzas(int i)
    {
        if (PizzasAmount > 0 && PizzasAmount >= i)
            PizzasAmount -= i;
    }
    public void AddPizzas(int amount)
    {
        if (amount > 0)
        {
            PizzasAmount += amount;
        }
    }
    public static void BuyOven()
    {
        if (Pizzeria.Instance.Money >= OvenCost)
        {
            Pizzeria.Instance.DeductMoney(OvenCost);
            Instantiate(Pizzeria.Instance.OvenPrefab);
            ovens[ovens.Count - 1].transform.position = new Vector3(25 + 7 * (ovens.Count - 1), 0, 12);
        }
    }
    public void ResetTriggers()
    {
        ovenInput = GetComponentInChildren<OvenInput>();
        ovenInput.SetOven(this);
        ovenOutput = GetComponentInChildren<OvenOutput>();
        ovenOutput.SetOven(this);
    }
    public void ResetOven()
    {
        ResetTriggers();
        OvenGoblinManager.ResetLists();
    }
    public static int SumOfReadyPizzas()
    {
        int sum = 0;
        foreach (Oven o in ovens)
        {
            sum += o.PizzasAmount;
        }
        return sum;
    }
    public static List<Oven> OvensThatNeedIngredients()
    {
        List<Oven> temp = new List<Oven>();
        foreach (Oven o in ovens)
        {
            if (o.OvenIngredientsManager.NeedIngredients())
            {
                temp.Add(o);
            }
        }
        return temp;
    }
    public static List<Oven> OvensWithPizzaReady()
    {
        List<Oven> temp = new List<Oven>();
        foreach (Oven o in ovens)
        {
            if (o.OvenIngredientsManager.HasPizzaToPickUp())
            {
                temp.Add(o);
            }
        }
        return temp;
    }

}

public class OvenGoblinManager
{
    Oven ownerOven;
    public List<GoblinTransporter> GoblinTransportersWithIngredients { get; private set; } = new List<GoblinTransporter>();
    public int AmountOfGoblinHelpers => ((int)ownerOven.Upgrades.Level / 2) + 1;
    public List<GoblinTransporter> GoblinForPizza { get; private set; } = new List<GoblinTransporter>();

    public OvenGoblinManager(Oven ownerOven)
    {
        this.ownerOven = ownerOven;
    }

    public void UpdateLogic()
    {
        if (GoblinTransportersWithIngredients.Count >= AmountOfGoblinHelpers)
        {
            if (GoblinTransportersWithIngredients.Any(g => g.ovenToHandle != ownerOven))
            {
                Debug.Log("NotRemovingCorrectly, adding too much to list");
                List<GoblinTransporter> toRemove = new List<GoblinTransporter>();
                foreach (GoblinTransporter t in GoblinTransportersWithIngredients)
                {
                    if (t.ovenToHandle != ownerOven)
                    {
                        toRemove.Add(t);
                    }
                }
                foreach (GoblinTransporter t in toRemove)
                {
                    GoblinTransportersWithIngredients.Remove(t);
                    t.State.Reset();
                }
            }
        }
        if (GoblinTransportersWithIngredients.Any(g => g.ovenToHandle != ownerOven))
        {
            Debug.Log("NotRemovingCorrectly");
            List<GoblinTransporter> toRemove = new List<GoblinTransporter>();
            foreach (GoblinTransporter t in GoblinTransportersWithIngredients)
            {
                if (t.ovenToHandle != ownerOven)
                {
                    toRemove.Add(t);
                }
            }
            foreach (GoblinTransporter t in toRemove)
            {
                GoblinTransportersWithIngredients.Remove(t);
                t.State.Reset();
            }

        }

    }
    public void SetIncomingGoblinWithIngredients(GoblinTransporter GoblinTransporter)
    {
        GoblinTransportersWithIngredients.Add(GoblinTransporter);
    }
    public void RemoveIncomingGoblinWithIngredientsFromList(GoblinTransporter goblinTransporter)
    {
        if (GoblinTransportersWithIngredients.Contains(goblinTransporter))
        {
            GoblinTransportersWithIngredients.Remove(goblinTransporter);
        }
        goblinTransporter.RemoveOvenToHandle(ownerOven);
    }
    public void AddIncomingGoblinForPizza(GoblinTransporter goblinTransporter)
    {
        GoblinForPizza.Add(goblinTransporter);
    }

    public void ResetLists()
    {
        GoblinForPizza.Clear();
        GoblinTransportersWithIngredients.Clear();
    }
}

public class OvenIngredientsManager
{
    Oven ownerOven;
    public OvenIngredientsManager(Oven ownerOven)
    {
        this.ownerOven = ownerOven;
    }
    /// <summary>
    /// returns true if no goblin is coming and need at least one ingredients and shelf has this ingredients
    /// </summary>
    public bool NeedIngredients()
    {
        if (ownerOven.OvenGoblinManager.GoblinTransportersWithIngredients.Count >= ownerOven.OvenGoblinManager.AmountOfGoblinHelpers)
            return false;
        if (Pizzeria.Instance.Ingredients.DoughAmount > 0 && NeedDough(0))
            return true;
        if (Pizzeria.Instance.Ingredients.SauceAmount > 0 && NeedSauce(0))
            return true;
        if (Pizzeria.Instance.Ingredients.ToppingsAmount > 0 && NeedToppings(0))
            return true;
        return false;
    }
    public bool NeedIngredients(GoblinTransporter goblinToCheck)
    {
        if (!ownerOven.OvenGoblinManager.GoblinTransportersWithIngredients.Contains(goblinToCheck))
            return false;
        if (Pizzeria.Instance.Ingredients.DoughAmount > 0 && NeedDough(0))
            return true;
        if (Pizzeria.Instance.Ingredients.SauceAmount > 0 && NeedSauce(0))
            return true;
        if (Pizzeria.Instance.Ingredients.ToppingsAmount > 0 && NeedToppings(0))
            return true;
        return false;
    }
    public bool PizzeriaHasIngredientsForThisOven()
    {
        if (Pizzeria.Instance.Ingredients.DoughAmount > 0 && NeedDough(0))
            return true;
        if (Pizzeria.Instance.Ingredients.SauceAmount > 0 && NeedSauce(0))
            return true;
        if (Pizzeria.Instance.Ingredients.ToppingsAmount > 0 && NeedToppings(0))
            return true;
        return false;

    }
    public bool HasPizzaToPickUp()
    {

        if (ownerOven.PizzasAmount > 0)
            if (ownerOven.PizzasAmount > ownerOven.OvenGoblinManager.GoblinForPizza.Sum(g => g.GoblinInventory.HowMuchFreeSpace()))
                return true;
        return false;
    }
    /// <summary>
    /// if with this amount of Dough oven will have enough for full batch
    /// </summary>
    public bool NeedDough(int incomingDough)
    {
        return ownerOven.Ingredients.DoughAmount + incomingDough + ownerOven.OvenGoblinManager.GoblinTransportersWithIngredients.Sum(g => g.GoblinInventory.DoughAmount) < ownerOven.Baking.BakeAmount;
    }
    /// <summary>
    /// if with this amount of Sauce oven will have enough for full batch
    /// </summary>
    public bool NeedSauce(int incomingSauce)
    {
        return ownerOven.Ingredients.SauceAmount + incomingSauce + ownerOven.OvenGoblinManager.GoblinTransportersWithIngredients.Sum(g => g.GoblinInventory.SauceAmount) < ownerOven.Baking.BakeAmount;
    }
    /// <summary>
    /// if with this amount of Toppings oven will have enough for full batch
    /// </summary>
    public bool NeedToppings(int incomingToppings)
    {
        return ownerOven.Ingredients.ToppingsAmount + incomingToppings + ownerOven.OvenGoblinManager.GoblinTransportersWithIngredients.Sum(g => g.GoblinInventory.ToppingsAmount) < ownerOven.Baking.BakeAmount;
    }

}

public class OvenVisuals
{
    Oven ownerOven;

    GameObject ovenVisMatOff;
    GameObject ovenVisMatOn;
    TextMeshProUGUI OvenDisplayT;
    GameObject UpgradeTrigger;

    GameObject Hut1;
    GameObject Hut2;
    GameObject Hut3;

    public void UpdateVis()
    {
        ovenVisMatOff.SetActive(!ownerOven.Baking.IsBaking);
        ovenVisMatOn.SetActive(ownerOven.Baking.IsBaking);
        UpgradeTrigger.SetActive(ownerOven.Upgrades.Upgrades.Count < ownerOven.Upgrades.Level - 1);
    }

    public void UpgradeBuildingVisual()
    {
        Hut1.SetActive(false);
        Hut2.SetActive(false);
        Hut3.SetActive(false);
        switch (ownerOven.Upgrades.Level)
        {
            case 0: 
            case 1: break;
            case 2:
            case 3: 
            case 4: Hut1.SetActive(true); break;
            case 5:
            case 6:
            case 7: Hut2.SetActive(true); break;
            default: Hut3.SetActive(true); break;
        }
    }

    public OvenVisuals(Oven ownerOven)
    {
        this.ownerOven = ownerOven;
    }
    public void UpdateDisplay()
    {
        OvenDisplayT.text = $"Ingredients: {ownerOven.Ingredients.DoughAmount}, {ownerOven.Ingredients.SauceAmount}, {ownerOven.Ingredients.ToppingsAmount}\n" +
            $"Oven: {OvenONOFF()}\n" +
            $"Pizzas ready: {ownerOven.PizzasAmount}\n" +
            $"Lv. {ownerOven.Upgrades.Level}  XP: {ownerOven.Upgrades.Exp}/{ownerOven.Upgrades.LevelThreshold()}";
    }
    string OvenONOFF()
    {
        if (ownerOven.Baking.IsBaking)
            return "ON";
        else
            return "OFF";
    }


    public void SetMatOFF(GameObject obj)
    {
        ovenVisMatOff = obj;
    }
    public void SetMatON(GameObject obj) { ovenVisMatOn = obj; }
    public void SetDisplayT(TextMeshProUGUI txt)
    {
        OvenDisplayT = txt;
    }
    public void SetUpgradeTrigger(GameObject obj)
    {
        UpgradeTrigger = obj;
    }

    public void SetHuts(GameObject obj1, GameObject obj2, GameObject obj3)
    {
        Hut1 = obj1;
        Hut2 = obj2;
        Hut3 = obj3;
    }
}