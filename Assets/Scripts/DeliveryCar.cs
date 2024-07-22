using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeliveryCar : MonoBehaviour
{
    TextMeshProUGUI CarBackUGUI;
    public Ingredients CarIngredients;
    private void Awake()
    {
        CarIngredients = new Ingredients(0,0,0);
        AddYourselfToList();
    }
    public void SetCarIngredients(Ingredients ingredients)
    {
        CarIngredients = ingredients;
    }
    private void Start()
    {
        CarBackUGUI = transform.Find("CarBackCanvas").Find("CarBackTMPT").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        RefreshInfo();
    }

    public void RefreshInfo()
    {
        CarBackUGUI.text = $"Dough: {CarIngredients.DoughAmount}\nSauce: {CarIngredients.SauceAmount}\nToppings: {CarIngredients.ToppingsAmount}";
    }

    public bool HasAnyIngredients()
    {
        return CarIngredients.HasAny();
    }

    private void AddYourselfToList()
    {
        if(!DeliverySystem.deliveryCars.Contains(this))
        {
            DeliverySystem.deliveryCars.Add(this);
        }
    }

}
