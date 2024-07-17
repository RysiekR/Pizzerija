using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    public static List<DeliveryCar> deliveryCars = new List<DeliveryCar>();
    public static DeliverySystem Instance;
    public Ingredients Ingredients;
    public int DoughPrice { get; private set; } = 3;
    public int SaucePrice { get; private set; } = 1;
    public int ToppingsPrice { get; private set; } = 2;
    public int LazyBuyPrice { get; private set; } = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Ingredients = new Ingredients(2, 2, 2);
        CalculateLazyBuyPrice();
    }

    private void Update()
    {
        if (deliveryCars.Count != 0)
        {
            TransferIngredientsTo(deliveryCars[0]);
        }
    }

    public bool HasAnyIngredients()
    {
        return Ingredients.HasAny();
    }

    public void TransferIngredientsTo(DeliveryCar deliveryCar)
    {
        Ingredients.TransferAllTo(deliveryCar.CarIngredients);
    }

    public void BuyDough()
    {
        if (Pizzeria.Instance.CanPayWithMoney(DoughPrice))
        {
            Pizzeria.Instance.DeductMoney(DoughPrice);
            Ingredients.AddDough(1);
        }
    }

    public void BuySauce()
    {
        if (Pizzeria.Instance.CanPayWithMoney(SaucePrice))
        {
            Pizzeria.Instance.DeductMoney(SaucePrice);
            Ingredients.AddSauce(1);
        }

    }

    public void BuyToppings()
    {
        if (Pizzeria.Instance.CanPayWithMoney(ToppingsPrice))
        {
            Pizzeria.Instance.DeductMoney(ToppingsPrice);
            Ingredients.AddToppings(1);
        }
    }

    public void LazyBuy()
    {
        if (Pizzeria.Instance.CanPayWithMoney(LazyBuyPrice))
        {
            Pizzeria.Instance.DeductMoney(LazyBuyPrice);
            Ingredients.AddDough(1);
            Ingredients.AddSauce(1);
            Ingredients.AddToppings(1);
        }
    }
    private void CalculateLazyBuyPrice()
    {
        LazyBuyPrice = DoughPrice + SaucePrice + ToppingsPrice + 3;

    }

}
