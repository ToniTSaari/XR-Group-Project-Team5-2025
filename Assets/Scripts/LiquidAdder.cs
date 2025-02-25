using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LiquidAdder : MonoBehaviour
{
    public DrinkClasses drinkObject;
    public string drinkOrder;
    public string ingredientUnit = "cl";
    void pourIngredient(string ingredientName, float ingredientAmount)
    {
        Debug.Log("Pouring " + ingredientAmount + ingredientUnit + " of " + ingredientName);

        DrinkClasses.DrinkObject currentDrink = drinkObject.drinks.Find(drink => drink.drinkName == drinkOrder);
        if(currentDrink == null) 
        {
            DrinkClasses.DrinkObject newDrink = new DrinkClasses.DrinkObject
            {
                drinkName = drinkOrder,
                ingredients = new List<DrinkClasses.DrinkIngredients>()
            };
            drinkObject.drinks.Add(newDrink);
            Debug.Log("Begin new order: " + drinkOrder);
        }
        DrinkClasses.DrinkIngredients currentIngredient = currentDrink.ingredients.Find(ingredient => ingredient.ingredientName == ingredientName);
        if (currentIngredient != null)
        {
            currentIngredient.ingredientAmount += ingredientAmount;
            Debug.Log("Added " + ingredientAmount + ingredientUnit + " of " + ingredientName + " to " + drinkOrder);
        }
        else
        {
            DrinkClasses.DrinkIngredients newIngredient = new DrinkClasses.DrinkIngredients
            {
                ingredientName = ingredientName,
                ingredientAmount = ingredientAmount,
                milliliters = false
            };
            currentDrink.ingredients.Add(newIngredient);
            Debug.Log("Added new ingredient " + ingredientName + " to " + drinkOrder);
        }
        Debug.Log("Current drink is: " + currentDrink.drinkName + " with " + currentDrink.ingredients[0].ingredientAmount + " of " + currentDrink.ingredients[0].ingredientName);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current[Key.NumpadPlus].wasPressedThisFrame && !Keyboard.current[Key.LeftShift].IsPressed())
        {
            pourIngredient("Beer", 1.0f);
        }
        if (Keyboard.current[Key.LeftShift].IsPressed() && Keyboard.current[Key.NumpadPlus].wasPressedThisFrame)
        {
            pourIngredient("Beer", 0.1f);
        }
    }
}
