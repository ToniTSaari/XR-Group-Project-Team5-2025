using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public float score = 0.0f;
    //public float scoreMultiplier = 100.0f;

    public void countScore(float drinkScore) 
    {
        score += drinkScore;
        Debug.Log("Score is now: " + score);
    }
}
