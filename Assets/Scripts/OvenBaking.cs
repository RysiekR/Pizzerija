using System.Collections;
using UnityEngine;

public class OvenBaking
{
    Oven oven;
    public const float BasicBakeTime = 4f;
    public const int BasicBakeAmount = 1;
    public float BakingTime { get => BasicBakeTime * oven.Upgrades.TimeMultiplier; }
    public int BakeAmount { get => (BasicBakeAmount + oven.Upgrades.AmountAdder) * (int)oven.Upgrades.AmountMultiplier; }
    public bool IsBaking { get; private set; } = false;
    Ingredients bakingIngredients = new Ingredients();

    public OvenBaking(Oven oven)
    {
        this.oven = oven;
    }

    public IEnumerator GetBake()
    {
        if (CanBake())
        {
            IsBaking = true;
            TransferIngredientsToBake();
            yield return new WaitForSeconds(BakingTime);
            for (int i = 0; i < BakeAmount; i++)
            {
                if (bakingIngredients.HasAtLeastOneOfEach())
                {
                    oven.Ingredients.Remove(1, 1, 1);
                    oven.AddPizzas(1);
                }
            }
            HUDScript.Instance.UpdatePizzasToSell();
            IsBaking = false;
            oven.coroutine = null;
        }
    }

    public bool CanBake()
    {
        return !IsBaking && oven.Ingredients.HasAtLeastOneOfEach();
    }

    public bool HaveEnoughForNextBake()
    {
        int j = 0;
        Ingredients temp = new Ingredients(oven.Ingredients);
        for (int i = 0; i < BakeAmount; i++)
        {
            if (temp.HasAtLeastOneOfEach())
            {
                j++;
                temp.Remove(1, 1, 1);
            }
        }
        return j >= BakeAmount;
    }
    public bool HaveEnoughForTwoBakes()
    {
        int j = 0;
        Ingredients temp = new Ingredients(oven.Ingredients);
        for (int i = 0; i < BakeAmount * 2; i++)
        {
            if (temp.HasAtLeastOneOfEach())
            {
                j++;
                temp.Remove(1, 1, 1);
            }
        }
        return j >= BakeAmount * 2;
    }
    private void TransferIngredientsToBake()
    {
        for (int i = 0; i < BakeAmount; i++)
        {
            if (oven.Ingredients.HasAtLeastOneOfEach())
            {
                oven.Ingredients.Remove(1, 1, 1);
                bakingIngredients.AddDough(1);
                bakingIngredients.AddSauce(1);
                bakingIngredients.AddToppings(1);
            }
        }
    }
}
