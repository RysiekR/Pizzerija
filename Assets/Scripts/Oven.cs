using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public static List<Oven> ovens = new List<Oven>();
    public static int OvenCost => ovens.Count * 10;
    public OvenInput ovenInput { get; private set; }
    public OvenOutput ovenOutput { get; private set; }
    public int PizzasAmount { get; private set; } = 0;
    public OvenUpgrades Upgrades { get; private set; }
    public OvenBaking Baking { get; private set; }
    public Ingredients Ingredients { get; private set; }
    public GoblinTransporter GoblinWithIngredients { get; private set; } = null;
    public GoblinTransporter GoblinForPizza { get; private set; } = null;
    public Coroutine coroutine = null;
    GameObject ovenVisMatOff;
    GameObject ovenVisMatOn;
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
        Upgrades = new OvenUpgrades(this);
        Baking = new OvenBaking(this);
        Ingredients = new Ingredients();
        ovenVisMatOn = transform.Find("OvenVisualOn").GameObject();
        ovenVisMatOff = transform.Find("OvenVisualOff").GameObject();
    }
    private void Update()
    {
        if (!Baking.IsBaking && Ingredients.HasAtLeastOneOfEach() && coroutine == null)
            coroutine = StartCoroutine(Baking.GetBake());

        ovenVisMatOff.SetActive(!Baking.IsBaking);
        ovenVisMatOn.SetActive(Baking.IsBaking);

        if (GoblinWithIngredients != null)
        {
            if (GoblinWithIngredients.ovenToHandle == null)
            {
                GoblinWithIngredients = null;
            }
            else if (GoblinWithIngredients.ovenToHandle != this)
            {
                GoblinWithIngredients = null;
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
                other.GetComponent<GoblinTransporter>().GoblinInventory.PassIngredients().TransferAllTo(Ingredients);
                GoblinWithIngredients.TargetOvenInput = null;
                GoblinWithIngredients.ovenToHandle = null;
                GoblinWithIngredients = null;
            }
        }

    }
    public void TriggerOutput(Collider other)
    {
        if (other.GetComponent<GoblinTransporter>().TargetOvenOutput != null)
        {
            if (other.GetComponent<GoblinTransporter>().TargetOvenOutput == ovenOutput.transform)
            {
                int i = Math.Min(other.GetComponent<GoblinTransporter>().GoblinInventory.HowMuchFreeSpace(), PizzasAmount);
                other.GetComponent<GoblinTransporter>().GoblinInventory.PickUpPizzaUpToFullInv(PizzasAmount);
                PizzasAmount -= i;
                HUDScript.Instance.UpdatePizzasToSell();
                GoblinForPizza = null;
            }
        }
    }
    public void SetIncomingGoblinWithIngredients(GoblinTransporter GoblinTransporter)
    {
        GoblinWithIngredients = GoblinTransporter;
    }
    public void SetIncomingGoblinForPizza(GoblinTransporter goblinTransporter)
    {
        GoblinForPizza = goblinTransporter;
    }
    /// <summary>
    /// returns true if no goblin is coming and need at least one ingredients and shelf has this ingredients
    /// </summary>
    public bool NeedIngredients()
    {
        if (GoblinWithIngredients != null)
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
        if (GoblinWithIngredients != goblinToCheck)
            return false;
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
        if (GoblinForPizza != null)
            return false;
        if (PizzasAmount > 0)
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
        return Ingredients.DoughAmount + incomingDough < Baking.BakeAmount;
    }
    /// <summary>
    /// if with this amount of Sauce oven will have enough for full batch
    /// </summary>
    public bool NeedSauce(int incomingSauce)
    {
        return Ingredients.SauceAmount + incomingSauce < Baking.BakeAmount;
    }
    /// <summary>
    /// if with this amount of Toppings oven will have enough for full batch
    /// </summary>
    public bool NeedToppings(int incomingToppings)
    {
        return Ingredients.ToppingsAmount + incomingToppings < Baking.BakeAmount;
    }
    public static void BuyOven()
    {
        if(Pizzeria.Instance.Money >= OvenCost)
        {
            Pizzeria.Instance.DeductMoney(OvenCost);
            Instantiate(Pizzeria.Instance.OvenPrefab);
            ovens[ovens.Count - 1].transform.position = new Vector3(25 + 5*(ovens.Count-1), 0, 12);
        }
    }
}