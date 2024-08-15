using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistributionInput : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DeliveryHUD;
    private void Start()
    {
        Distribution.Instance.DistributionInputSpot = transform;
    }
    private void Update()
    {
        RefreshText();
    }

    private void RefreshText()
    {
        DeliveryHUD.text = $"You have {Distribution.Instance.DeliveryGoblins.Count} distribution goblins. \n" +
            $"They rest for: {Distribution.Instance.RestingTimes()}. \n" +
            $"They have {Distribution.Instance.PizzaAmmount} pizzas to deliver.";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerLogic>() != null)
        {
            Distribution.Instance.ReceivePizzas(other.GetComponent<PlayerLogic>().DropPizzas());
        }
        if (other.GetComponent<GoblinTransporter>() != null)
        {
            Distribution.Instance.ReceivePizzas(other.GetComponent<GoblinTransporter>().GoblinInventory.PassPizzas());
        }
    }

}
