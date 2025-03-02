using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeUI : MonoBehaviour
{
    public DrinkClasses drinkRecipe;
    public TextMeshProUGUI recipeText;
    public DrinkRandomiser drinkRandomiser;

    public void displayRecipe()
    {
        string currentRecipe = drinkRecipe.drinks[drinkRandomiser.drinkIndex].drinkName + "\n\n";
        currentRecipe += "Ingredients:\n";
        for (int i = 0; i < drinkRecipe.drinks[drinkRandomiser.drinkIndex].ingredients.Count; i++)
        {
            currentRecipe += $"{drinkRecipe.drinks[drinkRandomiser.drinkIndex].ingredients[i].ingredientName} - {drinkRecipe.drinks[drinkRandomiser.drinkIndex].ingredients[i].ingredientAmount} cl\n";
        }
        recipeText.text = currentRecipe;
    }
    // Start is called before the first frame update
    void Start()
    {
        displayRecipe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
