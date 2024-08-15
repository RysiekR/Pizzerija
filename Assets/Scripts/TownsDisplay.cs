using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TownsDisplay : MonoBehaviour
{
    public static TownsDisplay Instance;

    [SerializeField] GameObject TownPanel;
    Transform contentTransform;
    List<(GameObject,Town)> towns = new List<(GameObject,Town)>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        contentTransform = transform;
    }
    private void Start()
    {
        CreatePanel("This is TownPanel\n" +
            "it will refresh everyday\n" +
            "you can track your reputation with towns");
        foreach(Town town in Town.TownList)
        {
            towns.Add((CreatePanel(town.TownInfo()),town));
        }
        UpdateTownsDisplay();
    }

    public void UpdateTownsDisplay()
    {
        foreach (var item in towns)
        {
            Town town = item.Item2;
            TextMeshProUGUI text = item.Item1.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.text = town.TownInfo();
        }
    }

    private GameObject CreatePanel(string panelText)
    {
        GameObject panel = Instantiate(TownPanel,contentTransform);

        panel.GetComponentInChildren<TextMeshProUGUI>().text = panelText;

        return panel;
    }
}
