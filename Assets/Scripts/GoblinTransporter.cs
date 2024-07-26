using System;
using UnityEngine;
using UnityEngine.AI;

public class GoblinTransporter : MonoBehaviour
{
    [SerializeField] public Transform RestingSpot;
    [SerializeField] private GameObject IngredientsVisual;
    [SerializeField] private GameObject PizzaBox;
    public NavMeshAgent NavMeshAgentGoblin;
    public GoblinInventory GoblinInventory;
    public GoblinState State {  get; private set; }
    //public TransportGoblinState State = TransportGoblinState.WalkingToRest;
    private void Start()
    {
        GoblinInventory = new();
        NavMeshAgentGoblin = GetComponent<NavMeshAgent>();
        SetState(new WalkingToRestState(this));
    }

    private void Update()
    {
        IngredientsVisual.SetActive(GoblinInventory.HasAnything());
        PizzaBox.SetActive(GoblinInventory.HasPizza());
        State.UpdateState();
        State.ExecuteBehaviour();
    }

    public void SetState(GoblinState newState)
    {
        State = newState;
    }

}
public enum TransportGoblinState
{
    WalkingToRest,
    Waiting,
    DeliveringIngredientsToPizzeria,
    WalkingForIngredientsToTruck,
    TransportPizzaToDistribution,
    WalkingToPickUpPizza,
}

public class GoblinInventory
{
    private const int baseCarryCap = 1;
    public int CarryCappacity;
    public int DoughAmount = 0;
    public int SauceAmount = 0;
    public int ToppingsAmount = 0;
    public int PizzasAmount = 0;
    public int TotalAmount = 0;
    public int CarriedTottal = 0;
    public GoblinInventory()
    {
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
    private void PickUpIngredients(int dough, int sauce, int toppings)
    {
        if (dough + TotalAmount <= CarryCappacity)
        {
            DoughAmount += dough;
            TotalAmount += dough;
        }
        if (sauce + TotalAmount <= CarryCappacity)
        {
            SauceAmount += sauce;
            TotalAmount += sauce;
        }
        if (toppings + TotalAmount <= CarryCappacity)
        {
            ToppingsAmount += toppings;
            TotalAmount += toppings;
        }
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
            int pizzasToPickUp = Math.Min(pizzas, HowMuchFreeSpace());
            TotalAmount += pizzasToPickUp;
            PizzasAmount += pizzasToPickUp;
            pizzas -= pizzasToPickUp;
        }
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
        CarriedTottal += TotalAmount;
        TotalAmount -= PizzasAmount;
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
    }
}
