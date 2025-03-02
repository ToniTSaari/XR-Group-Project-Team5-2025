using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkRandomiser : MonoBehaviour
{
    public DrinkClasses drinkRecipes;
    public DrinkClasses.DrinkObject drink;
    public int drinkIndex;
    // Start is called before the first frame update
    void Start()
    {
        drinkIndex = pickDrink();
        drink = drinkRecipes.drinks[drinkIndex];
    }

    public int pickDrink() 
    {
        int drinks = drinkRecipes.drinks.Count;
        int randomDrink = Random.Range(0, drinks);
        return randomDrink;
    }
}
