using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketDisplay : MonoBehaviour
{
    [SerializeField] GameObject OrderPanelPrefab;

    List<GameObject> orders = new List<GameObject>();
    Transform contentTransform;
    TextMeshProUGUI firstPanelText;


    private void Start()
    {
        Market.Instance.MarketDisplay = this;
        contentTransform = transform.Find("Canvas").Find("Scroll View").Find("Viewport").Find("Content");
        /*if (contentTransform != null)
        {
            Debug.Log("Sukcesssss");
        }*/
        firstPanelText = contentTransform.Find("Panel").Find("Text").GetComponent<TextMeshProUGUI>();
        RefreshText();
    }
    public void MakeOrdersList()
    {

        foreach (var order in orders)
        {
            Destroy(order);
        }
        orders.Clear();
        if (Market.Instance.MarketOrders.Count > 0)
        {
            foreach (var o in Market.Instance.MarketOrders)
            {
                var listMember = Instantiate(OrderPanelPrefab, contentTransform);
                listMember.GetComponentInChildren<TextMeshProUGUI>().text = o.OrderText();

                Button button = listMember.GetComponentInChildren<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(o.FullFillOrder);
                button.onClick.AddListener(MakeOrdersList);

                orders.Add(listMember);
            }
            //Debug.Log("market display refreshed with orders");
        }
        else
        {
            var listMember = Instantiate(OrderPanelPrefab, contentTransform);
            listMember.GetComponentInChildren<TextMeshProUGUI>().text = "No Orders Ready\n\n" +
                "Refreshing checks \n" +
                "for new orders";

            Button button = listMember.GetComponentInChildren<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(MakeOrdersList);
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Refresh";

            orders.Add(listMember);
        }
    }
    public void RefreshText()
    {
        firstPanelText.text = $"Welcome in your market\n" +
        $"you will sell your pizzas here\n" +
        $"Pizzas ready: {Market.Instance.PizzaAmmount}";
        if (Market.Instance.MarketOrders.Count == orders.Count)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                orders[i].GetComponentInChildren<TextMeshProUGUI>().text = Market.Instance.MarketOrders[i].OrderText();
            }
        }
        else
        {
            MakeOrdersList();
        }
    }
}