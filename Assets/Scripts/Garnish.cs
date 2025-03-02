// Garnish.cs (attached to each Garnish prefab)
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Garnish : MonoBehaviour
{
    // --- Enum for Garnish Types ---
    public enum GarnishType { Lemon, Olive, Cherry, Ice, Mint }

    // --- Public Variables ---
    public GarnishType garnishType; // Set in Inspector

    // --- Private Variables ---
    private Rigidbody _rb;
    private XRGrabInteractable _grabInteractable;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void PlaceInGlass(Transform glassTransform)
    {
        // 1. Parent to the attachment point (which is a child of the glass).
        transform.SetParent(glassTransform);

        // 2. Disable Physics *COMPLETELY*
        _rb.isKinematic = true;
        _rb.detectCollisions = false;
        GetComponent<Collider>().enabled = false;

        // 3. Disable Grabbing
        _grabInteractable.enabled = false;

        // 4. Set *local* position and rotation to zero.
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}