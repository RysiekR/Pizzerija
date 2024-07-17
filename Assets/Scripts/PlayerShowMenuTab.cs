using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShowMenuTab : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) ShowMenu();
    }
    public void ShowMenu()
    {
        PersistentUI.Instance.ShowMenu();
    }

}
