using UnityEngine;

public class Pizzeria : MonoBehaviour
{
    public static Pizzeria Instance { get; private set; }
    public Transform PizzeriaInputSpot;
    public Transform PizzeriaOutputSpot;
    public int Money { get; private set; } = 0;
    public int PizzasAmmount { get; private set; } = 10;
    public int BasePizzaPrice { get; private set; } = 5;
    public int PizzaPrice { get; private set; } = 0;
    public int PizzasSold { get; private set; } = 0;
    public Ingredients Ingredients { get; private set; }


    private float timeToBakeOnePizza = 1f;
    private float bakingTimer = 0f;
    public int OvensAmmount { get; private set; } = 1;


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
        RecalculatePizzaPrice();
    }

    private void FixedUpdate()
    {
        BakePizza();
    }


    public void SellPizza()
    {
        if (PizzasAmmount > 0)
        {
            PizzasAmmount--;
            Money += PizzaPrice;
            if (DayCycle.TodayBonus == TodayBonus.SellPrice) Money += PizzaPrice;
            IncreasePizzaSold(1);
        }
    }


    private void RecalculatePizzaPrice()
    {
        int priceIncrease = 0;
        // Obliczamy logarytm o podstawie 4 z liczby sprzedanych pizz
        if (PizzasSold > 0)
        {
            priceIncrease = (int)Mathf.Log(PizzasSold, 4);
        }
        // Obliczamy now¹ cenê
        PizzaPrice = BasePizzaPrice + priceIncrease;
    }
    private void BakePizza()
    {
        if (bakingTimer > timeToBakeOnePizza)
        {
            for (int i = 0; i < OvensAmmount; i++)
            {
                if (CanBake())
                {
                    PizzasAmmount++;
                    Ingredients.Remove(1, 1, 1);
                }
            }
            bakingTimer = 0f;
        }
        else
        {
            bakingTimer += Time.fixedDeltaTime;
        }
    }

    public bool CanBake()
    {
        return Ingredients.HasAtLeastOneOfEach();
    }


    public void SellPizzaButton()
    {
        SellPizza();
    }
    /// <summary>
    /// true when have that much money
    /// </summary>
    public bool CanPayWithMoney(int howMuch)
    {
        if (howMuch > Money) return false;
        else return true;
    }
    /// <summary>
    /// deducting money only when has that much
    /// </summary>
    public void DeductMoney(int howMuch)
    {
        if (CanPayWithMoney(howMuch))
        {
            Money -= howMuch;
        }
    }
    public void AddMoney(int howMuch)
    {
        Money += howMuch;
    }

    public void RemoveAllPizzas()
    {
        PizzasAmmount = 0;
    }
    public void RemovePizzas(int howMuch)
    {
        PizzasAmmount -= howMuch;
    }
    public void IncreasePizzaSold(int howMuch)
    {
        if (howMuch > 0)
        {
            PizzasSold += howMuch;
        }
        RecalculatePizzaPrice();
    }
}