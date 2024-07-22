using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopingCart
{
    public Ingredients ShoppingCart { get; private set; } = new Ingredients();
    public int TotalPrice = 0;
    
    public void BuyCart()
    {
        if(CanBuyThisCart())
        {
            //removemone
            //transfer ingredients
            //reset everything
        }
    }
    public bool CanBuyThisCart()
    {
        return TotalPrice <= Pizzeria.Instance.Money;
    }
    public void AddDough()
    {

    }
    public void RemoveDough() 
    {
    
    }
    public void AddSauce()
    {

    }
    public void RemoveSauce()
    {

    }
    public void AddToppings()
    {

    }
    public void RemoveToppings()
    {

    }

    private void ClearCart()
    {

    }
}