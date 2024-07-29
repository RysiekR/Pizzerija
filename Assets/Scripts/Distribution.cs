using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Distribution : MonoBehaviour
{
    public static Distribution Instance;
    public Transform DistributionInputSpot;
    public int PizzaAmmount { get; private set; } = 0;
    public List<DeliveryGoblin> DeliveryGoblins { get; private set; } = new List<DeliveryGoblin>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DeliveryGoblins.Add(new DeliveryGoblin("Debug name", 1, 5, 10));
        }
    }

    private void Update()
    {
        if (PizzaAmmount > 0)
        {
            SellPizzas();
        }
        RestDeliveryBoys(Time.deltaTime);
    }

    private void SellPizzas()
    {
        for (int i = 0; i < DeliveryGoblins.Count; i++)
        {
            if (DeliveryGoblins[i].Rested)
            {
                int pizzasToPickUp = DeliveryGoblins[i].CarryCappacity;
                if (pizzasToPickUp <= PizzaAmmount)
                {
                    DeliveryGoblins[i].PickUpPizza(pizzasToPickUp);
                    PizzaAmmount -= pizzasToPickUp;
                }
                else
                {
                    DeliveryGoblins[i].PickUpPizza(PizzaAmmount);
                    PizzaAmmount = 0;
                }
                DeliveryGoblins[i].SellPizza();
            }
        }
    }

    private void RestDeliveryBoys(float time)
    {
        for (int i = 0; i < DeliveryGoblins.Count; i++)
        {
            if (!DeliveryGoblins[i].Rested)
            {
                DeliveryGoblins[i].Rest(time);
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

    public void BuyDeliveryGoblin()
    {
        if (Pizzeria.Instance.PizzaPrice > 6)
        {
            DeliveryGoblin deliveryBoy = new DeliveryGoblin("Debug name",2,5,10);
            if (Pizzeria.Instance.CanPayWithMoney(deliveryBoy.PayPerDay))
            {
                Instance.DeliveryGoblins.Add(deliveryBoy);
                Pizzeria.Instance.DeductMoney(deliveryBoy.PayPerDay);
            }
        }
    }

    public string RestingTimes()
    {
        StringBuilder sb = new();
        foreach (DeliveryGoblin deliveryGoblin in Instance.DeliveryGoblins)
        {
            float restDifference = deliveryGoblin.RestSpeed - deliveryGoblin.RestTime;
            sb.Append(restDifference.ToString("F1"));
            sb.Append(", ");
        }

        if (sb.Length > 2)
        {
            sb.Length -= 2;
        }

        return sb.ToString();
    }
}
