using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInput : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerLogic>() != null)
        {
            other.GetComponent<PlayerLogic>().DropIngredients(Pizzeria.Instance.Ingredients);
        }
    }
}
