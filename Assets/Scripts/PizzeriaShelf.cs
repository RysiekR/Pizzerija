using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzeriaShelf : MonoBehaviour
{
    private void Start()
    {
        Pizzeria.Instance.PizzeriaIngredientsShelf = transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GoblinTransporter>() != null)
        {
            other.GetComponent<GoblinTransporter>().ManageTransferToOven();
            //Debug.Log($"Gob INV: {other.GetComponent<GoblinTransporter>().GoblinInventory.DoughAmount}, {other.GetComponent<GoblinTransporter>().GoblinInventory.SauceAmount}, {other.GetComponent<GoblinTransporter>().GoblinInventory.ToppingsAmount}");
        }
    }

}
