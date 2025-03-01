using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScore : MonoBehaviour
{
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        score = Random.Range(1, 10); // Assign a random score when spawned
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
