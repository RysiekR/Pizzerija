using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PizzeriaOutput : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HowManyPizzas;

    private void Update()
    {
        RefreshText();
    }

    private void RefreshText()
    {
        HowManyPizzas.text ="Ready Pizzas: " + Pizzeria.Instance.PizzasAmmount.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Pizzeria.Instance.PizzasAmmount > 0)
        {
            if (Distribution.Instance.DeliveryGoblins.Count > 0)
            {

                if (other.GetComponent<PlayerLogic>() != null)
                {
                    other.GetComponent<PlayerLogic>().GrabPizzas(Pizzeria.Instance.PizzasAmmount);
                }
            }
        }
    }
}
