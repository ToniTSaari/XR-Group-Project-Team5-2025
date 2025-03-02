using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeUI : MonoBehaviour
{
    public DrinkClasses drinkRecipes;
    public TextMeshProUGUI recipeText;
    public DrinkRandomiser drinkRandomiser;
    public void displayRecipe()
    {
        drinkRecipes.drinks.Clear();
        drinkRecipes.drinks.Add(drinkRandomiser.drink);
        string drinkRecipe = drinkRecipes.drinks[0].drinkName + "\n";
        drinkRecipe += "Ingredients:\n";
        for (int i = 0; i < drinkRecipes.drinks[0].ingredients.Count; i++)
        {
            drinkRecipe += $"{drinkRecipes.drinks[0].ingredients[i].ingredientName} - {drinkRecipes.drinks[0].ingredients[i].ingredientAmount} cl\n";
        }
        recipeText.text = drinkRecipe;
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
