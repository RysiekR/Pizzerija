public class ShopingCart
{
    public Ingredients ShoppingCartIngredients { get; private set; } = new Ingredients();
    public int LazyBuys = 0;
    public int TotalPrice = 0;

    public void BuyCart()
    {
        if (CanBuyThisCart())
        {
            Pizzeria.Instance.DeductMoney(TotalPrice);
            ModifyCartWithBonus();
            DeliverySystem.Instance.AddDeliveryCarToQueue(ShoppingCartIngredients);
            ClearCart();
        }
    }
    void ModifyCartWithBonus()
    {

        if (DayCycle.TodayBonus == TodayBonus.Dough)
            if (ShoppingCartIngredients.DoughAmount > LazyBuys)
                ShoppingCartIngredients.AddDough(ShoppingCartIngredients.DoughAmount - LazyBuys);
        if (DayCycle.TodayBonus == TodayBonus.Sauce)
            if(ShoppingCartIngredients.SauceAmount > LazyBuys)
            ShoppingCartIngredients.AddSauce(ShoppingCartIngredients.SauceAmount - LazyBuys);
        if (DayCycle.TodayBonus == TodayBonus.Toppings)
            if(ShoppingCartIngredients.ToppingsAmount > LazyBuys)
                ShoppingCartIngredients.AddToppings(ShoppingCartIngredients.ToppingsAmount - LazyBuys);
    }
    public bool CanBuyThisCart()
    {
        return TotalPrice <= Pizzeria.Instance.Money;
    }
    public void AddDough()
    {
        ShoppingCartIngredients.AddDough(1);
        TotalPrice += DeliverySystem.Instance.DoughPrice;
    }
    public void RemoveDough()
    {
        if (ShoppingCartIngredients.DoughAmount > 0)
        {
            ShoppingCartIngredients.Remove(1, 0, 0);
            TotalPrice -= DeliverySystem.Instance.DoughPrice;
        }
    }
    public void AddSauce()
    {
        ShoppingCartIngredients.AddSauce(1);
        TotalPrice += DeliverySystem.Instance.SaucePrice;
    }
    public void RemoveSauce()
    {
        if (ShoppingCartIngredients.SauceAmount > 0)
        {
            ShoppingCartIngredients.Remove(0, 1, 0);
            TotalPrice -= DeliverySystem.Instance.SaucePrice;
        }
    }
    public void AddToppings()
    {
        ShoppingCartIngredients.AddToppings(1);
        TotalPrice += DeliverySystem.Instance.ToppingsPrice;
    }
    public void RemoveToppings()
    {
        if (ShoppingCartIngredients.ToppingsAmount > 0)
        {
            ShoppingCartIngredients.Remove(0, 0, 1);
            TotalPrice -= DeliverySystem.Instance.ToppingsPrice;
        }
    }
    public void AddLazyBuy()
    {
        ShoppingCartIngredients.AddDough(1);
        ShoppingCartIngredients.AddSauce(1);
        ShoppingCartIngredients.AddToppings(1);
        LazyBuys++;
        TotalPrice += DeliverySystem.Instance.LazyBuyPrice;
    }
    public void RemoveLazyBuy()
    {
        if (LazyBuys > 0)
        {
            LazyBuys--;
            ShoppingCartIngredients.Remove(1, 1, 1);
            TotalPrice -= DeliverySystem.Instance.LazyBuyPrice;
        }
    }

    public void ClearCart()
    {
        LazyBuys = 0;
        TotalPrice = 0;
        ShoppingCartIngredients = new Ingredients();
    }
}