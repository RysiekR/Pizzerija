using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzeriaIngredientInput : MonoBehaviour
{
    private void Start()
    {
        Pizzeria.Instance.PizzeriaInputSpot = transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerLogic>() != null)
        {
            other.GetComponent<PlayerLogic>().DropIngredients(Pizzeria.Instance.Ingredients);
        }
        if (other.GetComponent<GoblinTransporter>() != null)
        {
            other.GetComponent<GoblinTransporter>().GoblinInventory.PassIngredients().TransferAllTo(Pizzeria.Instance.Ingredients);
        }
    }
}
