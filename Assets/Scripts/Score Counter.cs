using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public float correctnessPercentage = 0.0f;
    public int drinkStars = 0;
    //public float scoreMultiplier = 100.0f;

    public void countScore(float drinkScore) 
    {
        if(drinkScore < 0)
        {
            correctnessPercentage = 0;
            drinkStars = 0;
        }
        else if (drinkScore > 100)
        {
            correctnessPercentage = 100;
        }
        else if (drinkScore != 0) 
        {
            correctnessPercentage = drinkScore;
            drinkStars = (int)math.round(correctnessPercentage / 20);
            Debug.Log("Score is now: " + correctnessPercentage);
        }
        else 
        {
            correctnessPercentage = 0;
            drinkStars = 0;
        }
    }
}
