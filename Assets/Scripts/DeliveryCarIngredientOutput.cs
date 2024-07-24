using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCarIngredientOutput : MonoBehaviour
{
    DeliveryCar myCar;

    private void Start()
    {
        myCar = GetComponent<DeliveryCar>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (myCar.HasAnyIngredients())
        {
            if (other.GetComponent<PlayerLogic>() != null)
            {
                other.GetComponent<PlayerLogic>().GrabIngredients(myCar.CarIngredients);
                DeliverySystem.Instance.IngredientsPickedUp();

            }
            if (other.GetComponent<GoblinDeliveryToPizzeria>() != null)
            {
                other.GetComponent<GoblinDeliveryToPizzeria>().GoblinInventory.PickUpUpToFullInvFrom(myCar.CarIngredients);
                DeliverySystem.Instance.IngredientsPickedUp();
            }
        }
    }

}
