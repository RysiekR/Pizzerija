using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenInput : MonoBehaviour
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
            GoblinTransporter goblin = other.GetComponent<GoblinTransporter>();
            if (goblin.ovenToHandle == oven)
            {
                oven.OvenGoblinManager.RemoveIncomingGoblinWithIngredientsFromList(goblin);
                goblin.GoblinInventory.PassIngredients().TransferAllTo(oven.Ingredients);
            }
        }
        oven.OvenVisuals.UpdateDisplay();
    }
}
