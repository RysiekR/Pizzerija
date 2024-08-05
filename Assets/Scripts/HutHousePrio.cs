using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HutHousePrio : MonoBehaviour
{
    private HutHouse ThisHutHouse;
    private Canvas PrioC;

    Button Index0Up;
    Button Index0Down;
    Button Index1Up;
    Button Index1Down;
    Button Index2Up;
    Button Index2Down;

    TextMeshProUGUI Index0T;
    TextMeshProUGUI Index1T;
    TextMeshProUGUI Index2T;

    Button OldestB;
    Button RandomB;
    Button NewestB;
    TextMeshProUGUI ModeT;
    private void Start()
    {
        ThisHutHouse = GetComponentInParent<HutHouse>();
        PrioC = transform.Find("PrioC").GetComponent<Canvas>();

        Index0Up = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("GoblinBackgroundI").Find("Index0UpB").GetComponent<Button>();
        Index0Down = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("GoblinBackgroundI").Find("Index0DownB").GetComponent<Button>();
        Index1Up = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("GoblinBackgroundI").Find("Index1UpB").GetComponent<Button>();
        Index1Down = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("GoblinBackgroundI").Find("Index1DownB").GetComponent<Button>();
        Index2Up = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("GoblinBackgroundI").Find("Index2UpB").GetComponent<Button>();
        Index2Down = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("GoblinBackgroundI").Find("Index2DownB").GetComponent<Button>();

        Index0T = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("GoblinBackgroundI").Find("Index0T").GetComponent<TextMeshProUGUI>();
        Index1T = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("GoblinBackgroundI").Find("Index1T").GetComponent<TextMeshProUGUI>();
        Index2T = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("GoblinBackgroundI").Find("Index2T").GetComponent<TextMeshProUGUI>();

        Index0Up.onClick.AddListener(() => UpPrio(0));
        Index0Down.onClick.AddListener(() => DownPrio(0));
        Index1Up.onClick.AddListener(() => UpPrio(1));
        Index1Down.onClick.AddListener(() => DownPrio(1));
        Index2Up.onClick.AddListener(() => UpPrio(2));
        Index2Down.onClick.AddListener(() => DownPrio(2));

        OldestB = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("OvenPrio").Find("OldestB").GetComponent<Button>();
        RandomB = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("OvenPrio").Find("RandomB").GetComponent<Button>();
        NewestB = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("OvenPrio").Find("NewestB").GetComponent<Button>();
        ModeT = PrioC.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("OvenPrio").Find("ModeT").GetComponent<TextMeshProUGUI>();

        OldestB.onClick.AddListener(() => { ThisHutHouse.ChangeOvenPrio(0); RefreshOvenPrioText(); });
        RandomB.onClick.AddListener(() => { ThisHutHouse.ChangeOvenPrio(1); RefreshOvenPrioText(); });
        NewestB.onClick.AddListener(() => { ThisHutHouse.ChangeOvenPrio(2); RefreshOvenPrioText(); });

        RefreshOvenPrioText();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerLogic>() != null)
        {
            RefreshPrioText();
            PrioC.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerLogic>() != null)
        {
            PrioC.gameObject.SetActive(false);
        }
    }
    void UpPrio(int index)
    {
        foreach (var g in ThisHutHouse.HutMembers)
        {
            g.State.UpPrio(index);
        }
        RefreshPrioText();
    }
    void DownPrio(int index)
    {
        foreach (var g in ThisHutHouse.HutMembers)
        {
            g.State.DownPrio(index);
        }
        RefreshPrioText();
    }

    public void RefreshPrioText()
    {
        Index0T.text = string.Empty;
        Index1T.text = string.Empty;
        Index2T.text = string.Empty;
        if (ThisHutHouse.HutMembers.Count > 0)
        {
            for (int i = 0; i < ThisHutHouse.HutMembers.Count; i++)
            {
                Index0T.text += ThisHutHouse.HutMembers[i].JobPriority[0].ToString() + " ";
                Index1T.text += ThisHutHouse.HutMembers[i].JobPriority[1].ToString() + " ";
                Index2T.text += ThisHutHouse.HutMembers[i].JobPriority[2].ToString() + " ";
            }
        }
    }
    public void RefreshOvenPrioText()
    {
        ModeT.text = "Will walk to ";
        if (ThisHutHouse.HutMembers.Count > 0)
        {
            for (int i = 0; i < ThisHutHouse.HutMembers.Count; i++)
            {
                if(ThisHutHouse.HutMembers[i].OvenPriority == 0)
                {
                    ModeT.text += "Oldest ";
                }
                else if (ThisHutHouse.HutMembers[i].OvenPriority == 1)
                {
                    ModeT.text += "Newest ";
                }
                else
                {
                    ModeT.text += "Random ";
                }
            }
        }
        ModeT.text += "first.";
    }
}
