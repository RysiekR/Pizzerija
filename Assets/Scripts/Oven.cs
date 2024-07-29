using System;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public static List<Oven> ovens = new List<Oven>();
    public OvenInput ovenInput { get; private set; }
    public OvenOutput ovenOutput { get; private set; }
    public int PizzasAmount { get; private set; } = 0;
    public OvenUpgrades Upgrades { get; private set; }
    public OvenBaking Baking { get; private set; }
    public Ingredients Ingredients { get; private set; }
    public GoblinTransporter GoblinWithIngredients { get; private set; } = null;
    public GoblinTransporter GoblinForPizza { get; private set; } = null;
    private void Awake()
    {
        ovens.Add(this);
        ovenInput = GetComponentInChildren<OvenInput>();
        ovenInput.SetOven(this);
        ovenOutput = GetComponentInChildren<OvenOutput>();
        ovenOutput.SetOven(this);
        Upgrades = new OvenUpgrades(this);
        Baking = new OvenBaking(this);
        Ingredients = new Ingredients();
    }
    private void Update()
    {
        if (!Baking.IsBaking)
        {
            if (Ingredients.HasAtLeastOneOfEach())
                StartCoroutine(Baking.Bake());
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
            other.GetComponent<GoblinTransporter>().GoblinInventory.PassIngredients().TransferAllTo(Ingredients);
            GoblinWithIngredients = null;
        }

    }
    public void TriggerOutput(Collider other)
    {
        if(other.GetComponent<GoblinTransporter>().TargetOvenOutput != null)
        {
            if (other.GetComponent<GoblinTransporter>().TargetOvenOutput == ovenOutput.transform)
            {
                int i = Math.Min(other.GetComponent<GoblinTransporter>().GoblinInventory.HowMuchFreeSpace(), PizzasAmount);
                other.GetComponent<GoblinTransporter>().GoblinInventory.PickUpPizzaUpToFullInv(PizzasAmount);
                PizzasAmount -= i;
                HUDScript.Instance.UpdatePizzasToSell();
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
    public bool HasPizzaToPickUp()
    {
        if(GoblinForPizza != null)
            return false;
        if(PizzasAmount > 0) 
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
    public static List<Oven> OvensWithPizzaReady()
    {
        List<Oven> temp = new List<Oven>();
        foreach (Oven o in ovens)
        {
            if(o.HasPizzaToPickUp())
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
}