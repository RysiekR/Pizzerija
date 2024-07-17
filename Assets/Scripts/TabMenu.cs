using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
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

    private Button BuyDoughB;
    private Button BuySauceB;
    private Button BuyToppingsB;
    private Button LazyBuyB;
    private Button SellPizzaB;
    private Button BuyDeliveryGoblinsB;
    private void Start()
    {
        BuyDoughB = transform.Find("DoughB").GetComponent<Button>();
        BuyDoughB.onClick.AddListener(DeliverySystem.Instance.BuyDough);
        BuySauceB = transform.Find("SauceB").GetComponent<Button>();
        BuySauceB.onClick.AddListener(DeliverySystem.Instance.BuySauce);
        BuyToppingsB = transform.Find("ToppingsB").GetComponent<Button>();
        BuyToppingsB.onClick.AddListener(DeliverySystem.Instance.BuyToppings);
        LazyBuyB = transform.Find("LazyBuyB").GetComponent<Button>();
        LazyBuyB.onClick.AddListener(DeliverySystem.Instance.LazyBuy);
        SellPizzaB = transform.Find("SellPizzaB").GetComponent <Button>();
        SellPizzaB.onClick.AddListener(Pizzeria.Instance.SellPizzaButton);
        BuyDeliveryGoblinsB = transform.Find("BuyDeliveryGoblinB").GetComponent<Button>();
        BuyDeliveryGoblinsB.onClick.AddListener(Distribution.Instance.BuyDeliveryGoblin);
    }
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

        if (Pizzeria.Instance.Money > DeliverySystem.Instance.DoughPrice) DoughButtonText.text = $"Buy(cost: {DeliverySystem.Instance.DoughPrice})";
        else DoughButtonText.text = "Need: " + DeliverySystem.Instance.DoughPrice;

        if (Pizzeria.Instance.Money > DeliverySystem.Instance.SaucePrice) SauceButtunText.text = $"Buy(cost: {DeliverySystem.Instance.SaucePrice})";
        else SauceButtunText.text = "Need: " + DeliverySystem.Instance.SaucePrice;

        if (Pizzeria.Instance.Money > DeliverySystem.Instance.ToppingsPrice) ToppingsButtonText.text = $"Buy(cost: {DeliverySystem.Instance.ToppingsPrice})";
        else ToppingsButtonText.text = "Need: " + DeliverySystem.Instance.ToppingsPrice;

        if (Pizzeria.Instance.Money > DeliverySystem.Instance.LazyBuyPrice) LazyBuyButtonText.text = $"Buy(cost: {DeliverySystem.Instance.LazyBuyPrice})";
        else LazyBuyButtonText.text = "Need: " + DeliverySystem.Instance.LazyBuyPrice;
    }
}
