// GarnishAttachHitbox.cs (attached to the garnishattachhitbox object)
using UnityEngine;

public class GarnishAttachHitbox : MonoBehaviour
{
    private Glass _glass;

    void Start()
    {
        _glass = GetComponentInParent<Glass>();
        if (_glass == null)
        {
            Debug.LogError("GarnishAttachHitbox: Could not find Glass script in parent!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Garnish garnish = other.GetComponent<Garnish>();
        if (garnish != null) //Simplified check
        {
            _glass.AddGarnish(garnish);
        }
    }
}