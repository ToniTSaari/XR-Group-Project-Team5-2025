using Convai.Scripts.Runtime.Core;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using static DrinkClasses;

public class AutoDrink : MonoBehaviour
{
    public ConvaiNPC _npc; // Assign your NPC in the Inspector
    private bool hasInteractedWithDrink = false; // Prevents multiple activations
    private bool isPromptSent = false; // Prevents multiple sends of the same prompt
    public DrinkChecker drinkChecker;
    public DrinkClasses drinkObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drink") && !hasInteractedWithDrink)
        {
            Debug.Log($"Drink detected: {other.name}. Sending prompt to NPC...");
            hasInteractedWithDrink = true; // Prevents retriggering

            StartCoroutine(HandleAutoDrink(other.gameObject));
        }
        else
        {
            Debug.Log("NPC already interacted with a drink or the object isn't a drink.");
        }
    }

    private IEnumerator HandleAutoDrink(GameObject drinkObj)
    {
        // Ensure the NPC is facing the drink before any action
        RotateNPCToDrink(drinkObj);

        yield return new WaitForSeconds(0.5f); // Small delay for realism

        //Temporary random score for the drink
        /*TempScore drink = drinkObj.GetComponent<TempScore>(); // Get drink score component
        if (drink == null)
        {
            Debug.LogError("No TempScore component found on the drink!");
            yield break;
        }*/

        //Proper score for the drink after proper implementation
        ScoreCounter drinkStars = drinkObj.GetComponent<ScoreCounter>(); // OLD - Get score counter component

        //DrinkClasses drinkStars = drinkObj.GetComponent<DrinkClasses>(); // Get the Drink component from the object
        if (drinkStars == null)
        {
            Debug.LogError("No ScoreCounter component found on the drink!");
            yield break;
        }
        //drinkChecker.CheckDrink(drink); // Check the drink against the recipe
        int score = drinkChecker.scoreCounter.drinkStars; // Get the score from the ScoreCounter

        //int score = drink.score; // Get drink score (0-5)
        //string npcPrompt = $"I'm the bartender asking you to please grab and drink the {drinkObj.name} and rate it. The drink quality is {score}/10."; //TempScore version
        string npcPrompt = $"Please grab and drink the {drinkObj.name} and rate it. The drink quality is {score}/5."; //ScoreCounter version

        // Log the prompt being sent
        Debug.Log($"Prompt for NPC: {npcPrompt}");

        // Ensure the prompt is not empty
        if (string.IsNullOrWhiteSpace(npcPrompt) || isPromptSent)
        {
            Debug.LogWarning("The NPC prompt is empty. Not sending to Convai.");
            yield break;
        }

        // Mark that the prompt has been sent
        isPromptSent = true;

        // Send the text prompt to the NPC
        _npc.SendTextDataAsync(npcPrompt);
        Debug.Log($"Sent NPC prompt: {npcPrompt}");

        yield return new WaitForSeconds(1); // Wait for NPC to process the request

        // Make NPC play animations
        Debug.Log("Attempting to trigger GrabObject animation.");
        _npc.TriggerEvent("GrabObject"); // NPC grabs the drink
        yield return new WaitUntil(() => IsAnimationFinished("GrabObject"));
        Debug.Log("GrabObject animation finished.");

        _npc.TriggerEvent("Drinking"); // NPC starts drinking animation
        yield return new WaitUntil(() => IsAnimationFinished("Drinking"));
        Debug.Log("Drinking animation finished.");

        //Don't add WaitForSeconds here, as it screws up the animation

        Destroy(drinkObj); // Remove the drink after it's "consumed"
        Debug.Log("Drink object destroyed.");
        drinkObject.drinks.Clear();

        hasInteractedWithDrink = false; // Reset for new drinks
        isPromptSent = false;

    }
    private void RotateNPCToDrink(GameObject drinkObj)
    {
        // Calculate direction towards the drink
        Vector3 directionToDrink = drinkObj.transform.position - _npc.transform.position;
        directionToDrink.y = 0; // Ignore the Y axis (to avoid tilting up/down)

        // Rotate NPC to face the drink
        if (directionToDrink != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(directionToDrink);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, rotation, 0.1f);
        }
    }

    private bool IsAnimationFinished(string animationName)
    {
        // Check if the current animation is the specified one and if it has finished
        Animator animator = _npc.GetComponent<Animator>();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isFinished = stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 0.99f;

        //Debug.Log($"Checking animation: {animationName} - Finished: {isFinished}, normalizedTime: {stateInfo.normalizedTime}");
        return isFinished;
    }
}