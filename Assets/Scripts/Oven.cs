using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    OvenInput ovenInput;
    OvenOutput ovenOutput;
    public const float BasicBakeTime = 4f;
    public const int BasicBakeAmount = 1;
    public int PizzasAmount { get; private set; } = 0;
    public OvenUpgrades Upgrades { get; private set; }
    public OvenBaking Baking { get; private set; }
    private void Awake()
    {
        ovenInput = GetComponentInChildren<OvenInput>();
        ovenInput.SetOven(this);
        ovenOutput = GetComponentInChildren<OvenOutput>();
        ovenOutput.SetOven(this);
        Upgrades = new OvenUpgrades(this);
        Baking = new OvenBaking(this);
    }

    public void TriggerInput(Collider other)
    {

    }
    public void TriggerOutput(Collider other)
    {

    }

}