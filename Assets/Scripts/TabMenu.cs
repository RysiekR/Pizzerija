using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyUGUI;
    [SerializeField] private TextMeshProUGUI PizzasUGUI;
    [SerializeField] private TextMeshProUGUI PizzasSoldUGUI;
    [SerializeField] private TextMeshProUGUI PizzaPriceUGUI;

    private int menuTracker = 0;

    private Button DoughPB;
    private Button DoughMB;
    private Button SaucePB;
    private Button SauceMB;
    private Button ToppingsPB;
    private Button ToppingsMB;
    private Button LazyBuyPB;
    private Button LazyBuyMB;
    private Button BuyB;
    private Button CancelB;

    private TextMeshProUGUI PizzeriaDoughAmountT;
    private TextMeshProUGUI DoughAmountT;
    private TextMeshProUGUI DoughPriceT;
    private TextMeshProUGUI PizzeriaSauceAmountT;
    private TextMeshProUGUI SauceAmountT;
    private TextMeshProUGUI SaucePriceT;
    private TextMeshProUGUI PizzeriaToppingsAmountT;
    private TextMeshProUGUI ToppingsAmountT;
    private TextMeshProUGUI ToppingsPriceT;
    private TextMeshProUGUI LazyBuyAmountT;
    private TextMeshProUGUI LazyBuyPriceT;
    private TextMeshProUGUI TotalPriceT;

    private Button SellPizzaB;
    private Button BuyDeliveryGoblinsB;

    private Button BackToMainMenuB;
    private Button ChangeToShoppingB;

    private Canvas MainMenuFirst;
    private Canvas ShoppingCart;
    private void Start()
    {
        MainMenuFirst = transform.Find("MainMenuFirst").GetComponent<Canvas>();
        ShoppingCart = transform.Find("ShoppingCart").GetComponent<Canvas>();

        DoughPB = ShoppingCart.transform.Find("DoughPB").GetComponent<Button>();
        DoughPB.onClick.AddListener(DeliverySystem.Instance.ShopingCart.AddDough);
        DoughMB = ShoppingCart.transform.Find("DoughMB").GetComponent<Button>();
        DoughMB.onClick.AddListener(DeliverySystem.Instance.ShopingCart.RemoveDough);
        SaucePB = ShoppingCart.transform.Find("SaucePB").GetComponent<Button>();
        SaucePB.onClick.AddListener(DeliverySystem.Instance.ShopingCart.AddSauce);
        SauceMB = ShoppingCart.transform.Find("SauceMB").GetComponent<Button>();
        SauceMB.onClick.AddListener(DeliverySystem.Instance.ShopingCart.RemoveSauce);
        ToppingsPB = ShoppingCart.transform.Find("ToppingsPB").GetComponent<Button>();
        ToppingsPB.onClick.AddListener(DeliverySystem.Instance.ShopingCart.AddToppings);
        ToppingsMB = ShoppingCart.transform.Find("ToppingsMB").GetComponent<Button>();
        ToppingsMB.onClick.AddListener(DeliverySystem.Instance.ShopingCart.RemoveToppings);
        LazyBuyPB = ShoppingCart.transform.Find("LazyBuyPB").GetComponent<Button>();
        LazyBuyPB.onClick.AddListener(DeliverySystem.Instance.ShopingCart.AddLazyBuy);
        LazyBuyMB = ShoppingCart.transform.Find("LazyBuyMB").GetComponent<Button>();
        LazyBuyMB.onClick.AddListener(DeliverySystem.Instance.ShopingCart.RemoveLazyBuy);
        BuyB = ShoppingCart.transform.Find("BuyB").GetComponent<Button>();
        BuyB.onClick.AddListener(DeliverySystem.Instance.ShopingCart.BuyCart);
        CancelB = ShoppingCart.transform.Find("CancelB").GetComponent<Button>();
        CancelB.onClick.AddListener(DeliverySystem.Instance.ShopingCart.ClearCart);

        PizzeriaDoughAmountT = GameObject.Find("PizzeriaDoughAmountT").GetComponent<TextMeshProUGUI>();
        DoughAmountT = GameObject.Find("DoughAmountT").GetComponent<TextMeshProUGUI>();
        DoughPriceT = GameObject.Find("DoughPriceT").GetComponent<TextMeshProUGUI>();
        PizzeriaSauceAmountT = GameObject.Find("PizzeriaSauceAmountT").GetComponent<TextMeshProUGUI>();
        SauceAmountT = GameObject.Find("SauceAmountT").GetComponent<TextMeshProUGUI>();
        SaucePriceT = GameObject.Find("SaucePriceT").GetComponent<TextMeshProUGUI>();
        PizzeriaToppingsAmountT = GameObject.Find("PizzeriaToppingsAmountT").GetComponent<TextMeshProUGUI>();
        ToppingsAmountT = GameObject.Find("ToppingsAmountT").GetComponent<TextMeshProUGUI>();
        ToppingsPriceT = GameObject.Find("ToppingsPriceT").GetComponent<TextMeshProUGUI>();
        LazyBuyAmountT = GameObject.Find("LazyBuyAmountT").GetComponent<TextMeshProUGUI>();
        LazyBuyPriceT = GameObject.Find("LazyBuyPriceT").GetComponent<TextMeshProUGUI>();
        TotalPriceT = GameObject.Find("TotalPriceT").GetComponent<TextMeshProUGUI>();

        BackToMainMenuB = ShoppingCart.transform.Find("BackB").GetComponent<Button>();
        BackToMainMenuB.onClick.AddListener(() => { menuTracker = 0; });
        ChangeToShoppingB = MainMenuFirst.transform.Find("ChangeToShoppingB").GetComponent<Button>();
        ChangeToShoppingB.onClick.AddListener(() => { menuTracker = 1; });

        SellPizzaB = MainMenuFirst.transform.Find("SellPizzaB").GetComponent<Button>();
        SellPizzaB.onClick.AddListener(Pizzeria.Instance.SellPizzaButton);
        BuyDeliveryGoblinsB = MainMenuFirst.transform.Find("BuyDeliveryGoblinB").GetComponent<Button>();
        BuyDeliveryGoblinsB.onClick.AddListener(Distribution.Instance.BuyDeliveryGoblin);
    }
    private void Update()
    {
        CanvasKeeper();
        RefreshMoneyText();
        RefreshPizzaText();
        RefreshIngridients();
        BuyDeliveryGoblinsB.gameObject.SetActive(Pizzeria.Instance.PizzaPrice > 6);

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
        /*DoughUGUI.text = "Dough: " + Pizzeria.Instance.Ingredients.DoughAmount.ToString();
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
*/
        PizzeriaDoughAmountT.text = "in pizzerija:" + Pizzeria.Instance.Ingredients.DoughAmount.ToString();
        PizzeriaSauceAmountT.text = "in pizzerija:" + Pizzeria.Instance.Ingredients.SauceAmount.ToString();
        PizzeriaToppingsAmountT.text = "in pizzerija:" + Pizzeria.Instance.Ingredients.ToppingsAmount.ToString();
        DoughAmountT.text = DeliverySystem.Instance.ShopingCart.ShoppingCartIngredients.DoughAmount.ToString();
        DoughPriceT.text = "price: " + DeliverySystem.Instance.DoughPrice.ToString();
        SauceAmountT.text = DeliverySystem.Instance.ShopingCart.ShoppingCartIngredients.SauceAmount.ToString();
        SaucePriceT.text = "price: " + DeliverySystem.Instance.SaucePrice.ToString();
        ToppingsAmountT.text = DeliverySystem.Instance.ShopingCart.ShoppingCartIngredients.ToppingsAmount.ToString();
        ToppingsPriceT.text = "price: " + DeliverySystem.Instance.ToppingsPrice.ToString();
        LazyBuyAmountT.text = DeliverySystem.Instance.ShopingCart.LazyBuys.ToString();
        LazyBuyPriceT.text = "price: " + DeliverySystem.Instance.LazyBuyPrice.ToString();
        TotalPriceT.text = "Total price: " + DeliverySystem.Instance.ShopingCart.TotalPrice.ToString();

    }

    private void CanvasKeeper()
    {
        MainMenuFirst.gameObject.SetActive(menuTracker == 0);
        ShoppingCart.gameObject.SetActive(menuTracker == 1);
    }
}
