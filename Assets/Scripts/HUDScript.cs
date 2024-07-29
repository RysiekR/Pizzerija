using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    public static HUDScript Instance;
    TextMeshProUGUI money;
    TextMeshProUGUI pizzasToSell;
    TextMeshProUGUI goblinsWaiting;
    TextMeshProUGUI noIngredients;
    TextMeshProUGUI ETANextCarT;
    TextMeshProUGUI TodayBonusT;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        TodayBonusT = transform.Find("TodayBonusT").GetComponent <TextMeshProUGUI>();
    }

    private void Start()
    {
        money = transform.Find("MoneyTMPT").GetComponent<TextMeshProUGUI>();
        pizzasToSell = transform.Find("PizzasToSellTMPT").GetComponent<TextMeshProUGUI>();
        goblinsWaiting = transform.Find("GoblinsAreWaitingTMPT").GetComponent<TextMeshProUGUI>();
        noIngredients = transform.Find("PizzerijaIngrTMPT").GetComponent<TextMeshProUGUI>();
        ETANextCarT = transform.Find("ETANextCarT").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        UpdateMoney();
        UpdatePizzasToSell();
        UpdateGoblinsWaiting();
        UpdateNoIngredients();
        UpdateETA();
    }
    void UpdateETA()
    {
        if (DeliverySystem.Instance.AutomaticDeliveriesON)
        {
            if (DeliverySystem.Instance.ETANextCar != 5)
            {
                ETANextCarT.text = "Next delivery in: " + DeliverySystem.Instance.ETANextCar.ToString();
            }
            else if (!DeliverySystem.Instance.HasDeliveriesInQueue())
            {
                ETANextCarT.text = "No orders";
            }
            else if (DeliverySystem.Instance.ETANextCar == 5)
            {
                ETANextCarT.text = "Delivery waiting";
            }
        }
        else
        {
            ETANextCarT.text = "Auto deliveries OFF";
        }
    }
    public void UpdateTodayBonus()
    {
        TodayBonusT.text = "Today bonus: 2x" + DayCycle.TodayBonus;
    }
    void UpdateMoney()
    {
        money.text = "Money: " + Pizzeria.Instance.Money.ToString();
    }
    public void UpdatePizzasToSell()
    {
        pizzasToSell.text = "Pizzas ready in ovens: " + Oven.SumOfReadyPizzas().ToString();
    }
    void UpdateGoblinsWaiting()
    {
        if (Distribution.Instance.DeliveryGoblins.Count <= 0)
        {
            goblinsWaiting.transform.gameObject.SetActive(false);
            return;
        }
        if (Distribution.Instance.DeliveryGoblins.All(g => !g.Rested))
        {
            goblinsWaiting.transform.gameObject.SetActive(false);
            return;
        }
        goblinsWaiting.transform.gameObject.SetActive(true);
    }

    void UpdateNoIngredients()
    {
        noIngredients.transform.gameObject.SetActive(!Pizzeria.Instance.CanBake());
    }
}