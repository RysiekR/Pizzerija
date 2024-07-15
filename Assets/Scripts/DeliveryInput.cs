using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryInput : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerLogic>() != null)
        {
            Distribution.Instance.ReceivePizzas(other.GetComponent<PlayerLogic>().DropPizzas());
        }
    }
}
