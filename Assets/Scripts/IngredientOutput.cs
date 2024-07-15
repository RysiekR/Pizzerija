using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientOutput : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (DeliveryTruck.Instance.HasAnyIngredients())
        {
            if (other.GetComponent<PlayerLogic>() != null)
            {
                other.GetComponent<PlayerLogic>().GrabIngredients(DeliveryTruck.Instance.Ingredients);
            }
        }
    }

}
