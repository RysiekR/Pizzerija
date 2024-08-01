using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingSpotTrigger : MonoBehaviour
{
    public static Transform RestingSpot;
    private void Awake()
    {
        RestingSpot = transform;
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GoblinTransporter>() != null && other.GetComponent<GoblinTransporter>().State is WalkingToRestState)
        {
            other.GetComponent<GoblinTransporter>().SetState(new WaitingState(other.GetComponent<GoblinTransporter>()));
        }
    }*/
}
