using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Ingredients
{
    public int DoughAmount { get; private set; }
    public int SauceAmount { get; private set; }
    public int ToppingsAmount { get; private set; }

    public Ingredients(int doughAmount = 0, int sauceAmount = 0, int toppingsAmount = 0)
    {
        DoughAmount = doughAmount;
        SauceAmount = sauceAmount;
        ToppingsAmount = toppingsAmount;
    }
    /// <summary>
    /// use in baking oven for a copy to check
    /// </summary>
    public Ingredients(Ingredients i)
    {
        DoughAmount = i.DoughAmount;
        SauceAmount= i.SauceAmount;
        ToppingsAmount = i.ToppingsAmount;
    }

    public bool HasAtLeastOneOfEach()
    {
        return (DoughAmount > 0 && SauceAmount > 0 && ToppingsAmount > 0);
    }
    public bool HasAny()
    {
        return (DoughAmount > 0 || SauceAmount > 0 || ToppingsAmount > 0);
    }

    public void Remove(int doughAmount, int sauceAmount, int toppingsAmount)
    {
        if (doughAmount <= DoughAmount && sauceAmount <= SauceAmount && toppingsAmount <= ToppingsAmount)
        {
            DoughAmount-=doughAmount;
            SauceAmount-=sauceAmount;
            ToppingsAmount-=toppingsAmount;
        }
    }

    public void AddDough(int i)
    {
        if (i > 0)
        {
            for (int j = 0; j < i; j++)
            {
                DoughAmount++;
            }
        }
    }
    public void AddSauce(int i)
    {
        if (i > 0)
        {
            for (int j = 0; j < i; j++)
            {
                SauceAmount++;
            }
        }
    }

    public void AddToppings(int i)
    {
        if(i > 0)
        {
            for(int j = 0;j < i; j++)
            {
                ToppingsAmount++;
            }
        }
    }

    public void TransferAllTo(Ingredients ingridientsWhereToTransfer)
    {
        ingridientsWhereToTransfer.AddDough(DoughAmount);
        DoughAmount = 0;
        ingridientsWhereToTransfer.AddSauce(SauceAmount);
        SauceAmount = 0;
        ingridientsWhereToTransfer.AddToppings(ToppingsAmount);
        ToppingsAmount = 0;
    }

}