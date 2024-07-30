using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*public class PizzeriaOutput : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HowManyPizzas;
    private void Start()
    {
        Pizzeria.Instance.PizzeriaOutputSpot = transform;
    }
    private void Update()
    {
        RefreshText();
    }

    private void RefreshText()
    {
        //HowManyPizzas.text ="Ready Pizzas: " + Pizzeria.Instance.PizzasAmmount.ToString();
        HowManyPizzas.text ="Pizzas in oven outputs: " + Oven.SumOfReadyPizzas().ToString();
    }
*//*    private void OnTriggerEnter(Collider other)
    {
        if (Pizzeria.Instance.PizzasAmmount > 0)
        {
            if (Distribution.Instance.DeliveryGoblins.Count > 0)
            {

                if (other.GetComponent<PlayerLogic>() != null)
                {
                    other.GetComponent<PlayerLogic>().GrabPizzas(Pizzeria.Instance.PizzasAmmount);
                }
            if (other.GetComponent<GoblinTransporter>() != null)
                {
                    int i = Math.Min(other.GetComponent<GoblinTransporter>().GoblinInventory.HowMuchFreeSpace(),Pizzeria.Instance.PizzasAmmount);
                    other.GetComponent<GoblinTransporter>().GoblinInventory.PickUpPizzaUpToFullInv(Pizzeria.Instance.PizzasAmmount);
                    Pizzeria.Instance.RemovePizzas(i);
                }
            }
        }
    }
*//*}
*/