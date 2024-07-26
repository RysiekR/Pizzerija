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
public class WalkingToRestState : GoblinState
{
    public WalkingToRestState(GoblinTransporter goblin) : base(goblin) { }
    public override void UpdateState()
    {
        //priorytet
        if (Pizzeria.Instance.PizzasAmmount > 0 && Distribution.Instance.DeliveryGoblins.Count > 0)
        {
            Goblin.SetState(new WalkingToPickUpPizzaState(Goblin));
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
public class WaitingState : GoblinState
{
    public WaitingState(GoblinTransporter goblin) : base(goblin) { }
    public override void UpdateState()
    {
        if (Pizzeria.Instance.PizzasAmmount > 0 && Distribution.Instance.DeliveryGoblins.Count > 0)
        {
            Goblin.SetState(new WalkingToPickUpPizzaState(Goblin));
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
        Goblin.NavMeshAgentGoblin.SetDestination(Goblin.transform.right);
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
        if(Goblin.GoblinInventory.HasIngredients())
        {
            Goblin.SetState(new DeliveringIngredientsToPizzeriaState(Goblin));
            return;
        }
        if(!DeliverySystem.Instance.TruckWaiting)
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
public class WalkingToPickUpPizzaState : GoblinState
{
    public WalkingToPickUpPizzaState(GoblinTransporter goblin) : base(goblin) { }
    public override void UpdateState()
    {
        if (Goblin.GoblinInventory.HasPizza())
        {
            Goblin.SetState(new WalkToDropOffPizzaState(Goblin));
            return;
        }
        if (Pizzeria.Instance.PizzasAmmount <= 0)
        {
            Goblin.SetState(new WalkingToRestState(Goblin));
            return;
        }
    }
    public override void ExecuteBehaviour()
    {
        Goblin.NavMeshAgentGoblin.SetDestination(Pizzeria.Instance.PizzeriaOutputSpot.position);
    }
}