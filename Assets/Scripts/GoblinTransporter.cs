using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GoblinTransporter : MonoBehaviour
{
    public static List<GoblinTransporter> Goblins { get; private set; } = new List<GoblinTransporter>();
    public static int GoblinCost => Goblins.Count * 10;
    public Transform RestingSpot;
    [SerializeField] private GameObject IngredientsVisual;
    [SerializeField] private GameObject PizzaBox;
    private TextMeshProUGUI MaxCarryT;
    public Animator Animator;
    public NavMeshAgent NavMeshAgentGoblin;
    public GoblinInventory GoblinInventory;
    [field: SerializeField] public GoblinState State { get; private set; }
    private bool IsChangingState = false;
    public Transform TargetOvenInput;
    public Transform TargetOvenOutput;
    public Oven ovenToHandle;
    private void Awake()
    {
        GoblinInventory = new(this);
        if (!Goblins.Contains(this))
        {
            Goblins.Add(this);
        }
    }
    private void Start()
    {
        MaxCarryT = transform.Find("Canvas").Find("MaxCarryT").GetComponent<TextMeshProUGUI>();
        NavMeshAgentGoblin = GetComponent<NavMeshAgent>();
        SetState(new WalkingToRestState(this));
        UpdateCarryCap();
        RestingSpot = RestingSpotTrigger.RestingSpot;
    }

    private void Update()
    {
        Animator.SetBool("IsCarrying", GoblinInventory.HasAnything());
        Animator.SetBool("IsMoving", State is not WaitingState);
        StateExecution();
    }
    public void UpdateCarryCap()
    {
        if (MaxCarryT != null)
        {
            MaxCarryT.text = $"Carry: {GoblinInventory.TotalAmount} / {GoblinInventory.CarryCappacity.ToString()} \n" +
                $"Exp: {GoblinInventory.CarriedTottal} / {GoblinInventory.ExpNeeded()}";
        }
    }
    private void StateExecution()
    {
        if (!IsChangingState)
        {
            State.UpdateState();
            State.ExecuteBehaviour();
        }
        CheckIfNotCarrying();
    }
    public void RemoveOvenToHandle(Oven oven)
    {
        if (oven != ovenToHandle)
            Debug.Log("GOBLINTRANSPORTER romoving wrong oven");
        else
        {
            ovenToHandle = null;
            TargetOvenInput = null;
        }
    }
    public void SetState(GoblinState newState)
    {
        if (newState is DeliveringIngredientsToPizzeriaState || newState is WalkToDropOffPizzaState || newState is DeliverToOven)
        {
            NavMeshAgentGoblin.isStopped = true;
            IsChangingState = true;
            StartCoroutine(ChangingStateVisuals());
        }
        State = newState;
    }
    IEnumerator ChangingStateVisuals()
    {

        yield return new WaitForSeconds(0.8f);
        IngredientsVisual.SetActive(GoblinInventory.HasIngredients());
        PizzaBox.SetActive(GoblinInventory.HasPizza());
        NavMeshAgentGoblin.isStopped = false;
        IsChangingState = false;
    }
    void CheckIfNotCarrying()
    {
        if (!GoblinInventory.HasAnything())
        {
            IngredientsVisual.SetActive(false);
            PizzaBox.SetActive(false);
        }

    }
    public void ManageTransferFromShelfToGoblinInventory()
    {
        if (ovenToHandle == null)
        {
            TargetOvenInput = null;
            SetState(new WalkingToRestState(this));
            return;
        }
        //if(ovenToHandle.GoblinWithIngredients != this)
        if (!ovenToHandle.GoblinTransportersWithIngredients.Contains(this))
        {
            ovenToHandle = null;
            TargetOvenInput = null;
            SetState(new WalkingToRestState(this));
            return;
        }
        // Transfer Dough
        if (Pizzeria.Instance.Ingredients.DoughAmount > 0)
        {
            int i = GoblinInventory.HowMuchFreeSpace();
            int j = ovenToHandle.Ingredients.DoughAmount;
            int k = Pizzeria.Instance.Ingredients.DoughAmount;
            int l = ovenToHandle.Baking.BakeAmount;

            int neededDough = l - j;
            int doughAmountFreeSpace = Mathf.Min(i, neededDough);
            int finalDoughAmount = Mathf.Min(k, doughAmountFreeSpace);
            if (finalDoughAmount > 0)
            {
                GoblinInventory.PickUpIngredients(finalDoughAmount, 0, 0);
                Pizzeria.Instance.Ingredients.Remove(finalDoughAmount, 0, 0);
            }

            if (GoblinInventory.IsFull()) return;
        }

        // Transfer Sauce
        if (Pizzeria.Instance.Ingredients.SauceAmount > 0)
        {
            int i = GoblinInventory.HowMuchFreeSpace();
            int j = ovenToHandle.Ingredients.SauceAmount;
            int k = Pizzeria.Instance.Ingredients.SauceAmount;
            int l = ovenToHandle.Baking.BakeAmount;

            int needed = l - j;
            int sauceAmountFreeSpace = Mathf.Min(i, needed);
            int finalSauceAmount = Mathf.Min(k, sauceAmountFreeSpace);
            if (finalSauceAmount > 0)
            {
                GoblinInventory.PickUpIngredients(0, finalSauceAmount, 0);
                Pizzeria.Instance.Ingredients.Remove(0, finalSauceAmount, 0);
            }
            if (GoblinInventory.IsFull()) return;
        }

        // Transfer Toppings
        if (Pizzeria.Instance.Ingredients.ToppingsAmount > 0)
        {
            int i = GoblinInventory.HowMuchFreeSpace();
            int j = ovenToHandle.Ingredients.ToppingsAmount;
            int k = Pizzeria.Instance.Ingredients.ToppingsAmount;
            int l = ovenToHandle.Baking.BakeAmount;

            int needed = l - j;
            int toppingsAmountFreeSpace = Mathf.Min(i, needed);
            int finalToppingsAmount = Mathf.Min(k, toppingsAmountFreeSpace);
            if (finalToppingsAmount > 0)
            {
                GoblinInventory.PickUpIngredients(0, 0, finalToppingsAmount);
                Pizzeria.Instance.Ingredients.Remove(0, 0, finalToppingsAmount);
            }
            if (GoblinInventory.IsFull()) return;
        }
    }
    public static void BuyGoblinTransporter()
    {
        if (Pizzeria.Instance.Money >= GoblinCost)
        {
            Pizzeria.Instance.DeductMoney(GoblinCost);
            Instantiate(Pizzeria.Instance.GoblinTransporterPrefab);
        }
    }
}
/* {
     if(Pizzeria.Instance.Ingredients.DoughAmount > 0)
     {
         int i = GoblinInventory.HowMuchFreeSpace();
         int j = ovenToHandle.Ingredients.DoughAmount;
         int k = Pizzeria.Instance.Ingredients.DoughAmount;
         int l = ovenToHandle.Baking.BakeAmount;

         int needed = l - j;

         int doughAmountFreeSpace = Math.Min(i, needed);
         int finalDoughAmount = Math.Min(k, doughAmountFreeSpace);

         GoblinInventory.DoughAmount = finalDoughAmount;
         Pizzeria.Instance.Ingredients.Remove(finalDoughAmount, 0, 0);
         if(GoblinInventory.IsFull()) return;
     }
 }*/

public class GoblinInventory
{
    GoblinTransporter owner;
    private const int baseCarryCap = 1;
    public int CarryCappacity;
    public int DoughAmount = 0;
    public int SauceAmount = 0;
    public int ToppingsAmount = 0;
    public int PizzasAmount = 0;
    public int TotalAmount = 0;
    public int CarriedTottal = 0;
    public GoblinInventory(GoblinTransporter goblinOwner)
    {
        owner = goblinOwner;
        UpdateCarryCappacity();
    }
    /// <summary>
    /// debug only, sets carried total to see if goblin can carry more for each log base he carried
    /// </summary>
    public GoblinInventory(int carriedTottal)
    {
        CarriedTottal = carriedTottal;
        UpdateCarryCappacity();
    }
    /// <summary>
    /// takes away ingredients from ingredients and puts them in goblin inventory leves in ingredients what he cant take
    /// </summary>
    public void PickUpUpToFullInvFrom(Ingredients ingredients)
    {
        int dough = ingredients.DoughAmount;
        int sauce = ingredients.SauceAmount;
        int toppings = ingredients.ToppingsAmount;
        if (dough > 0)
        {
            int i;
            if (dough >= HowMuchFreeSpace())
            {
                i = HowMuchFreeSpace();
            }
            else
            {
                i = dough;
            }
            PickUpIngredients(i, 0, 0);
            ingredients.Remove(i, 0, 0);
            if (IsFull()) return;
        }
        if (sauce > 0)
        {
            int i;
            if (sauce >= HowMuchFreeSpace())
            {
                i = HowMuchFreeSpace();
            }
            else
            {
                i = sauce;
            }
            PickUpIngredients(0, i, 0);
            ingredients.Remove(0, i, 0);
            if (IsFull()) return;
        }
        if (toppings > 0)
        {
            int i;
            if (toppings >= HowMuchFreeSpace())
            {
                i = HowMuchFreeSpace();
            }
            else
            {
                i = toppings;
            }
            PickUpIngredients(0, 0, i);
            ingredients.Remove(0, 0, i);
        }
    }
    public void PickUpIngredients(int dough, int sauce, int toppings)
    {
        if (dough <= HowMuchFreeSpace())
        {
            DoughAmount += dough;
            TotalAmount += dough;
        }
        if (sauce <= HowMuchFreeSpace())
        {
            SauceAmount += sauce;
            TotalAmount += sauce;
        }
        if (toppings <= HowMuchFreeSpace())
        {
            ToppingsAmount += toppings;
            TotalAmount += toppings;
        }
        UpdateCarryCappacity();
    }
    /// <summary>
    /// Picks up and returns the number of pizzas that couldn't be picked up due to insufficient space.
    /// </summary>
    /// <param name="pizzas">The number of pizzas to pick up.</param>
    /// <returns>The number of pizzas that couldn't be picked up.</returns>
    public int PickUpPizzaUpToFullInv(int pizzas)
    {
        if (pizzas > 0)
        {
            int pizzasToPickUp = Mathf.Min(pizzas, HowMuchFreeSpace());
            TotalAmount += pizzasToPickUp;
            PizzasAmount += pizzasToPickUp;
            pizzas -= pizzasToPickUp;
        }
        UpdateCarryCappacity();

        return pizzas;
    }
    /// <summary>
    /// Warning! it zeros out ingredients in goblin inventory
    /// </summary>
    public Ingredients PassIngredients()
    {
        Ingredients ingredients = new(DoughAmount, SauceAmount, ToppingsAmount);
        CarriedTottal += TotalAmount;
        TotalAmount -= DoughAmount;
        TotalAmount -= SauceAmount;
        TotalAmount -= ToppingsAmount;
        DoughAmount = 0;
        SauceAmount = 0;
        ToppingsAmount = 0;
        UpdateCarryCappacity();
        return ingredients;
    }
    /// <summary>
    /// Warning! it zeros out pizzas in goblin inventory
    /// </summary>
    public int PassPizzas()
    {
        int i = PizzasAmount;
        CarriedTottal += i;
        TotalAmount -= i;
        PizzasAmount = 0;
        UpdateCarryCappacity();
        return i;
    }
    public int HowMuchFreeSpace()
    {
        return CarryCappacity - TotalAmount;
    }
    public bool HasAnything()
    {
        return TotalAmount > 0;
    }
    public bool HasPizza()
    {
        return PizzasAmount > 0;
    }
    public bool HasIngredients()
    {
        return DoughAmount > 0 || SauceAmount > 0 || ToppingsAmount > 0;
    }
    public bool IsFull()
    {
        return HowMuchFreeSpace() <= 0;
    }
    private void UpdateCarryCappacity()
    {
        CarryCappacity = (int)Mathf.Log(CarriedTottal, 4) + baseCarryCap;
        if (CarryCappacity <= 0) CarryCappacity = 1;
        owner.UpdateCarryCap();
    }
    public int ExpNeeded()
    {
        if (CarryCappacity <= 1)
            return 4;
        else return (int)Mathf.Pow(CarryCappacity, 4);
    }
}