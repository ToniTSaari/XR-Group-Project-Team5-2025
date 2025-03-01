using Convai.Scripts.Runtime.Core;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AutoDrink : MonoBehaviour
{
    public ConvaiNPC _npc; // Assign your NPC in the Inspector
    private bool hasInteractedWithDrink = false; // Prevents multiple activations
    private bool isPromptSent = false; // Prevents multiple sends of the same prompt


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

        TempScore drink = drinkObj.GetComponent<TempScore>(); // Get drink score component
        if (drink == null)
        {
            Debug.LogError("No TempScore component found on the drink!");
            yield break;
        }

        int score = drink.score; // Get drink score (1-10)
        string npcPrompt = $"Please drink the {drinkObj.name} and rate it. The drink quality is {score}/10.";

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

        // Rotate NPC to face the drink before grabbing it
        //Vector3 directionToDrink = (drinkObj.transform.position - _npc.transform.position).normalized;
        //_npc.transform.rotation = Quaternion.LookRotation(directionToDrink);

        // Make NPC play animations
        _npc.TriggerEvent("GrabObject"); // NPC grabs the drink
        yield return new WaitUntil(() => IsAnimationFinished("GrabObject"));


        _npc.TriggerEvent("Drinking"); // NPC starts drinking animation
        yield return new WaitUntil(() => IsAnimationFinished("Drinking"));

        //Don't add WaitForSeconds here, as it screws up the animation

        Destroy(drinkObj); // Remove the drink after it's "consumed"
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
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
    }
}