using System.Collections.Generic;
using UnityEngine;

public class Distribution : MonoBehaviour
{
    public static Distribution Instance;
    public int PizzaAmmount { get; private set; } = 0;
    public List<DeliveryBoy> DeliveryBoys { get; private set; } = new List<DeliveryBoy>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        SellPizzas();
        RestDeliveryBoys(Time.deltaTime);
    }

    private void SellPizzas()
    {
        for (int i = 0; i < DeliveryBoys.Count; i++)
        {
            if (DeliveryBoys[i].Rested)
            {
                int pizzasToPickUp = DeliveryBoys[i].CarryCappacity;
                if (pizzasToPickUp <= PizzaAmmount)
                {
                    DeliveryBoys[i].PickUpPizza(pizzasToPickUp);
                    PizzaAmmount -= pizzasToPickUp;
                }
                else
                {
                    DeliveryBoys[i].PickUpPizza(PizzaAmmount);
                    PizzaAmmount = 0;
                }
                DeliveryBoys[i].SellPizza();
            }
        }
    }

    private void RestDeliveryBoys(float time)
    {
        for (int i = 0; i < DeliveryBoys.Count; i++)
        {
            if (!DeliveryBoys[i].Rested)
            {
                DeliveryBoys[i].Rest(time);
            }
        }
    }

    public void ReceivePizzas(int howMany)
    {
        if (howMany > 0)
        {
            PizzaAmmount += howMany;
        }
    }

    public static void BuyDeliveryBoy()
    {
        if (Pizzeria.Instance.PizzaPrice > 6)
        {
            DeliveryBoy deliveryBoy = new DeliveryBoy();
            if (Pizzeria.Instance.Money >= deliveryBoy.PayPerDay)
            {
                Instance.DeliveryBoys.Add(deliveryBoy);
                Pizzeria.Instance.DeductMoney(deliveryBoy.PayPerDay);
            }
        }
    }
}
