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
        string currentRecipe = this.drinkRecipe.drinks[0].drinkName + "\n\n";
        currentRecipe += "Ingredients:\n";
        for (int i = 0; i < this.drinkRecipe.drinks[0].ingredients.Count; i++)
        {
            currentRecipe += $"{this.drinkRecipe.drinks[0].ingredients[i].ingredientName} - {this.drinkRecipe.drinks[0].ingredients[i].ingredientAmount} cl\n";
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
