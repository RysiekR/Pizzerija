using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenUpgradeTrigger : MonoBehaviour
{
    OvenUpgrades ovenUpgrades;
    private void Start()
    {
        ovenUpgrades = GetComponentInParent<Oven>().Upgrades;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerLogic>() != null)
        {
            ovenUpgrades.ShowUpgradeHUD();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerLogic>() != null)
        {
            ovenUpgrades.HideUpgradeHUD();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerLogic>() != null)
        {
            ovenUpgrades.ManageUpgradeHUD();
        }        
    }

}
