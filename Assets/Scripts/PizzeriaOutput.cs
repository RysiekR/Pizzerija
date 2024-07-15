using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzeriaOutput : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Pizzeria.Instance.PizzasAmmount > 0)
        {
            if (other.GetComponent<PlayerLogic>() != null)
            {
                other.GetComponent<PlayerLogic>().GrabPizzas(Pizzeria.Instance.PizzasAmmount);
            }
        }
    }
}
