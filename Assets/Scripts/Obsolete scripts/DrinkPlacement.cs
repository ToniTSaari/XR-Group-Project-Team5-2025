using Convai.Scripts.Runtime.Core;
using UnityEngine;
using System.Collections;

public class DrinkPlacement : MonoBehaviour
{
    public ConvaiNPC _npc; // Assign your Convai NPC in the Inspector
    private bool hasInteractedWithDrink = false; // Flag to prevent multiple interactions

    public DrinkChecker drinkChecker;
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
        //TempScore drink = drinkObj.GetComponent<TempScore>(); // Get the Drink component from the object
        DrinkClasses drink = drinkObj.GetComponent<DrinkClasses>(); // Get the Drink component from the object

        if (drink == null)
        {
            Debug.LogError("No Drink component found on the object.");
            return;
        }

        drinkChecker.CheckDrink(drink); // Check the drink against the recipe
        int score = drinkChecker.scoreCounter.drinkStars; // Get the score from the ScoreCounter
        string npcResponse;

        if (score > 4)
            npcResponse = "Now that’s a damn good drink.";
        else if (score > 2)
            npcResponse = "Eh, it’s alright.";
        else
            npcResponse = "Tastes like garbage. You tryna poison me?";

        _npc.TriggerSpeech(npcResponse); // Make NPC speak response
        Destroy(drinkObj); // Destroy the drink after evaluation
    }

}

