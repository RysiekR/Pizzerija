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
                if (!CheckForIngredients())
                {
                    DeliverySystem.Instance.DeliveryCarIngredientsEmpty();
                }
            }
            if (other.GetComponent<GoblinTransporter>() != null)
            {
                other.GetComponent<GoblinTransporter>().GoblinInventory.PickUpUpToFullInvFrom(myCar.CarIngredients);
                if (!CheckForIngredients())
                {
                    DeliverySystem.Instance.DeliveryCarIngredientsEmpty();
                }
            }
        }
    }

    private bool CheckForIngredients()
    {
        return myCar.HasAnyIngredients();
    }
}
