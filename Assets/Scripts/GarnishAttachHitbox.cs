// GarnishAttachHitbox.cs
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class GarnishAttachHitbox : MonoBehaviour
{
    private Glass _glass; // Reference to the Glass script
    private XRGrabInteractable _heldGarnishInteractable = null;

    private void Start()
    {
        // IMPORTANT CHANGE: Find the Glass component on the *parent*.
        _glass = GetComponentInParent<Glass>();
        if (_glass == null)
        {
            Debug.LogError("GarnishAttachHitbox: Could not find Glass script in parent!");
            enabled = false;
            return;
        }
          //Ensure that this is a trigger
            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
            if(GetComponent<Rigidbody>() != null){
                Debug.LogError("GarnishAttachHitbox should not have a rigidbody attached");
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Garnish"))  // Check for Garnish tag
        {
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                _heldGarnishInteractable = grabInteractable;
                _heldGarnishInteractable.selectExited.AddListener(OnGarnishReleased);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
      if (other.gameObject.CompareTag("Garnish"))
        {
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();

            // Only clear if this is the *same* garnish we were tracking.
            if(grabInteractable != null)
            {
                grabInteractable.selectExited.RemoveListener(OnGarnishReleased);
                if(grabInteractable == _heldGarnishInteractable)
                {
                    _heldGarnishInteractable = null;
                }
            }
        }
    }

    private void OnGarnishReleased(SelectExitEventArgs args)
    {
        if (_heldGarnishInteractable != null)
        {
            Garnish garnish = _heldGarnishInteractable.GetComponent<Garnish>();
            if (garnish != null)
            {
                _glass.AddGarnish(garnish); // Add it to the glass!
            }
            _heldGarnishInteractable = null;
        }
    }

    private void OnDestroy()
    {
        if (_heldGarnishInteractable != null)
        {
            _heldGarnishInteractable.selectExited.RemoveListener(OnGarnishReleased);
        }
    }
}