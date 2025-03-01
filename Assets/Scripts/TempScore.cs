using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScore : MonoBehaviour
{
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        TempScore drink = new TempScore();
        drink.score = Random.Range(1, 10); // Example scoring system
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
