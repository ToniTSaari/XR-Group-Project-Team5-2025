using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GarnishGrabPoint : MonoBehaviour
{
    public GameObject garnishPrefab;  // Assign the correct prefab in the Inspector

    private XRGrabInteractable _grabInteractable;
    private XRInteractionManager _interactionManager; // Store a reference to the manager
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    void Start()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _grabInteractable.selectEntered.AddListener(OnGrabbed);
        // _grabInteractable.selectExited.AddListener(OnReleased); // Not needed

        // Find the XRInteractionManager in the scene.
        _interactionManager = FindObjectOfType<XRInteractionManager>();
        if (_interactionManager == null)
        {
            Debug.LogError("GarnishGrabPoint: No XRInteractionManager found in the scene!");
        }

        // Ensure the grab point's Rigidbody is kinematic and constrained!
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll; // Freeze all constraints!
        }
        else
        {
            Debug.LogError("GarnishGrabPoint: Grab point needs a Rigidbody component!");
        }

        // Store the original position and rotation
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Spawn the garnish.
        GameObject newGarnish = Instantiate(garnishPrefab, transform.position, transform.rotation);

        // Get the XRGrabInteractable from the *spawned* garnish.
        XRGrabInteractable newGarnishGrab = newGarnish.GetComponent<XRGrabInteractable>();
        if (newGarnishGrab == null)
        {
            Debug.LogError("GarnishGrabPoint: Garnish prefab does not have an XRGrabInteractable component!");
            Destroy(newGarnish); // Don't leave a broken object
            return;
        }

        // Use the _interactionManager to start the grab.
        if (args.interactorObject is IXRSelectInteractor handInteractor)
        {
            _interactionManager.SelectEnter(handInteractor, newGarnishGrab);
        }
    }

    // Use LateUpdate to reset the position *after* everything else.
    void LateUpdate()
    {
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
    }

    void OnDestroy()
    {
        if (_grabInteractable != null)
        {
            _grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            // _grabInteractable.selectExited.RemoveListener(OnReleased); // Not needed
        }
    }
}