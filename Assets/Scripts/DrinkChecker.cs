using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class DrinkChecker : MonoBehaviour
{
    public DrinkClasses drinkObject;
    public DrinkClasses drinkRecipes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.Space].IsPressed())
        {
            Debug.Log("Checking if drink name is the same");
            if (drinkObject.drinks[0].drinkName == drinkRecipes.drinks[0].drinkName) 
            {
                Debug.Log("Drink name is the same");
                foreach(DrinkClasses.DrinkObject drink in drinkObject.drinks) 
                {
                    foreach (DrinkClasses.DrinkObject recipe in drinkRecipes.drinks) 
                    {
                        if(drink.drinkName == recipe.drinkName) 
                        {
                            Debug.Log("Drink name is the same");
                            Debug.Log("Checking if ingredients are the same");
                            int drinkIngredientCount = drink.ingredients.Count;
                            for(int i = 0; i < drinkIngredientCount; i++) 
                            {
                                int recipeIngredientCount = recipe.ingredients.Count;
                                for(int j = 0; j < drinkIngredientCount; j++) 
                                {
                                    if (recipe.ingredients[j].ingredientName == drink.ingredients[i].ingredientName)
                                    {
                                        Debug.Log("Ingredients are the same");
                                    }
                                    else
                                    {
                                        Debug.Log("Ingredients are not the same");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Drink name is not the same");
            }
        }
        
    }
}
