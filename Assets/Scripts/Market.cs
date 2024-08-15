using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    public static Market Instance;
    public Transform MarketInputSpot;
    public int PizzaAmmount { get; private set; } = 0;
    public List<MarketOrder> MarketOrders;
    public int OrderCap => GetOrderCap();
    public MarketDisplay MarketDisplay { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MarketOrders = new List<MarketOrder>();
            new Town();
            new Town();
            new Town();
        }

    }

    private void Update()
    {
        ManageOrders();
    }
    private void FailOrder(MarketOrder order)
    {
        order.Fail();
        MarketOrders.Remove(order);
    }

    public void ReceivePizzas(int amount)
    {
        if (amount > 0)
        {
            PizzaAmmount += amount;
        }
    }
    private void AddMarkedOrder()
    {
        MarketOrders.Add(new MarketOrder());
        //Debug.Log("New Order Added");
    }
    public void CreateOrders()
    {
        while (MarketOrders.Count < OrderCap)
        {
            AddMarkedOrder();
        }
        MarketDisplay.MakeOrdersList();
        /*if (MarketOrders.Count != 0)
            Debug.Log("Orders created");
        else
            Debug.Log("something went wrong");*/
    }
    private int GetOrderCap()
    {
        int cap = 1;
        foreach (var t in Town.TownList)
        {
            if ((int)t.Reputation >= 3)
                cap++;
            if ((int)t.Reputation >= 5)
                cap++;
            if ((int)t.Reputation >= 6)
                cap++;
        }
        return cap;
    }

    private void ManageOrders()
    {
        // reversed for loop
        for (int i = MarketOrders.Count - 1; i >= 0; i--)
        {
            MarketOrders[i].TimeToFullfill -= Time.deltaTime;
            MarketOrders[i].TimeForExtra -= Time.deltaTime;
            if (MarketOrders[i].TimeToFullfill < 0)
            {
                FailOrder(MarketOrders[i]);
            }
        }
        MarketDisplay.RefreshText();
    }
    /// <summary>
    /// use only in marketOrder.FulFillOrder
    /// </summary>
    public void RemovePizzas(int i)
    {
        PizzaAmmount -= i;
    }
}

public class MarketOrder
{
    public Town WhereFrom;
    public float TimeToFullfill;
    public float TimeForExtra;
    public int Amount;
    public MarketOrder()
    {
        WhereFrom = Town.TownList[Random.Range(0, Town.TownList.Count)];
        TimeToFullfill = Random.Range(((int)WhereFrom.Reputation + 1), ((int)WhereFrom.Reputation + 1) * 40);
        TimeForExtra = TimeToFullfill / (Random.Range(1, 10));

        Amount = Random.Range(6, 16) + WhereFrom.ReputationAmountAdder();

    }

    /// <summary>
    /// use this to sell pizzas stored in market maybe use in a button
    /// </summary>
    public void FullFillOrder()
    {
        if (Market.Instance.PizzaAmmount >= Amount)
        {
            if (DayCycle.TodayBonus == TodayBonus.SellPrice)
                Pizzeria.Instance.AddMoney(Amount * Pizzeria.Instance.PizzaPrice);

            int FinalMoney = 0;
            FinalMoney += Amount * Pizzeria.Instance.PizzaPrice;

            if (TimeForExtra > 0)
            {
                FinalMoney += CalculateExtraCash();
                WhereFrom.ReputationChangeUp();
            }

            Pizzeria.Instance.AddMoney(FinalMoney);
            WhereFrom.ReputationChangeUp();
            Market.Instance.RemovePizzas(Amount);
            Pizzeria.Instance.IncreasePizzaSold(Amount);
            Market.Instance.MarketOrders.Remove(this);
        }
    }
    public void Fail()
    {
        WhereFrom.ReputationChangeDown();
    }
    private int CalculateExtraCash()
    {
        return Mathf.Max(Amount * (WhereFrom.ReputationAmountAdder()), 0);
    }
    public string OrderText()
    {
        string bonusTime = $"(bonus: {TimeForExtra:F1})";
        string bonusCash = string.Empty;
        if (DayCycle.TodayBonus == TodayBonus.SellPrice)
            bonusCash += $"+ {Amount * Pizzeria.Instance.PizzaPrice}";
        if (TimeForExtra < 0)
            bonusTime = string.Empty;
        else
            bonusCash += $"+ {CalculateExtraCash()}";

        return $"Town: {WhereFrom.Name}\n" +
            $"Rep: {WhereFrom.Reputation}\n" +
            $"{Amount} Pizzas\n" +
            $"time: {TimeToFullfill:F1} {bonusTime}\n" +
            $"profit: {Amount * Pizzeria.Instance.PizzaPrice} {bonusCash}";
    }
}

public class Town
{
    public static int RepFactorMax = 4;
    public static List<Town> TownList = new List<Town>();
    public string Name;
    public Reputation Reputation;
    public int ReputationFactor { get; private set; } = 2;

    public Town()
    {
        Name = RandomizeName();
        Reputation = Reputation.Bad;
        TownList.Add(this);
    }


    public void ReputationChangeUp()
    {
        ReputationFactor++;
        if (ReputationFactor >= 10)
        {
            if (Reputation != Reputation.Partner)
            {
                Reputation++;
                ReputationFactor = 0;
            }
            else
                ReputationFactor = RepFactorMax;
        }
    }
    public void ReputationChangeDown()
    {
        ReputationFactor--;
        if (ReputationFactor < 0)
        {
            if (Reputation != Reputation.Tragic)
            {
                Reputation--;
                ReputationFactor = 9;
            }
            else
                ReputationFactor = 0;
        }
    }
    public string TownInfo()
    {
        return $"Town: {Name}\n" +
            $"Reputation: {Reputation.ToString()}\n" +
            $"Reputation growth: {ReputationFactor} / {RepFactorMax}";
    }

    public static string RandomizeName()
    {
        string[] FirstName = { "qwe", "asd", "zxc" };
        string[] SecondName = { "ewq", "dsa", "cxz" };
        string[] ThirdName = { "qaz", "wsx", "edc" };
        string[] ForthName = { "zaq", "xsw", "cde" };

        return FirstName[Random.Range(0, FirstName.Length)] +
            SecondName[Random.Range(0, SecondName.Length)] + " " +
            ThirdName[Random.Range(0, ThirdName.Length)] +
            ForthName[Random.Range(0, ForthName.Length)];
    }
    public int ReputationAmountAdder()
    {
        return Reputation switch
        {
            Reputation.Tragic => -5,
            Reputation.VeryBad => -3,
            Reputation.Bad => -1,
            Reputation.Normal => 0,
            Reputation.Good => 1,
            Reputation.VeryGood => 3,
            Reputation.Partner => 5,
            _ => 0,
        };
    }
}

public enum Reputation
{
    Tragic,
    VeryBad,
    Bad,
    Normal,
    Good,
    VeryGood,
    Partner
}