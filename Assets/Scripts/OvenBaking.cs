using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenBaking
{
    Oven oven;
    public OvenBaking(Oven oven)
    {
        this.oven = oven;
    }

    public void Bake()
    {

    }
    public bool CanBake()
    {
        return true;
    }

    public bool HaveEnoughForNextBake()
    {
        return false;
    }
}
