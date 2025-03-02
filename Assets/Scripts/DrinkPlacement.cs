using Convai.Scripts.Runtime.Core;
using UnityEngine;
using System.Collections;

public class DrinkPlacement : MonoBehaviour
{
    public ConvaiNPC _npc; // Assign your Convai NPC in the Inspector
    private bool hasInteractedWithDrink = false; // Flag to prevent multiple interactions


    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is the drink and the NPC hasn't interacted yet
        if (other.CompareTag("Drink") && !hasInteractedWithDrink)
        {
            Debug.Log("Drink placed! Notifying NPC...");
            _npc.TriggerEvent("NoticeDrink"); // NPC notices the drink
            MakeNPCDrink(other.gameObject);
            hasInteractedWithDrink = true; // Set flag to prevent re-triggering
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset the interaction flag if the drink leaves the trigger zone
        if (other.CompareTag("Drink"))
        {
            hasInteractedWithDrink = false;
        }
    }


    void MakeNPCDrink(GameObject drink)
    {
        _npc.TriggerEvent("GrabObject"); // NPC grabs the drink
        StartCoroutine(DelayBeforeDrinking(drink));
    }

    private IEnumerator DelayBeforeDrinking(GameObject drink)
    {
        yield return new WaitForSeconds(3); // Wait before drinking
        _npc.TriggerEvent("Drinking");
        yield return new WaitForSeconds(7); // Wait for the drinking animation to finish
        EvaluateDrink(drink);
    }

    void EvaluateDrink(GameObject drinkObj)
    {
        TempScore drink = drinkObj.GetComponent<TempScore>(); // Get the Drink component from the object

        if (drink == null)
        {
            Debug.LogError("No Drink component found on the object.");
            return;
        }

        int score = drink.score; // Get the score attached to the drink
        string npcResponse;

        if (score > 8)
            npcResponse = "Now that’s a damn good drink.";
        else if (score > 5)
            npcResponse = "Eh, it’s alright.";
        else
            npcResponse = "Tastes like garbage. You tryna poison me?";

        _npc.TriggerSpeech(npcResponse); // Make NPC speak response
        Destroy(drinkObj); // Destroy the drink after evaluation
    }

}

