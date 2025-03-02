// Garnish.cs (attached to each Garnish prefab)
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Garnish : MonoBehaviour
{
    // --- Enum for Garnish Types ---
    public enum GarnishType { Lemon, Olive, Cherry, Ice, Mint } // Add all your garnish types here

    // --- Public Variables ---
    public bool IsInGlass { get; private set; } = false;
    public float timeToDestroy = 15f;
    public GarnishType garnishType; // This is the important addition

    // --- Private Variables ---
    private float _timer = 0f;
    private bool _isGrounded = false;
    private Rigidbody _rb;
    private XRGrabInteractable _grabInteractable;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _grabInteractable.selectExited.AddListener(OnReleased);
    }

    void Update()
    {
        if (!IsInGlass && !_rb.isKinematic)
        {
            if (_isGrounded)
            {
                _timer += Time.deltaTime;
                if (_timer >= timeToDestroy)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            _timer = 0f;
        }
    }

    public void PlaceInGlass(Transform glassTransform)
    {
        IsInGlass = true;
        _rb.isKinematic = true;
        transform.SetParent(glassTransform, worldPositionStays: true);
        _timer = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            _isGrounded = false;
            _timer = 0;
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        _rb.isKinematic = false;
    }
    void OnDestroy()
    {
        if (_grabInteractable != null)
        {
            _grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
}