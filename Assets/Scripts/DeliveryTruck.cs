using TMPro;
using UnityEngine;

public class DeliveryTruck : MonoBehaviour
{
    public static DeliveryTruck Instance;
    [SerializeField] private TextMeshProUGUI CarBackUGUI;
    public Ingredients Ingredients;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Ingredients = new Ingredients(2, 2, 2);
    }

    private void Update()
    {
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        CarBackUGUI.text = $"Dough: {Ingredients.DoughAmount}\nSauce: {Ingredients.SauceAmount}\nToppings: {Ingredients.ToppingsAmount}";
    }

    public bool HasAnyIngredients()
    {
        return Ingredients.HasAny();
    }

}
