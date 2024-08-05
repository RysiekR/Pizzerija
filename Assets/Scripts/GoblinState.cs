using System.Linq;
using UnityEngine;

public abstract class GoblinState
{
    protected GoblinTransporter Goblin;
    protected GoblinState(GoblinTransporter goblin)
    {

        Goblin = goblin;
    }
    protected abstract void UpdateStateSpecific();
    public abstract void ExecuteBehaviour();
    public void Reset()
    {
        Goblin.ovenToHandle = null;
        Goblin.TargetOvenInput = null;
        Goblin.TargetOvenOutput = null;
        Goblin.SetState(new WalkingToRestState(Goblin));
    }
    private void ChangeJobPriority(int jobIndex, int plusMinus)
    {
        Goblin.JobPriority[jobIndex] = Mathf.Clamp(Goblin.JobPriority[jobIndex] + plusMinus, 1, 5);
    }

    public void UpPrio(int jobIndex)
    {
        ChangeJobPriority(jobIndex, 1);
    }
    public void DownPrio(int jobIndex)
    {
        ChangeJobPriority(jobIndex, -1);
    }
    /// <summary>
    /// 0 - oldest, 1 - newest, 2 - random
    /// </summary>
    public void ChangeOvenPrio(int prio)
    {
        switch (prio)
        {
            case 0: Goblin.OvenPriority = 0; break;
            case 1: Goblin.OvenPriority = 1; break;
            case 2: Goblin.OvenPriority = 2; break;
            default: Goblin.OvenPriority = 1; break;
        }
    }
    public void UpdateState()
    {
        UpdateStateSpecific();
        HandleAnimation();
    }
    private void HandleAnimation()
    {
        Goblin.Animator.SetBool("IsCarrying", Goblin.GoblinInventory.HasAnything());

        if (Goblin.NavMeshAgentGoblin.velocity != Vector3.zero)
            Goblin.Animator.SetBool("IsMoving", true);
        else
            Goblin.Animator.SetBool("IsMoving", false);
    }
}

public class DeliverToOven : GoblinState
{
    public DeliverToOven(GoblinTransporter goblin) : base(goblin) { }
    protected override void UpdateStateSpecific()
    {
        if (!Goblin.GoblinInventory.HasIngredients())
        {
            if (Goblin.ovenToHandle != null)
            {
                Goblin.ovenToHandle.RemoveIncomingGoblinWithIngredientsFromList(Goblin);
            }
            Goblin.SetState(new WalkingToRestState(Goblin));
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
    public WalkingToPickUpIngredientsToOven(GoblinTransporter goblin) : base(goblin) { }
    protected override void UpdateStateSpecific()
    {
        if (Goblin.ovenToHandle != null)
        {
            if (!Goblin.ovenToHandle.GoblinTransportersWithIngredients.Contains(Goblin))
            {
                Reset();
                return;
            }
            if (!Goblin.ovenToHandle.PizzeriaHasIngredientsForThisOven())
            {
                Goblin.ovenToHandle.RemoveIncomingGoblinWithIngredientsFromList(Goblin);
                Goblin.SetState(new WalkingToRestState(Goblin));
                return;
            }
        }
        if (Goblin.ovenToHandle == null)
        {
            Goblin.State.Reset();
            return;
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
    public WalkingToRestState(GoblinTransporter goblin) : base(goblin) { }
    protected override void UpdateStateSpecific()
    {
        if (CleanUp())
            return;

        //priorytet
        for (int i = 5; i > 0; i--)
        {
            for (int j = 0; j < Goblin.JobPriority.Length; j++)
            {
                if (Goblin.JobPriority[j] == i)
                {
                    switch (j)
                    {
                        case 0:
                            if (GoToPickUpIngredientsFromShelf())
                                return;
                            break;
                        case 1:
                            if (GoToPickUpPizza())
                                return;
                            break;
                        case 2:
                            if (GoToPickUpIngredientsFromTruck())
                                return;
                            break;
                    }
                }
            }
        }

    }
    public override void ExecuteBehaviour()
    {
        Goblin.NavMeshAgentGoblin.SetDestination(Goblin.RestingSpot.position);
        /*if (Goblin.NavMeshAgentGoblin.remainingDistance < 5f)
            //walk in a circle
            Goblin.NavMeshAgentGoblin.Move(Goblin.transform.right * 3 * Time.deltaTime);
*/
        //Goblin.NavMeshAgentGoblin.Warp(Goblin.transform.position);

    }
    private bool GoToPickUpIngredientsFromTruck()
    {
        if (DeliverySystem.Instance.TruckWaiting && DeliverySystem.Instance.PickUpSpot != null)
        {
            Goblin.SetState(new WalkingForIngredientsToTruckState(Goblin));
            return true;
        }
        else
            return false;
    }
    private bool GoToPickUpPizza()
    {
        var ovensWithPizza = Oven.OvensWithPizzaReady();
        if (ovensWithPizza.Count > 0)
        {
            Goblin.TargetOvenOutput = ovensWithPizza[0].ovenOutput.transform;
            ovensWithPizza[0].AddIncomingGoblinForPizza(Goblin);
            Goblin.SetState(new WalkForPizzaToOven(Goblin));
            return true;
        }
        else
            return false;
    }
    private bool GoToPickUpIngredientsFromShelf()
    {
        int index;
        var ovensNeedingIngredients = Oven.OvensThatNeedIngredients();
        if (ovensNeedingIngredients.Count > 0)
        {
            switch (Goblin.OvenPriority)
            {
                case 0: index = 0; break;
                case 1: index = ovensNeedingIngredients.Count - 1; break;
                case 2: index = Random.Range(0, ovensNeedingIngredients.Count); break;
            }
            Oven temp = ovensNeedingIngredients[0];
            Goblin.ovenToHandle = temp;
            Goblin.TargetOvenInput = temp.ovenInput.transform;
            temp.SetIncomingGoblinWithIngredients(Goblin);
            Goblin.SetState(new WalkingToPickUpIngredientsToOven(Goblin));
            return true;
        }
        else
            return false;
    }
    private bool CleanUp()
    {
        //remaining targets
        if (Goblin.ovenToHandle != null)
        {
            if (!Goblin.ovenToHandle.GoblinTransportersWithIngredients.Contains(Goblin))
            {
                Reset();
            }
            if (!Pizzeria.Instance.Ingredients.HasAny())
            {
                Goblin.ovenToHandle.RemoveIncomingGoblinWithIngredientsFromList(Goblin);
            }
        }
        if (!Oven.ovens.Any(o => o.GoblinForPizza.Contains(Goblin)))
        {
            Reset();
        }
        if (Goblin.ovenToHandle == null)
        {
            if (Oven.ovens.Any(o => o.GoblinTransportersWithIngredients.Contains(Goblin)))
                foreach (var o in Oven.ovens)
                {
                    if (o.GoblinTransportersWithIngredients.Contains(Goblin))
                        o.GoblinTransportersWithIngredients.Remove(Goblin);
                }
        }

        //remaining Inventory
        if (Goblin.GoblinInventory.HasPizza())
        {
            Goblin.SetState(new WalkToDropOffPizzaState(Goblin));
            return true;
        }
        if (Goblin.GoblinInventory.HasIngredients())
        {
            Goblin.SetState(new DeliveringIngredientsToPizzeriaState(Goblin));
            return true;
        }
        return false;
    }
}
public class WalkForPizzaToOven : GoblinState
{
    public WalkForPizzaToOven(GoblinTransporter goblin) : base(goblin) { }
    protected override void UpdateStateSpecific()
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
public class DeliveringIngredientsToPizzeriaState : GoblinState
{
    public DeliveringIngredientsToPizzeriaState(GoblinTransporter goblin) : base(goblin) { }
    protected override void UpdateStateSpecific()
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
    protected override void UpdateStateSpecific()
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
    protected override void UpdateStateSpecific()
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