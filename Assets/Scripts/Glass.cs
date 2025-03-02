// Glass.cs (attached to the *main* Glass object)
using UnityEngine;
using System.Collections.Generic;

public class Glass : MonoBehaviour
{
    private List<Garnish.GarnishType> _garnishesInGlass = new List<Garnish.GarnishType>();

    // Public method to add a garnish (called by GarnishAttachHitbox)
    public void AddGarnish(Garnish garnish)
    {
        if (garnish != null && !_garnishesInGlass.Contains(garnish.garnishType))
        {
            Rigidbody rb = garnish.GetComponent<Rigidbody>();
            if(rb != null && !rb.isKinematic)
            {
                garnish.PlaceInGlass(transform); // Parent to the glass
                _garnishesInGlass.Add(garnish.garnishType); // Add to the list
            }

        }
    }
}