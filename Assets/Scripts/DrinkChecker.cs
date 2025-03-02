using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class DrinkChecker : MonoBehaviour
{
    public DrinkClasses drinkObject;
    public DrinkClasses drinkRecipes;
    public ScoreCounter scoreCounter;

    public void CheckDrink(DrinkClasses drinkObject)
    {
        Debug.Log("Checking if drink name is the same");
        if (drinkObject.drinks[0].drinkName == drinkRecipes.drinks[0].drinkName)
        {
            Debug.Log("Drink name is the same");
            foreach (DrinkClasses.DrinkObject drink in drinkObject.drinks)
            {
                foreach (DrinkClasses.DrinkObject recipe in drinkRecipes.drinks)
                {
                    if (drink.drinkName == recipe.drinkName)
                    {
                        Debug.Log("Drink name is the same");
                        Debug.Log("Checking if ingredients are the same");
                        int drinkIngredientCount = drink.ingredients.Count;
                        for (int i = 0; i < drinkIngredientCount; i++)
                        {
                            int recipeIngredientCount = recipe.ingredients.Count;
                            for (int j = 0; j < recipeIngredientCount; j++)
                            {
                                if (recipe.ingredients[j].ingredientName == drink.ingredients[i].ingredientName)
                                {
                                    Debug.Log("Ingredients are the same");
                                    if (recipe.ingredients[j].ingredientAmount == drink.ingredients[i].ingredientAmount)
                                    {
                                        Debug.Log("Ingredient amounts are the same, 100% right!");
                                        scoreCounter.countScore(100);
                                    }
                                    else if (recipe.ingredients[j].ingredientAmount != drink.ingredients[i].ingredientAmount)
                                    {
                                        float difference = 0;
                                        //Debug.Log("Too little of: " + recipe.ingredients[j].ingredientName);
                                        if (drink.ingredients[i].ingredientAmount < recipe.ingredients[j].ingredientAmount)
                                        {
                                            difference = Mathf.Clamp((drink.ingredients[i].ingredientAmount / recipe.ingredients[j].ingredientAmount) * 100, 0, 100);
                                        }
                                        Debug.Log("Drink is: " + difference + "% correct!");
                                        scoreCounter.countScore(difference);
                                        if(recipe.ingredients[j].hasGarnish)
                                        {
                                            if (drink.ingredients[i].hasGarnish)
                                            {
                                                if (recipe.ingredients[j].garnishName == drink.ingredients[i].garnishName)
                                                {
                                                    Debug.Log("Garnish is correct");
                                                    scoreCounter.garnishScore(true);
                                                }
                                                else
                                                {
                                                    Debug.Log("Garnish is not correct");
                                                    scoreCounter.garnishScore(false);
                                                }
                                            }
                                            else
                                            {
                                                Debug.Log("Garnish is missing");
                                                scoreCounter.garnishScore(false);
                                            }
                                        }
                                        else
                                        {
                                            if (drink.ingredients[i].hasGarnish)
                                            {
                                                Debug.Log("Garnish is not needed");
                                                scoreCounter.garnishScore(false);
                                            }
                                        }
                                    }
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
