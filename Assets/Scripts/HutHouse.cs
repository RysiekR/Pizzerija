using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HutHouse : MonoBehaviour, IHasACost, IHasStaticList
{
    public static List<HutHouse> HutHouseList { get; private set; } = new List<HutHouse>();
    public static int HutPrice => HutHouseList.Count * 50;
    public static int MaxGoblinsPerHouse => 5;
    int IHasACost.Cost => HutPrice;
    public Transform Inside;
    private Button BuyHutGoblinB;
    [field: SerializeField] public List<GoblinTransporter> HutMembers { get; private set; } = new();

    private void Awake()
    {
        Inside = transform;
    }
    void IHasStaticList.Initiate()
    {
        if (!HutHouseList.Contains(this))
        {
            HutHouseList.Add(this);
        }
        BuyHutGoblinB = transform.Find("Canvas").Find("BuyHutGoblinB").GetComponent<Button>();
        Navigation navigation = BuyHutGoblinB.navigation;
        navigation.mode = Navigation.Mode.None;
        BuyHutGoblinB.navigation = navigation;
        BuyHutGoblinB.onClick.AddListener(SpawnGoblin);
        UpdateButton();
    }
    private void Start()
    {
    }
    private void Update()
    {
        RestingSpot();
    }
    private void OnDestroy()
    {
        if (HutHouseList.Contains(this))
        {
            HutHouseList.Remove(this);
        }
        Destroy(gameObject);
    }

    public void SpawnGoblin()
    {
        if (HutMembers.Count < MaxGoblinsPerHouse)
        {
            if (Pizzeria.Instance.Money >= GoblinTransporter.GoblinCost)
            {
                GoblinTransporter.BuyGoblinTransporter();
                GoblinTransporter goblinTransporter = GoblinTransporter.Goblins[^1];
                HutMembers.Add(goblinTransporter);
                goblinTransporter.RestingSpot = Inside;
            }
        }
        UpdateAllHutsButtons();
    }
    void RestingSpot()
    {
        if (HutMembers.All(g => g.RestingSpot == Inside))
            return;
        foreach (var g in HutMembers)
        {
            g.RestingSpot = Inside;
        }
    }
    void UpdateAllHutsButtons()
    {
        foreach (var h in HutHouseList)
        {
            h.UpdateButton();
        }
    }
    void UpdateButton()
    {
        if (HutMembers.Count >= MaxGoblinsPerHouse)
        {
            BuyHutGoblinB.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Max Members Here!";
            return;
        }
        BuyHutGoblinB.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = $"Get Another Goblin here.\n(cost: {GoblinTransporter.GoblinCost})";
    }
}
