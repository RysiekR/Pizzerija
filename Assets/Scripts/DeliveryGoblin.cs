public class DeliveryGoblin
{
    public string Name { get; private set; }
    public int CarryCappacity { get; private set; }
    public float RestSpeed { get; private set; }
    public float RestTime { get; private set; } = 0f;
    public bool Rested { get; private set; } = true;
    public int PayPerDay { get; private set; }
    public int PizzasAmount {  get; private set; }
    
    public DeliveryGoblin(string name = "Debug", int carryCappacity = 1, float restSpeed = 1f, int payPerDay = 1) 
    {
        Name = name;
        CarryCappacity = carryCappacity;
        RestSpeed = restSpeed;
        PayPerDay = payPerDay;
    }

    public void DeducePayment()
    {
        Pizzeria.Instance.DeductMoney(PayPerDay);
    }

    public void SellPizza()
    {
        Pizzeria.Instance.AddMoney(PizzasAmount * Pizzeria.Instance.PizzaPrice);
        Pizzeria.Instance.IncreasePizzaSold(PizzasAmount);
        PizzasAmount = 0;
        Rested = false;
    }
    public void PickUpPizza(int howMany)
    {
        if (howMany > 0)
        {
            PizzasAmount += howMany;
        }
    }
    public void Rest(float time)
    {
        if (!Rested)
        {
            RestTime += time;
            if (RestTime > RestSpeed)
            {
                Rested = true;
                RestTime = 0;
            }
        }
    }
}