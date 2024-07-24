using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    public static DeliverySystem Instance;
    [SerializeField] GameObject DeliveryCarPrefab;
    public ShopingCart ShopingCart;
    private Queue<Ingredients> IngredientsDeliveryQueue = new Queue<Ingredients>();
    public bool TruckWaiting { get; private set; } = false;
    public bool AutomaticDeliveriesON { get; private set; } = false;
    public float ETANextCar { get; private set; } = 5f;
    private Coroutine sendingTrucksCoroutine;
    public int DoughPrice { get; private set; } = 3;
    public int SaucePrice { get; private set; } = 1;
    public int ToppingsPrice { get; private set; } = 2;
    public int LazyBuyPrice { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        ShopingCart = new ShopingCart();
        CalculateLazyBuyPrice();
    }
    private void Update()
    {
        ETACounddown();
    }

    private void CalculateLazyBuyPrice()
    {
        LazyBuyPrice = DoughPrice + SaucePrice + ToppingsPrice + 3;

    }

    public void AddDeliveryCarToQueue(Ingredients ingredientsToAdd)
    {
        IngredientsDeliveryQueue.Enqueue(ingredientsToAdd);
    }

    public void SendDeliveryCar()
    {
        if (IngredientsDeliveryQueue.Count > 0 && !TruckWaiting)
        {
            TruckWaiting = true;
            GameObject newCar = Instantiate(DeliveryCarPrefab);
            newCar.GetComponent<DeliveryCar>().SendWithIngredients(IngredientsDeliveryQueue.Dequeue());
        }
    }
    /// <summary>
    /// change TruckWaiting to false
    /// </summary>
    public void IngredientsPickedUp()
    {
        TruckWaiting = false;
    }

    public void AutomaticSendOfTrucks()
    {
        if (sendingTrucksCoroutine == null)
        {
            AutomaticDeliveriesON = true;
            sendingTrucksCoroutine = StartCoroutine(SendAfterTime(5));
        }
    }
    public void StopAutomaticSendOfTrucks()
    {
        if (sendingTrucksCoroutine != null)
        {
            AutomaticDeliveriesON = false;
            StopCoroutine(sendingTrucksCoroutine);
            sendingTrucksCoroutine = null;
        }
    }
    private void ETACounddown()
    {
        if (IngredientsDeliveryQueue.Count > 0)
        {
            if (sendingTrucksCoroutine != null)
            {
                if (!TruckWaiting)
                {
                    ETANextCar -= Time.deltaTime;
                }
                else
                {
                    ETANextCar = 5f;
                }
            }
            else
            {
                ETANextCar = 5f;
            }
        }
        else
        {
            ETANextCar = 5f;
        }
    }
    IEnumerator SendAfterTime(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            SendDeliveryCar();
        }
    }
    public bool HasDeliveriesInQueue()
    {
        return IngredientsDeliveryQueue.Count > 0;
    }
}