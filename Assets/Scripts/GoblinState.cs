using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class GoblinState
{
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
    public DeliverToOven(GoblinTransporter goblin) : base(goblin) { }
    public override void UpdateState()
    {
        if (!Goblin.GoblinInventory.HasIngredients())
        {
            Goblin.SetState(new WalkingToRestState(Goblin));
            Goblin.TargetOvenInput = null;
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
    public WalkingToPickUpIngredientsToOven(GoblinTransporter goblin) : base(goblin) { }
    public override void UpdateState()
    {
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
    public WalkingToRestState(GoblinTransporter goblin) : base(goblin) { }
    public override void UpdateState()
    {
        //priorytet
        if (Oven.OvensThatNeedIngredients().Count > 0)
        {
            Goblin.ovenToHandle = Oven.OvensThatNeedIngredients()[0];
            Goblin.TargetOvenInput = Oven.OvensThatNeedIngredients()[0].ovenInput.transform;
            Oven.OvensThatNeedIngredients()[0].SetIncomingGoblinWithIngredients(Goblin);
            Goblin.SetState(new WalkingToPickUpIngredientsToOven(Goblin));
            return;
        }
        /*if (Pizzeria.Instance.PizzasAmmount > 0 && Distribution.Instance.DeliveryGoblins.Count > 0)
        {
            Goblin.SetState(new WalkingToPickUpPizzaState(Goblin));
            return;
        }*/
        if(Oven.OvensWithPizzaReady().Count > 0 )
        {
            Goblin.TargetOvenOutput = Oven.OvensWithPizzaReady()[0].ovenOutput.transform;
            Oven.OvensWithPizzaReady()[0].SetIncomingGoblinForPizza(Goblin);
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
    public WalkForPizzaToOven(GoblinTransporter goblin) : base(goblin) { }
    public override void UpdateState()
    {
        if(Goblin.GoblinInventory.HasPizza())
        {
            Goblin.SetState(new WalkToDropOffPizzaState(Goblin));
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
    public WaitingState(GoblinTransporter goblin) : base(goblin) { }
    public override void UpdateState()
    {
        if(Oven.OvensThatNeedIngredients().Count>0)
        {
            Goblin.ovenToHandle = Oven.OvensThatNeedIngredients()[0];
            Goblin.TargetOvenInput = Oven.OvensThatNeedIngredients()[0].transform;
            Oven.OvensThatNeedIngredients()[0].SetIncomingGoblinWithIngredients(Goblin);
            Goblin.SetState(new WalkingToPickUpIngredientsToOven(Goblin));
            return;
        }
        /*        if (Pizzeria.Instance.PizzasAmmount > 0 && Distribution.Instance.DeliveryGoblins.Count > 0)
                {
                    Goblin.SetState(new WalkingToPickUpPizzaState(Goblin));
                    return;
                }
        */
        if (Oven.OvensWithPizzaReady().Count > 0)
        {
            Goblin.TargetOvenOutput = Oven.OvensWithPizzaReady()[0].ovenOutput.transform;
            Oven.OvensWithPizzaReady()[0].SetIncomingGoblinForPizza(Goblin);
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
    public DeliveringIngredientsToPizzeriaState(GoblinTransporter goblin) : base(goblin) { }
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
    public WalkingForIngredientsToTruckState(GoblinTransporter goblin) : base(goblin) { }
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
    public WalkToDropOffPizzaState(GoblinTransporter goblin) : base(goblin) { }
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
/*public class WalkingToPickUpPizzaState : GoblinState
{
    public WalkingToPickUpPizzaState(GoblinTransporter goblin) : base(goblin) { }
    public override void UpdateState()
    {
        if (Goblin.GoblinInventory.HasPizza())
        {
            Goblin.SetState(new WalkToDropOffPizzaState(Goblin));
            return;
        }
*//*        if (Pizzeria.Instance.PizzasAmmount <= 0)
        {
            Goblin.SetState(new WalkingToRestState(Goblin));
            return;
        }
*//*    }
    public override void ExecuteBehaviour()
    {
        Goblin.NavMeshAgentGoblin.SetDestination(Pizzeria.Instance.PizzeriaOutputSpot.position);
    }
}*/