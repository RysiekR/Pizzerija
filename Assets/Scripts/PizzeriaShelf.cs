using UnityEngine;

public class PizzeriaShelf : MonoBehaviour
{
    private void Start()
    {
        Pizzeria.Instance.PizzeriaIngredientsShelf = transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GoblinTransporter>() == null)
        {
            return;
        }
        if (other.GetComponent<GoblinTransporter>().ovenToHandle == null)
        {
            return;
        }
        if (other.GetComponent<GoblinTransporter>().ovenToHandle.GoblinWithIngredients == other.GetComponent<GoblinTransporter>())
        {
            other.GetComponent<GoblinTransporter>().ManageTransferToOven();
            if(other.GetComponent<GoblinTransporter>().State is not DeliverToOven)
            {
                other.GetComponent<GoblinTransporter>().SetState(new DeliverToOven(other.GetComponent<GoblinTransporter>()));
            }
        }
        else
        {
            other.GetComponent<GoblinTransporter>().ovenToHandle = null;
            other.GetComponent<GoblinTransporter>().TargetOvenInput = null;
        }
    }
    //Debug.Log($"Gob INV: {other.GetComponent<GoblinTransporter>().GoblinInventory.DoughAmount}, {other.GetComponent<GoblinTransporter>().GoblinInventory.SauceAmount}, {other.GetComponent<GoblinTransporter>().GoblinInventory.ToppingsAmount}");
}



