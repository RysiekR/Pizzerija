using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] GameObject IngredientsVisual;
    [SerializeField] GameObject PizzaBoxxesVisual;
    public Ingredients IngredientsOnPlayer;
    public int PizzasAmount = 0;

    private void Awake()
    {
        IngredientsOnPlayer = new Ingredients();
    }


    private void Update()
    {
        IngredientsVisual.SetActive(IngredientsOnPlayer.HasAny());
        PizzaBoxxesVisual.SetActive(PizzasAmount > 0);
    }

    public void GrabIngredients(Ingredients ingredientsToGrab)
    {
        ingredientsToGrab.TransferAllTo(IngredientsOnPlayer);
    }

    public void DropIngredients(Ingredients ingredientsToPut)
    {
        IngredientsOnPlayer.TransferAllTo(ingredientsToPut);
    }

/*    public void GrabPizzas(int howMany)
    {
        PizzasAmount += howMany;
        Pizzeria.Instance.RemoveAllPizzas();
    }
*/
    public int DropPizzas()
    {
        if (PizzasAmount > 0)
        {
            int temp = PizzasAmount;
            PizzasAmount = 0;
            return temp;
        }
        return 0;
    }
}
