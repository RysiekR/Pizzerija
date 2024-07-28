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
        oven.TriggerInput(other);
    }
}
