using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public static List<Oven> ovens = new List<Oven>();
    public static int OvenCost => ovens.Count * 100;
    public OvenInput ovenInput { get; private set; }
    public OvenOutput ovenOutput { get; private set; }
    public int PizzasAmount { get; private set; } = 0;
    public OvenUpgrades Upgrades { get; private set; }
    public OvenBaking Baking { get; private set; }
    public Ingredients Ingredients { get; private set; }
    //public GoblinTransporter GoblinWithIngredients { get; private set; } = null;
    public List<GoblinTransporter> GoblinTransportersWithIngredients { get; private set; } = new List<GoblinTransporter>();
    public int AmountOfGoblinHelpers => ((int)Upgrades.Level / 2) + 1;
    //public GoblinTransporter GoblinForPizza { get; private set; } = null;
    public List<GoblinTransporter> GoblinForPizza { get; private set; } = new List<GoblinTransporter>();
    public Coroutine coroutine = null;
    GameObject ovenVisMatOff;
    GameObject ovenVisMatOn;
    TextMeshProUGUI OvenDisplayT;
    GameObject UpgradeTrigger;
    private void Awake()
    {
        if (!ovens.Contains(this))
        {
            ovens.Add(this);
        }
        ovenInput = GetComponentInChildren<OvenInput>();
        ovenInput.SetOven(this);
        ovenOutput = GetComponentInChildren<OvenOutput>();
        ovenOutput.SetOven(this);
        Upgrades = new OvenUpgrades(this, transform.Find("UpgradeHUD").GetComponent<Canvas>());
        Baking = new OvenBaking(this);
        Ingredients = new Ingredients();
        ovenVisMatOn = transform.Find("OvenVisualOn").GameObject();
        ovenVisMatOff = transform.Find("OvenVisualOff").GameObject();
        OvenDisplayT = transform.Find("Canvas").Find("OvenDisplayT").GetComponent<TextMeshProUGUI>();
        UpgradeTrigger = transform.Find("UpgradeTrigger").GameObject();
    }
    private void Start()
    {
        UpdateDisplay();
    }
    private void Update()
    {
        if (!Baking.IsBaking && Ingredients.HasAtLeastOneOfEach() && coroutine == null)
            coroutine = StartCoroutine(Baking.GetBake());

        ovenVisMatOff.SetActive(!Baking.IsBaking);
        ovenVisMatOn.SetActive(Baking.IsBaking);
        UpgradeTrigger.SetActive(Upgrades.Upgrades.Count < Upgrades.Level - 1);

        if (GoblinTransportersWithIngredients.Count >= AmountOfGoblinHelpers)
        {
            if (GoblinTransportersWithIngredients.Any(g => g.ovenToHandle != this))
            {
                Debug.Log("NotRemovingCorrectly");
                List<GoblinTransporter> toRemove = new List<GoblinTransporter>();
                foreach (GoblinTransporter t in GoblinTransportersWithIngredients)
                {
                    if (t.ovenToHandle != this)
                    {
                        toRemove.Add(t);
                    }
                }
                foreach (GoblinTransporter t in toRemove)
                {
                    GoblinTransportersWithIngredients.Remove(t);
                }
            }
        }
        //Debug.Log(Ingredients.DoughAmount + ", " + Ingredients.SauceAmount + ", " + Ingredients.ToppingsAmount);
        //Debug.Log($"Pizzas ready: {PizzasAmount}");
    }

    private void OnDestroy()
    {
        ovens.Remove(this);
    }
    public void AddPizzas(int amount)
    {
        if (amount > 0)
        {
            PizzasAmount += amount;
        }
    }
    public void TriggerInput(Collider other)
    {
        if (other.GetComponent<GoblinTransporter>() != null)
        {
            if (other.GetComponent<GoblinTransporter>().ovenToHandle == this)
            {
                //goblinTransportersWithIngredients.Remove(other.GetComponent<GoblinTransporter>());
                RemoveIncomingGoblinWithIngredientsFromList(other.GetComponent<GoblinTransporter>());
                other.GetComponent<GoblinTransporter>().GoblinInventory.PassIngredients().TransferAllTo(Ingredients);
            }
        }
        UpdateDisplay();
    }
    public void TriggerOutput(Collider other)
    {
        if (other.GetComponent<GoblinTransporter>() != null)
        {
            if (other.GetComponent<GoblinTransporter>().TargetOvenOutput != null)
            {
                if (other.GetComponent<GoblinTransporter>().TargetOvenOutput == ovenOutput.transform)
                {
                    int i = Math.Min(other.GetComponent<GoblinTransporter>().GoblinInventory.HowMuchFreeSpace(), PizzasAmount);
                    other.GetComponent<GoblinTransporter>().GoblinInventory.PickUpPizzaUpToFullInv(PizzasAmount);
                    PizzasAmount -= i;
                    HUDScript.Instance.UpdatePizzasToSell();
                    //GoblinForPizza = null;
                    GoblinForPizza.Remove(other.GetComponent<GoblinTransporter>());
                }
            }
        }
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        OvenDisplayT.text = $"Ingredients: {Ingredients.DoughAmount}, {Ingredients.SauceAmount}, {Ingredients.ToppingsAmount}\n" +
            $"Oven: {OvenONOFF()}\n" +
            $"Pizzas ready: {PizzasAmount}\n" +
            $"Lv. {Upgrades.Level}  XP: {Upgrades.Exp}/{Upgrades.LevelThreshold()}";
    }
    string OvenONOFF()
    {
        if (Baking.IsBaking)
            return "ON";
        else
            return "OFF";
    }
    public void SetIncomingGoblinWithIngredients(GoblinTransporter GoblinTransporter)
    {
        //GoblinWithIngredients = GoblinTransporter;
        GoblinTransportersWithIngredients.Add(GoblinTransporter);
    }
    public void RemoveIncomingGoblinWithIngredientsFromList(GoblinTransporter goblinTransporter)
    {
        if (GoblinTransportersWithIngredients.Contains(goblinTransporter))
        {
            GoblinTransportersWithIngredients.Remove(goblinTransporter);
        }
        goblinTransporter.RemoveOvenToHandle(this);
    }
    public void AddIncomingGoblinForPizza(GoblinTransporter goblinTransporter)
    {
        GoblinForPizza.Add(goblinTransporter);
    }
    /// <summary>
    /// returns true if no goblin is coming and need at least one ingredients and shelf has this ingredients
    /// </summary>
    public bool NeedIngredients()
    {
        if (GoblinTransportersWithIngredients.Count >= AmountOfGoblinHelpers)
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
        if (!GoblinTransportersWithIngredients.Contains(goblinToCheck))
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
    public static List<Oven> OvensThatNeedIngredients()
    {
        List<Oven> temp = new List<Oven>();
        foreach (Oven o in ovens)
        {
            if (o.NeedIngredients())
            {
                temp.Add(o);
            }
        }
        return temp;
    }
    public bool HasPizzaToPickUp()
    {

        if (PizzasAmount > 0)
            if (PizzasAmount > GoblinForPizza.Sum(g => g.GoblinInventory.HowMuchFreeSpace()))
                return true;
        return false;
    }
    public static List<Oven> OvensWithPizzaReady()
    {
        List<Oven> temp = new List<Oven>();
        foreach (Oven o in ovens)
        {
            if (o.HasPizzaToPickUp())
            {
                temp.Add(o);
            }
        }
        return temp;
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
    /// <summary>
    /// if with this amount of Dough oven will have enough for full batch
    /// </summary>
    public bool NeedDough(int incomingDough)
    {
        return Ingredients.DoughAmount + incomingDough + GoblinTransportersWithIngredients.Sum(g => g.GoblinInventory.DoughAmount) < Baking.BakeAmount;
    }
    /// <summary>
    /// if with this amount of Sauce oven will have enough for full batch
    /// </summary>
    public bool NeedSauce(int incomingSauce)
    {
        return Ingredients.SauceAmount + incomingSauce + GoblinTransportersWithIngredients.Sum(g => g.GoblinInventory.SauceAmount) < Baking.BakeAmount;
    }
    /// <summary>
    /// if with this amount of Toppings oven will have enough for full batch
    /// </summary>
    public bool NeedToppings(int incomingToppings)
    {
        return Ingredients.ToppingsAmount + incomingToppings + GoblinTransportersWithIngredients.Sum(g => g.GoblinInventory.ToppingsAmount) < Baking.BakeAmount;
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
}