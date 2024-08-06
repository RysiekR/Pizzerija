using System.Linq;
using UnityEngine;

public class PizzeriaShelf : MonoBehaviour
{
    private void Start()
    {
        Pizzeria.Instance.PizzeriaIngredientsShelf = transform;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<GoblinTransporter>() == null)
        {
            return;
        }
        GoblinTransporter goblin = other.GetComponent<GoblinTransporter>();
        if (goblin.ovenToHandle == null)
        {
            goblin.State.Reset();
            if (Oven.ovens.Any(o => o.OvenGoblinManager.GoblinTransportersWithIngredients.Contains(goblin)))
                foreach (var o in Oven.ovens)
                {
                    if (o.OvenGoblinManager.GoblinTransportersWithIngredients.Contains(goblin))
                    {
                        o.OvenGoblinManager.GoblinTransportersWithIngredients.Remove(goblin);
                    }
                }
            return;
        }
        if (goblin.ovenToHandle.OvenGoblinManager.GoblinTransportersWithIngredients.Contains(goblin))
        {
            goblin.GoblinInventory.ManageTransferFromShelfToGoblinInventory();
            if (goblin.State is not DeliverToOven)
                goblin.SetState(new DeliverToOven(goblin));
            return;
        }
        else
        {
            goblin.State.Reset();
            return;
        }

    }
    //Debug.Log($"Gob INV: {other.GetComponent<GoblinTransporter>().GoblinInventory.DoughAmount}, {other.GetComponent<GoblinTransporter>().GoblinInventory.SauceAmount}, {other.GetComponent<GoblinTransporter>().GoblinInventory.ToppingsAmount}");
}



