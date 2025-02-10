using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkClasses : MonoBehaviour
{
    [System.Serializable]
    public class DrinkIngredients
    {
        public string ingredientName;
        public float ingredientAmount;
        public string ingredientUnit;
    }
    [System.Serializable]
    public class DrinkRecipes
    {
        public string drinkName;
        public List<DrinkIngredients> ingredients = new List<DrinkIngredients>();
    }
    public List<DrinkRecipes> drinkRecipes = new List<DrinkRecipes>();
}
