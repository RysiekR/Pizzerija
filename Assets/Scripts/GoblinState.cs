using UnityEngine;

[System.Serializable]
public abstract class GoblinState
{
    [SerializeField] protected int num = 0;
    protected GoblinTransporter Goblin;
    protected GoblinState(GoblinTransporter goblin)
    {
        Goblin = goblin;
    }
    public abstract void UpdateState();
    public abstract void ExecuteBehaviour();
}
public class DeliverToOven : GoblinState
{
    public DeliverToOven(GoblinTransporter goblin) : base(goblin) { num = 1; }
    public override void UpdateState()
    {
        if (!Goblin.GoblinInventory.HasIngredients())
        {
            //Goblin.ovenToHandle.SetIncomingGoblinWithIngredients(null);
            if (Goblin.ovenToHandle != null)
            {
                Goblin.ovenToHandle.RemoveIncomingGoblinWithIngredientsFromList(Goblin);
            }
/*            Goblin.TargetOvenInput = null;
            Goblin.ovenToHandle = null;
*/            Goblin.SetState(new WalkingToRestState(Goblin));
            return;
        }
        if (Goblin.ovenToHandle == null)
        {
            Goblin.SetState(new WalkingToRestState(Goblin));
            return;
        }
    }
    public override void ExecuteBehaviour()
    {
        Goblin.NavMeshAgentGoblin.SetDestination(Goblin.TargetOvenInput.position);
    }

}
public class WalkingToPickUpIngredientsToOven : GoblinState
{
    public WalkingToPickUpIngredientsToOven(GoblinTransporter goblin) : base(goblin) { num = 2; }
    public override void UpdateState()
    {
        if (Goblin.ovenToHandle != null)
        {
            //if (Goblin.ovenToHandle.GoblinWithIngredients != Goblin)// && !Goblin.GoblinInventory.HasIngredients())
            if (!Goblin.ovenToHandle.GoblinTransportersWithIngredients.Contains(Goblin))
            {
                Goblin.ovenToHandle = null;
                Goblin.TargetOvenInput = null;
                Goblin.SetState(new WalkingToRestState(Goblin));
                return;
            }
            if (!Goblin.ovenToHandle.PizzeriaHasIngredientsForThisOven())
            {
                Goblin.ovenToHandle.RemoveIncomingGoblinWithIngredientsFromList(Goblin);
                Goblin.SetState(new WalkingToRestState(Goblin));
                return;
            }
        }
        if (Goblin.GoblinInventory.HasIngredients())
        {
            Goblin.SetState(new DeliverToOven(Goblin));
            return;
        }
    }
    public override void ExecuteBehaviour()
    {
        Goblin.NavMeshAgentGoblin.SetDestination(Pizzeria.Instance.PizzeriaIngredientsShelf.position);
    }
}
public class WalkingToRestState : GoblinState
{
    public WalkingToRestState(GoblinTransporter goblin) : base(goblin) { num = 3; }
    public override void UpdateState()
    {
        //priorytet
        if (Goblin.GoblinInventory.HasPizza())
        {
            Goblin.SetState(new WalkToDropOffPizzaState(Goblin));
            return;
        }
        if (Goblin.GoblinInventory.HasIngredients())
        {
            Goblin.SetState(new DeliveringIngredientsToPizzeriaState(Goblin));
            return;
        }
        var ovensNeedingIngredients = Oven.OvensThatNeedIngredients();
        if (ovensNeedingIngredients.Count > 0)
        {
            Oven temp = ovensNeedingIngredients[0];
            //if (temp.GoblinWithIngredients != null)
            if (temp.GoblinTransportersWithIngredients.Count >= ((int)temp.Upgrades.Level / 2) + 1)
            {
                Debug.Log("2 gob same job from WalkingToRest");
                return;
            }
            Goblin.ovenToHandle = temp;
            Goblin.TargetOvenInput = temp.ovenInput.transform;
            temp.SetIncomingGoblinWithIngredients(Goblin);
            Goblin.SetState(new WalkingToPickUpIngredientsToOven(Goblin));
            return;
        }
        if (Oven.OvensWithPizzaReady().Count > 0)
        {
            Goblin.TargetOvenOutput = Oven.OvensWithPizzaReady()[0].ovenOutput.transform;
            Oven.OvensWithPizzaReady()[0].AddIncomingGoblinForPizza(Goblin);
            Goblin.SetState(new WalkForPizzaToOven(Goblin));
            return;
        }
        if (DeliverySystem.Instance.TruckWaiting && DeliverySystem.Instance.PickUpSpot != null)
        {
            Goblin.SetState(new WalkingForIngredientsToTruckState(Goblin));
            return;
        }
    }
    public override void ExecuteBehaviour()
    {
        Goblin.NavMeshAgentGoblin.SetDestination(Goblin.RestingSpot.position);
    }
}
public class WalkForPizzaToOven : GoblinState
{
    public WalkForPizzaToOven(GoblinTransporter goblin) : base(goblin) { num = 4; }
    public override void UpdateState()
    {
        if (Goblin.GoblinInventory.HasPizza())
        {
            Goblin.SetState(new WalkToDropOffPizzaState(Goblin));
            Goblin.TargetOvenOutput = null;
            return;
        }
    }
    public override void ExecuteBehaviour()
    {
        Goblin.NavMeshAgentGoblin.SetDestination(Goblin.TargetOvenOutput.position);
    }
}
public class WaitingState : GoblinState
{
    public WaitingState(GoblinTransporter goblin) : base(goblin) { num = 5; }
    public override void UpdateState()
    {
        //cleanup
        if (Goblin.ovenToHandle != null)
        {
            //if (Goblin.ovenToHandle.GoblinWithIngredients == Goblin)
            if (Goblin.ovenToHandle.GoblinTransportersWithIngredients.Contains(Goblin))
            {
                //Goblin.ovenToHandle.SetIncomingGoblinWithIngredients(null);
                Goblin.ovenToHandle.RemoveIncomingGoblinWithIngredientsFromList(Goblin);
            }
            Goblin.ovenToHandle = null;
            Goblin.TargetOvenInput = null;
        }
        if (Goblin.GoblinInventory.HasAnything())
        {
            Goblin.SetState(new WalkingToRestState(Goblin));
        }
        var ovensNeedingIngredients = Oven.OvensThatNeedIngredients();
        if (ovensNeedingIngredients.Count > 0)
        {
            Oven temp = ovensNeedingIngredients[0];
            //if (temp.GoblinWithIngredients != null)
            if (temp.GoblinTransportersWithIngredients.Count >= ((int)temp.Upgrades.Level / 2) + 1)

            {
                Debug.Log("2 gob same job from Waiting");
                return;
            }
            Goblin.ovenToHandle = temp;
            Goblin.TargetOvenInput = temp.ovenInput.transform;
            temp.SetIncomingGoblinWithIngredients(Goblin);
            Goblin.SetState(new WalkingToPickUpIngredientsToOven(Goblin));
            return;
        }
        if (Oven.OvensWithPizzaReady().Count > 0)
        {
            Goblin.TargetOvenOutput = Oven.OvensWithPizzaReady()[0].ovenOutput.transform;
            Oven.OvensWithPizzaReady()[0].AddIncomingGoblinForPizza(Goblin);
            Goblin.SetState(new WalkForPizzaToOven(Goblin));
            return;
        }

        if (DeliverySystem.Instance.TruckWaiting && DeliverySystem.Instance.PickUpSpot != null)
        {
            Goblin.SetState(new WalkingForIngredientsToTruckState(Goblin));
            return;
        }

    }
    public override void ExecuteBehaviour()
    {
        //walk in a circle
        Goblin.NavMeshAgentGoblin.Move(Goblin.transform.right * Time.deltaTime);
    }
}
public class DeliveringIngredientsToPizzeriaState : GoblinState
{
    public DeliveringIngredientsToPizzeriaState(GoblinTransporter goblin) : base(goblin) { num = 6; }
    public override void UpdateState()
    {
        if (!Goblin.GoblinInventory.HasIngredients())
        {
            Goblin.SetState(new WalkingToRestState(Goblin));
            return;
        }
    }
    public override void ExecuteBehaviour()
    {
        Goblin.NavMeshAgentGoblin.SetDestination(Pizzeria.Instance.PizzeriaInputSpot.position);
    }
}
public class WalkingForIngredientsToTruckState : GoblinState
{
    public WalkingForIngredientsToTruckState(GoblinTransporter goblin) : base(goblin) { num = 7; }
    public override void UpdateState()
    {
        if (Goblin.GoblinInventory.HasIngredients())
        {
            Goblin.SetState(new DeliveringIngredientsToPizzeriaState(Goblin));
            return;
        }
        if (!DeliverySystem.Instance.TruckWaiting)
        {
            Goblin.SetState(new WalkingToRestState(Goblin));
            return;
        }
    }
    public override void ExecuteBehaviour()
    {
        if (DeliverySystem.Instance.PickUpSpot != null)
        {
            Goblin.NavMeshAgentGoblin.SetDestination(DeliverySystem.Instance.PickUpSpot.position);
        }
    }
}
public class WalkToDropOffPizzaState : GoblinState
{
    public WalkToDropOffPizzaState(GoblinTransporter goblin) : base(goblin) { num = 8; }
    public override void UpdateState()
    {
        if (!Goblin.GoblinInventory.HasPizza())
        {
            Goblin.SetState(new WalkingToRestState(Goblin));
            return;
        }
    }
    public override void ExecuteBehaviour()
    {
        Goblin.NavMeshAgentGoblin.SetDestination(Distribution.Instance.DistributionInputSpot.position);
    }
}