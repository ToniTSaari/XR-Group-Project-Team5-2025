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
        correctnessPercentage += drinkScore;
        drinkStars = (int)math.round(correctnessPercentage / 20);
        Debug.Log("Score is now: " + correctnessPercentage);
    }
}
