using Convai.Scripts.Runtime.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDrink : MonoBehaviour
{
    public ConvaiNPC _npc;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drink"))
        {
            Debug.Log($"Drink detected: {other.name}. Sending prompt to NPC...");

            StartCoroutine(HandleAutoDrink(other.gameObject));
        }
    }
    private IEnumerator HandleAutoDrink(GameObject drinkObj)
    {
        yield return new WaitForSeconds(0.05f); // Small delay for realism

        TempScore drink = drinkObj.GetComponent<TempScore>();
        int score = drink.score;
        _npc.SendTextDataAsync($"Please drink the {drinkObj.name}.");
        _npc.TriggerEvent("GrabObject"); // NPC starts drinking animation
        _npc.TriggerEvent("Drinking"); // NPC starts drinking animation


        Destroy(drinkObj); // Remove the drink after it's "consumed"

    }
}
