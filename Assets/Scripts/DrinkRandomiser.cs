using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkRandomiser : MonoBehaviour
{
    public DrinkClasses drinkRecipes;
    public DrinkClasses.DrinkObject drink;
    // Start is called before the first frame update
    void Start()
    {
        drink = pickDrink();
    }

    public DrinkClasses.DrinkObject pickDrink() 
    {
        int drinks = drinkRecipes.drinks.Count;
        int randomDrink = Random.Range(0, drinks);
        return drinkRecipes.drinks[randomDrink];
    }
}
