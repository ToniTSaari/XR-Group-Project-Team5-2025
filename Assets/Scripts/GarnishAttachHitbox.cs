// GarnishAttachHitbox.cs
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))] // Ensure a Collider is present
public class GarnishAttachHitbox : MonoBehaviour
{
    private Glass _glass; // Reference to the parent Glass script
    private XRGrabInteractable _heldGarnishInteractable = null;

    private void Start()
    {
        // Get the Glass script from the *parent* object.
        _glass = GetComponentInParent<Glass>();
        if (_glass == null)
        {
            Debug.LogError("GarnishAttachHitbox: Could not find Glass script in parent!");
            enabled = false; // Disable this script if no Glass is found
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
        if (other.gameObject.CompareTag("Garnish"))
        {
            // Check if we are currently holding this garnish.
             XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
              if (grabInteractable != null && grabInteractable.isSelected)
              {
                _heldGarnishInteractable = grabInteractable;
                grabInteractable.selectExited.AddListener(OnGarnishReleased); //Listen for release
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
        if (_heldGarnishInteractable != null) // Check if we were tracking a garnish
        {
            // Get the Garnish component.  More robust to get it *now*.
            Garnish garnish = _heldGarnishInteractable.GetComponent<Garnish>();
            if (garnish != null)
            {
                _glass.AddGarnish(garnish); // Add it to the glass!
            }
             _heldGarnishInteractable = null; //Clear the reference
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