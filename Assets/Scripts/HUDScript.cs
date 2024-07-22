using System.Linq;
using TMPro;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    TextMeshProUGUI money;
    TextMeshProUGUI pizzasToSell;
    TextMeshProUGUI goblinsWaiting;
    TextMeshProUGUI noIngredients;
    private void Start()
    {
        money = transform.Find("MoneyTMPT").GetComponent<TextMeshProUGUI>();
        pizzasToSell = transform.Find("PizzasToSellTMPT").GetComponent<TextMeshProUGUI>();
        goblinsWaiting = transform.Find("GoblinsAreWaitingTMPT").GetComponent<TextMeshProUGUI>();
        noIngredients = transform.Find("PizzerijaIngrTMPT").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        UpdateMoney();
        UpdatePizzasToSell();
        UpdateGoblinsWaiting();
        UpdateNoIngredients();
    }

    void UpdateMoney()
    {
        money.text = "Money: " + Pizzeria.Instance.Money.ToString();
    }

    void UpdatePizzasToSell()
    {
        if (Pizzeria.Instance.PizzasAmmount > 0)
        {
            pizzasToSell.text = "Pizzas ready to sell:" + Pizzeria.Instance.PizzasAmmount.ToString();
        }
        else
        {
            if (Pizzeria.Instance.CanBake())
            {
                pizzasToSell.text = "Pizzas are baking, just wait";
            }
            else
            {
                if (Pizzeria.Instance.Money >= 6)
                {
                    pizzasToSell.text = "buy ingredients, remember they cost (press tab)";
                }
                else
                {
                    pizzasToSell.text = "you dont have money to buy ingredients, you propably lost the game";
                }
            }
        }
    }
    void UpdateGoblinsWaiting()
    {
        if (Distribution.Instance.DeliveryGoblins.Count <= 0)
        {
            goblinsWaiting.transform.gameObject.SetActive(false);
            return;
        }
        if (Distribution.Instance.DeliveryGoblins.All(g => !g.Rested))
        {
            goblinsWaiting.transform.gameObject.SetActive(false);
            return;
        }
        goblinsWaiting.transform.gameObject.SetActive(true);
    }

    void UpdateNoIngredients()
    {
        noIngredients.transform.gameObject.SetActive(!Pizzeria.Instance.CanBake());
    }
}