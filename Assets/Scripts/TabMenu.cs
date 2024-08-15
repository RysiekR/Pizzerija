using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyUGUI;
    [SerializeField] private TextMeshProUGUI PizzasUGUI;
    [SerializeField] private TextMeshProUGUI PizzasSoldUGUI;
    [SerializeField] private TextMeshProUGUI PizzaPriceUGUI;

    private int menuTracker = 2;

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
    private Button StartSendingTrucksB;
    private Button StopSendingTrucksB;

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
    private TextMeshProUGUI MoneyT;
    private TextMeshProUGUI TodayPromotionT;


    //private Button SellPizzaB;
    private Button BuyDeliveryGoblinsB;
    private Button BuyGoblinB;
    private Button BuyOvenB;

    private TextMeshProUGUI BuyOvenT;
    private TextMeshProUGUI BuyGoblinT;


    private Button BackToMainMenuB;
    private Button ChangeToShoppingB;
    private Button ChangeToHelpB;

    private Canvas MainMenuFirst;
    private Canvas ShoppingCart;

    private Canvas Help;
    private Button BackToMainMenu2B;
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
        StartSendingTrucksB = ShoppingCart.transform.Find("StartSendingTrucksB").GetComponent<Button>();
        StartSendingTrucksB.onClick.AddListener(DeliverySystem.Instance.AutomaticSendOfTrucks);
        StopSendingTrucksB = ShoppingCart.transform.Find("StopSendingTrucksB").GetComponent<Button>();
        StopSendingTrucksB.onClick.AddListener(DeliverySystem.Instance.StopAutomaticSendOfTrucks);


        PizzeriaDoughAmountT = ShoppingCart.transform.Find("PizzeriaDoughAmountT").GetComponent<TextMeshProUGUI>();
        DoughAmountT = ShoppingCart.transform.Find("DoughAmountT").GetComponent<TextMeshProUGUI>();
        DoughPriceT = ShoppingCart.transform.Find("DoughPriceT").GetComponent<TextMeshProUGUI>();
        PizzeriaSauceAmountT = ShoppingCart.transform.Find("PizzeriaSauceAmountT").GetComponent<TextMeshProUGUI>();
        SauceAmountT = ShoppingCart.transform.Find("SauceAmountT").GetComponent<TextMeshProUGUI>();
        SaucePriceT = ShoppingCart.transform.Find("SaucePriceT").GetComponent<TextMeshProUGUI>();
        PizzeriaToppingsAmountT = ShoppingCart.transform.Find("PizzeriaToppingsAmountT").GetComponent<TextMeshProUGUI>();
        ToppingsAmountT = ShoppingCart.transform.Find("ToppingsAmountT").GetComponent<TextMeshProUGUI>();
        ToppingsPriceT = ShoppingCart.transform.Find("ToppingsPriceT").GetComponent<TextMeshProUGUI>();
        LazyBuyAmountT = ShoppingCart.transform.Find("LazyBuyAmountT").GetComponent<TextMeshProUGUI>();
        LazyBuyPriceT = ShoppingCart.transform.Find("LazyBuyPriceT").GetComponent<TextMeshProUGUI>();
        TotalPriceT = ShoppingCart.transform.Find("TotalPriceT").GetComponent<TextMeshProUGUI>();
        MoneyT = ShoppingCart.transform.Find("MoneyT").GetComponent<TextMeshProUGUI>();
        TodayPromotionT = ShoppingCart.transform.Find("TodayPromotionT").GetComponent<TextMeshProUGUI>();

        BackToMainMenuB = ShoppingCart.transform.Find("BackB").GetComponent<Button>();
        BackToMainMenuB.onClick.AddListener(() => { menuTracker = 0; });
        ChangeToShoppingB = MainMenuFirst.transform.Find("ChangeToShoppingB").GetComponent<Button>();
        ChangeToShoppingB.onClick.AddListener(() => { menuTracker = 1; });
        ChangeToHelpB = MainMenuFirst.transform.Find("ChangeToHelpB").GetComponent<Button>();
        ChangeToHelpB.onClick.AddListener(() => { menuTracker = 2; });

        //SellPizzaB = MainMenuFirst.transform.Find("SellPizzaB").GetComponent<Button>();
        //SellPizzaB.onClick.AddListener(Pizzeria.Instance.SellPizzaButton);
        //BuyDeliveryGoblinsB = MainMenuFirst.transform.Find("BuyDeliveryGoblinB").GetComponent<Button>();
        //BuyDeliveryGoblinsB.onClick.AddListener(Distribution.Instance.BuyDeliveryGoblin);
        BuyOvenB = MainMenuFirst.transform.Find("BuyOvenB").GetComponent<Button>();
        BuyOvenB.onClick.AddListener(Oven.BuyOven);
        BuyOvenB.onClick.AddListener(UpdateCost);
/*        BuyGoblinB = MainMenuFirst.transform.Find("BuyGoblinB").GetComponent<Button>();
        BuyGoblinB.onClick.AddListener(GoblinTransporter.BuyGoblinTransporter);
        BuyGoblinB.onClick.AddListener(UpdateCost);
*/
        BuyGoblinT = MainMenuFirst.transform.Find("BuyGoblinT").GetComponent<TextMeshProUGUI>();
        BuyOvenT = MainMenuFirst.transform.Find("BuyOvenT").GetComponent<TextMeshProUGUI>();


        Help = transform.Find("Help").GetComponent<Canvas>();
        BackToMainMenu2B = Help.transform.Find("BackToMainMenu2B").GetComponent<Button>();
        BackToMainMenu2B.onClick.AddListener(() => { menuTracker = 0; });

    }
    private void Update()
    {
        CanvasKeeper();
        RefreshMoneyText();
        RefreshPizzaText();
        RefreshIngridients();
        //BuyDeliveryGoblinsB.gameObject.SetActive(Pizzeria.Instance.PizzaPrice > 6);
        AutoDeliveries();
        UpdateCost();
    }
    private void AutoDeliveries()
    {
        StartSendingTrucksB.gameObject.SetActive(!DeliverySystem.Instance.AutomaticDeliveriesON);
        StopSendingTrucksB.gameObject.SetActive(DeliverySystem.Instance.AutomaticDeliveriesON);
    }

    private void RefreshMoneyText()
    {
        MoneyUGUI.text = "Money: " + Pizzeria.Instance.Money.ToString();
    }

    private void RefreshPizzaText()
    {
        //PizzasUGUI.text = "Pizzas: " + Pizzeria.Instance.PizzasAmmount.ToString();
        PizzasUGUI.text = "Pizzas in oven outputs: " + Oven.SumOfReadyPizzas().ToString();
        PizzasSoldUGUI.text = "Pizzas sold: " + Pizzeria.Instance.PizzasSold.ToString();
        PizzaPriceUGUI.text = "Pizza price: " + Pizzeria.Instance.PizzaPrice.ToString();
    }

    private void RefreshIngridients()
    {
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
        MoneyT.text = "Money: " + Pizzeria.Instance.Money.ToString();

        if (DayCycle.IsTodayBonusIngredients())
            TodayPromotionT.text = "Today promotion: " + DayCycle.TodayBonus.ToString();
        else
            TodayPromotionT.text = string.Empty;

    }

    public void UpdateCost()
    {
        BuyGoblinT.text = $"cost: {GoblinTransporter.GoblinCost}";
        BuyOvenT.text = $"cost: {Oven.OvenCost}";
    }
    private void CanvasKeeper()
    {
        MainMenuFirst.gameObject.SetActive(menuTracker == 0);
        ShoppingCart.gameObject.SetActive(menuTracker == 1);
        Help.gameObject.SetActive(menuTracker == 2);
    }
}
