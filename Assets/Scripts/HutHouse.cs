using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HutHouse : MonoBehaviour, IHasACost, IHasStaticList
{
    public static List<HutHouse> HutHouseList { get; private set; } = new List<HutHouse>();
    public static int HutPrice => HutHouseList.Count * 50;
    public static int MaxGoblinsPerHouse => 3;
    int IHasACost.Cost => HutPrice;
    public Transform Inside;
    private Button BuyHutGoblinB;
    [field: SerializeField] public List<GoblinTransporter> HutMembers { get; private set; } = new();
    GameObject Vis1;
    GameObject Vis2;
    GameObject Vis3;
    private void Awake()
    {
        Vis1 = transform.Find("Houses_Level1").gameObject;
        Vis2 = transform.Find("Houses_Level2").gameObject;
        Vis3 = transform.Find("Houses_Level3").gameObject;
        Inside = transform;
        Vis1.SetActive(false);
        Vis2.SetActive(true);
        Vis3.SetActive(false);
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
        BuyHutGoblinB.onClick.AddListener(UpdateVis);
        BuyHutGoblinB.onClick.AddListener(RestingSpot);
        BuyHutGoblinB.onClick.AddListener(NavMeshRebuilder.Instance.ResetNavMeshSurface);
        UpdateButtonAndVis();
        Vis1.SetActive(true);
        Vis2.SetActive(false);
        RestingSpot();
        NavMeshRebuilder.Instance.ResetNavMeshSurface();
    }
    private void Start()
    {
    }
    private void Update()
    {
    }
    private void OnDestroy()
    {
        if (HutMembers.Count > 0)
        {
            foreach (var member in HutMembers)
            {
                Pizzeria.Instance.AddMoney(GoblinTransporter.GoblinCost - 10);
                Destroy(member);
            }
        }
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
        RestingSpot();
        GetComponentInChildren<HutHousePrio>().RefreshPrioText();
        GetComponentInChildren<HutHousePrio>().RefreshOvenPrioText();
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
            h.UpdateButtonAndVis();
        }
    }
    void UpdateButtonAndVis()
    {
        if (HutMembers.Count >= MaxGoblinsPerHouse)
        {
            BuyHutGoblinB.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Max Members Here!";
            return;
        }
        BuyHutGoblinB.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = $"Get Another Goblin here.\n(cost: {GoblinTransporter.GoblinCost})";

        UpdateVis();
    }
    void UpdateVis()
    {
        Vis1.SetActive(false);
        Vis2.SetActive(false);
        Vis3.SetActive(false);

        if (HutMembers.Count >= MaxGoblinsPerHouse)
        {
            Vis3.SetActive(true);
            BuyHutGoblinB.onClick.RemoveListener(NavMeshRebuilder.Instance.ResetNavMeshSurface);
        }
        else if (HutMembers.Count <= 1)
        {
            Vis1.SetActive(true);
        }
        else
        {
            Vis2.SetActive(true);
        }

    }
    public void ChangeOvenPrio(int prio)
    {
        if (HutMembers.Count <= 0)
            return;
        foreach (var g in HutMembers)
        {
            g.State.ChangeOvenPrio(prio);
        }
    }
}
