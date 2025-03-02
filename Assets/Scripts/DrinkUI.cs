using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrinkUI : MonoBehaviour
{
    public DrinkChecker drinkChecker;
    public DrinkClasses drinkObject;
    public ScoreCounter scoreCounter;
    public DrinkRandomiser drinkRandomiser;
    public TextMeshProUGUI drinkUI;

    public void displayDrink() 
    {
        if(drinkObject != null && drinkObject.drinks.Count != 0 && drinkObject.drinks != null) 
        {
            DrinkClasses.DrinkObject drink = drinkObject.drinks[0];
            if (drink.ingredients.Count == 0)
            {
                drinkUI.text = "Empty glass of " + drink.drinkName;
            }
            else
            {
                string drinkText = drink.drinkName + "\n\n";
                if (drink.ingredients.Count > 0)
                {
                    drinkText += "Ingredients:\n";
                    for (int i = 0; i < drink.ingredients.Count; i++)
                    {
                        drinkText += $"{drink.ingredients[i].ingredientName} - {drink.ingredients[i].ingredientAmount} cl\n";
                    }
                }
                drinkUI.text = drinkText;
            }
        }
        else 
        {
            drinkUI.text = "No drink object found";
        }
    }
    private void Start()
    {
        displayDrink();
    }

    private void Update()
    {
        displayDrink();
    }
}
