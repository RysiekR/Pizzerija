using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenOutput : MonoBehaviour
{
    Oven oven;

    public void SetOven(Oven oven)
    {
        this.oven = oven;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GoblinTransporter>() != null)
        {
            if (other.GetComponent<GoblinTransporter>().TargetOvenOutput != null)
            {
                if (other.GetComponent<GoblinTransporter>().TargetOvenOutput == oven.ovenOutput.transform)
                {
                    int i = Math.Min(other.GetComponent<GoblinTransporter>().GoblinInventory.HowMuchFreeSpace(), oven.PizzasAmount);
                    other.GetComponent<GoblinTransporter>().GoblinInventory.PickUpPizzaUpToFullInv(oven.PizzasAmount);
                    oven.RemovePizzas(i);
                    HUDScript.Instance.UpdatePizzasToSell();
                    oven.OvenGoblinManager.GoblinForPizza.Remove(other.GetComponent<GoblinTransporter>());
                }
            }
        }
        oven.OvenVisuals.UpdateDisplay();
    }
}
