using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketInput : MonoBehaviour
{
    private void Start()
    {
        Market.Instance.MarketInputSpot = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerLogic>() != null)
        {
            Market.Instance.ReceivePizzas(other.GetComponent<PlayerLogic>().DropPizzas());
        }
        if (other.GetComponent<GoblinTransporter>() != null)
        {
            Market.Instance.ReceivePizzas(other.GetComponent<GoblinTransporter>().GoblinInventory.PassPizzas());
        }
    }

}
