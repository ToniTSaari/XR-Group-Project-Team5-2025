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
        public bool milliliters;
    }
    [System.Serializable]
    public class DrinkObject
    {
        public string drinkName;
        public List<DrinkIngredients> ingredients = new List<DrinkIngredients>();
        public DrinkObject()
        {
            ingredients = new List<DrinkIngredients>();
        }
    }
    public List<DrinkObject> drinks = new List<DrinkObject>();
}
