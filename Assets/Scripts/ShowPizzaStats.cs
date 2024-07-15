using TMPro;
using UnityEngine;

public class ShowPizzaStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyUGUI;
    [SerializeField] private TextMeshProUGUI PizzasUGUI;
    [SerializeField] private TextMeshProUGUI PizzasSoldUGUI;
    [SerializeField] private TextMeshProUGUI PizzaPriceUGUI;

    [SerializeField] private TextMeshProUGUI DoughUGUI;
    [SerializeField] private TextMeshProUGUI SauceUGUI;
    [SerializeField] private TextMeshProUGUI ToppingsUGUI;
    [SerializeField] private TextMeshProUGUI LazyBuyUGUI;

    [SerializeField] private TextMeshProUGUI DoughButtonText;
    [SerializeField] private TextMeshProUGUI SauceButtunText;
    [SerializeField] private TextMeshProUGUI ToppingsButtonText;
    [SerializeField] private TextMeshProUGUI LazyBuyButtonText;


    private void Update()
    {
        RefreshMoneyText();
        RefreshPizzaText();
        RefreshIngridients();
    }

    private void RefreshMoneyText()
    {
        MoneyUGUI.text = "Money: " + Pizzeria.Instance.Money.ToString();
    }

    private void RefreshPizzaText()
    {
        PizzasUGUI.text = "Pizzas: " + Pizzeria.Instance.PizzasAmmount.ToString();
        PizzasSoldUGUI.text = "Pizzas sold: " + Pizzeria.Instance.PizzasSold.ToString();
        PizzaPriceUGUI.text = "Pizza price: " + Pizzeria.Instance.PizzaPrice.ToString();
    }

    private void RefreshIngridients()
    {
        DoughUGUI.text = "Dough: " + Pizzeria.Instance.Ingredients.DoughAmount.ToString();
        SauceUGUI.text = "Sauce: " + Pizzeria.Instance.Ingredients.SauceAmount.ToString();
        ToppingsUGUI.text = "Toppings: " + Pizzeria.Instance.Ingredients.ToppingsAmount.ToString();
        LazyBuyUGUI.text = "Lazy buy cost more!";

        if (Pizzeria.Instance.Money > Pizzeria.Instance.DoughPrice) DoughButtonText.text = $"Buy(cost: {Pizzeria.Instance.DoughPrice})";
        else DoughButtonText.text = "Need: " + Pizzeria.Instance.DoughPrice;

        if (Pizzeria.Instance.Money > Pizzeria.Instance.SaucePrice) SauceButtunText.text = $"Buy(cost: {Pizzeria.Instance.SaucePrice})";
        else SauceButtunText.text = "Need: " + Pizzeria.Instance.SaucePrice;

        if (Pizzeria.Instance.Money > Pizzeria.Instance.ToppingsPrice) ToppingsButtonText.text = $"Buy(cost: {Pizzeria.Instance.ToppingsPrice})";
        else ToppingsButtonText.text = "Need: " + Pizzeria.Instance.ToppingsPrice;

        if (Pizzeria.Instance.Money > Pizzeria.Instance.LazyBuyPrice) LazyBuyButtonText.text = $"Buy(cost: {Pizzeria.Instance.LazyBuyPrice})";
        else LazyBuyButtonText.text = "Need: " + Pizzeria.Instance.LazyBuyPrice;
    }

}
