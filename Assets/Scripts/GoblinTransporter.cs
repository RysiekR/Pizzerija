using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GoblinTransporter : MonoBehaviour
{
    public static List<GoblinTransporter> Goblins { get; private set; } = new List<GoblinTransporter>();
    public static int GoblinCost => Goblins.Count * 10;
    
    public GoblinInventory GoblinInventory;
    private TextMeshProUGUI MaxCarryT;
    public Animator Animator;
    public NavMeshAgent NavMeshAgentGoblin;

    public GoblinState State { get; private set; }
    public int[] JobPriority { get; private set; } = { 4, 5, 1 };//ingrtooven, sellpizza, ingrefromtruck
    public int OvenPriority { get; set; } = 2;// start: 0 from oldest, 1 from newest, 2 random
    private bool IsChangingState = false;

    [SerializeField] private GameObject IngredientsVisual;
    [SerializeField] private GameObject PizzaBox;

    public Transform RestingSpot;
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
        SetState(new WalkingToRestState(this));
    }
    private void Start()
    {
        MaxCarryT = transform.Find("Canvas").Find("MaxCarryT").GetComponent<TextMeshProUGUI>();
        NavMeshAgentGoblin = GetComponent<NavMeshAgent>();
        UpdateCarryCap();
        RestingSpot = RestingSpotTrigger.RestingSpot;
        ResetRestSpot();
    }

    private void Update()
    {
        StateExecution();
        CheckIfNotCarrying();
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
    }
    public void ResetRestSpot()
    {
        if (HutHouse.HutHouseList.Any(h => h.HutMembers.Contains(this)))
        {
            foreach (var h in HutHouse.HutHouseList)
            {
                foreach (var g in h.HutMembers)
                {
                    if (g == this)
                    {
                        RestingSpot = h.Inside;
                        return;
                    }
                }
            }
        }
        Debug.Log("Goblin didnt found home");

    }
    public void RemoveOvenToHandle(Oven oven)
    {
        if (oven != ovenToHandle && oven !=null)
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
    public static void BuyGoblinTransporter()
    {
        if (Pizzeria.Instance.Money >= GoblinCost)
        {
            Pizzeria.Instance.DeductMoney(GoblinCost);
            Instantiate(Pizzeria.Instance.GoblinTransporterPrefab);
        }
    }
}

