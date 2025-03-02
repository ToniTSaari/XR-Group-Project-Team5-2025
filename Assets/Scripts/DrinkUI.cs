using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrinkUI : MonoBehaviour
{
    public DrinkChecker drinkChecker;
    public DrinkClasses drinkObject;
    public DrinkClasses drinkRecipes;
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
                drinkUI.text = drinkText + "\n";
            }
        }
        else 
        {
            drinkUI.text = "No drink object found";
        }
        drinkChecker.CheckDrink(drinkObject);
        float drinkScore = scoreCounter.correctnessPercentage;
        drinkUI.text += "Drink correctness: " + drinkScore + "%\n";
        drinkUI.text += "Drink stars: " + scoreCounter.drinkStars + "\n";
    }
    private void Start()
    {
        drinkObject.drinks[0].drinkName += drinkRecipes.drinks[drinkRandomiser.drinkIndex].drinkName;
        displayDrink();
    }

    private void Update()
    {
        displayDrink();
    }
}
