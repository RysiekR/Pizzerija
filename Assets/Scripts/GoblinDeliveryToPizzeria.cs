using UnityEngine;
using UnityEngine.AI;

public class GoblinDeliveryToPizzeria : MonoBehaviour
{

    [SerializeField] private Transform RestingSpot;
    //private Transform PickUpSpot;
    [SerializeField] private GameObject IngredientsVisual;
    private NavMeshAgent NavMeshAgentGoblin;
    public GoblinInventory GoblinInventory;
    public State State = State.WalkingToRest;
    private void Start()
    {
        GoblinInventory = new(3);
        //RestingSpot = transform.Find("RestingSpot").GetComponent<Transform>();
        //IngredientsVisual = transform.parent.Find("Ingredients").gameObject;
        NavMeshAgentGoblin = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        IngredientsVisual.SetActive(GoblinInventory.HasAnything());
        Behave();
    }

    void Behave()
    {
        switch (this.State)
        {
            case State.WalkingToRest:
                WalkTowardsRest(); break;
            case State.WalkingForIngredients:
                WalkForIngredients(); break;
            case State.Waiting:
                WaitingState(); break;
            case State.Delivering:
                Deliver(); break;
            default:
                WaitingState(); break;
        }
    }
    void WalkTowardsRest()
    {
        NavMeshAgentGoblin.SetDestination(RestingSpot.position);
        if (transform.position == RestingSpot.position)
        {
            State = State.Waiting;
        }
        if (DeliverySystem.Instance.TruckWaiting && DeliverySystem.Instance.PickUpSpot != null)
        {
            State = State.WalkingForIngredients;
        }

    }
    void WalkForIngredients()
    {
        if (DeliverySystem.Instance.PickUpSpot != null)
        {
            NavMeshAgentGoblin.SetDestination(DeliverySystem.Instance.PickUpSpot.position);
        }
        /*if (GoblinInventory.IsFull() || (GoblinInventory.HasAnything() && !DeliverySystem.Instance.TruckWaiting))
    {
        State = State.Delivering;
    }*/
        if (GoblinInventory.HasAnything())
        {
            State = State.Delivering;
        }
        else if (!DeliverySystem.Instance.TruckWaiting)
        {
            State = State.WalkingToRest;
        }
    }
    void WaitingState()
    {
        if (DeliverySystem.Instance.TruckWaiting && DeliverySystem.Instance.PickUpSpot.position != null)
        {
            State = State.WalkingForIngredients;
        }
    }
    void Deliver()
    {
        NavMeshAgentGoblin.SetDestination(Pizzeria.Instance.PizzeriaInputSpot.position);
        if (!GoblinInventory.HasAnything())
        {
            if (!DeliverySystem.Instance.TruckWaiting)
            {
                State = State.WalkingToRest;
            }
            else
            {
                State = State.WalkingForIngredients;
            }
        }

    }

    public void SendToWork(Transform workTransform)
    {
        //change state to walkingforingredients
        //PickUpSpot = workTransform;
    }

}


public enum State
{
    Delivering,
    WalkingToRest,
    WalkingForIngredients,
    Waiting,
}

public class GoblinInventory
{
    public int MaxIngredients;
    public int DoughAmount = 0;
    public int SauceAmount = 0;
    public int ToppingsAmount = 0;
    public int TotalAmount = 0;
    public GoblinInventory(int maxIngredients = 3)
    {
        MaxIngredients = maxIngredients;
    }
    /// <summary>
    /// takes away ingredients from ingredients and puts them in goblin inventory leves in ingredients what he cant take
    /// </summary>
    public void PickUpUpToFullInvFrom(Ingredients ingredients)
    {
        /*if (ingredients.DoughAmount >= HowMuchFreeSpace())
        {
            int i = HowMuchFreeSpace();
            PickUpIngredients(i, 0, 0);
            ingredients.Remove(i, 0, 0);
            if (TotalAmount >= MaxIngredients) return;
        }
        if (ingredients.DoughAmount >= HowMuchFreeSpace())
        {
            int i = HowMuchFreeSpace();
            PickUpIngredients(0, i, 0);
            ingredients.Remove(0, i, 0);
            if (TotalAmount >= MaxIngredients) return;
        }
        if (ingredients.SauceAmount >= HowMuchFreeSpace())
        {
            int i = HowMuchFreeSpace();
            PickUpIngredients(0, 0, i);
            ingredients.Remove(0, 0, i);
            if (TotalAmount >= MaxIngredients) return;
        }*/

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
            if (HowMuchFreeSpace() <= 0) return;
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
            if (HowMuchFreeSpace() <= 0) return;
        }
        if (toppings > 0)
        {
            int i;
            if (toppings >= 0)
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
        if (dough + TotalAmount <= MaxIngredients)
        {
            DoughAmount += dough;
            TotalAmount += dough;
        }
        if (sauce + TotalAmount <= MaxIngredients)
        {
            SauceAmount += sauce;
            TotalAmount += sauce;
        }
        if (toppings + TotalAmount <= MaxIngredients)
        {
            ToppingsAmount += toppings;
            TotalAmount += toppings;
        }
    }
    /// <summary>
    /// Warning! it zeros out goblin inventory
    /// </summary>
    public Ingredients PassIngredients()
    {
        Ingredients ingredients = new(DoughAmount, SauceAmount, ToppingsAmount);
        DoughAmount = 0;
        SauceAmount = 0;
        ToppingsAmount = 0;
        TotalAmount = 0;
        return ingredients;
    }

    public int HowMuchFreeSpace()
    {
        return MaxIngredients - TotalAmount;
    }
    public bool HasAnything()
    {
        return TotalAmount > 0;
    }
    public bool IsFull()
    {
        return HowMuchFreeSpace() <= 0;
    }
}
