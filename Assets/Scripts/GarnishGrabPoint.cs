// GarnishGrabPoint.cs (attached to EACH Grab Point object)
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GarnishGrabPoint : MonoBehaviour
{
    public GameObject garnishPrefab;  // Assign the correct prefab in the Inspector

    private XRGrabInteractable _grabInteractable;
    private XRInteractionManager _interactionManager; // Store a reference to the manager

    void Start()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _grabInteractable.selectEntered.AddListener(OnGrabbed);
        _grabInteractable.selectExited.AddListener(OnReleased);

        // Find the XRInteractionManager in the scene.  This is the MOST RELIABLE way.
        _interactionManager = FindObjectOfType<XRInteractionManager>();
        if (_interactionManager == null)
        {
            Debug.LogError("GarnishGrabPoint: No XRInteractionManager found in the scene!");
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Spawn the garnish.
        GameObject newGarnish = Instantiate(garnishPrefab, args.interactorObject.transform.position, args.interactorObject.transform.rotation);
        newGarnish.GetComponent<Rigidbody>().isKinematic = true;

        // --- Corrected Interaction Manager Usage ---
        if (args.interactorObject is IXRSelectInteractor handInteractor)
        {
            if (newGarnish.GetComponent<XRGrabInteractable>() is IXRSelectInteractable newGarnishGrab)
            {
                // Use the _interactionManager we found in Start().
                _interactionManager.SelectEnter(handInteractor, newGarnishGrab);
            }
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        GameObject[] garnishes = GameObject.FindGameObjectsWithTag("Garnish");
        foreach (GameObject garnish in garnishes)
        {
            Rigidbody rb = garnish.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }

    void OnDestroy()
    {
        if (_grabInteractable != null)
        {
            _grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            _grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
}