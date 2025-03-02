// Glass.cs
using UnityEngine;
using System.Collections.Generic;

public class Glass : MonoBehaviour
{
    private readonly List<Garnish.GarnishType> _garnishesInGlass = new List<Garnish.GarnishType>();

    // Add a serialized field for attachment points
    [System.Serializable] // This is important so it shows up in the Inspector
    public struct GarnishAttachmentPoint
    {
        public Garnish.GarnishType garnishType;
        public Transform attachmentPoint;
    }

    [SerializeField] private List<GarnishAttachmentPoint> attachmentPoints = new List<GarnishAttachmentPoint>();


    public void AddGarnish(Garnish garnish)
    {
        if (garnish == null) { return; }
        if (_garnishesInGlass.Contains(garnish.garnishType)) { return; }

        // Find the attachment point for this garnish type
        Transform attachPoint = null;
        foreach (var point in attachmentPoints)
        {
            if (point.garnishType == garnish.garnishType)
            {
                attachPoint = point.attachmentPoint;
                break; // Exit the loop once found
            }
        }

        if (attachPoint == null)
        {
            Debug.LogWarning("No attachment point found for garnish type: " + garnish.garnishType);
            // Fallback: Attach to the glass itself (you could also choose to *not* attach)
            garnish.PlaceInGlass(transform);
        }
        else
        {
            garnish.PlaceInGlass(attachPoint); // Use the attachment point!
        }

        _garnishesInGlass.Add(garnish.garnishType);
    }

      public void RemoveGarnish(Garnish.GarnishType type)
    {
        _garnishesInGlass.Remove(type);
    }
     private void OnValidate()
    {
        //This will cause a minor error on runtime, as it cannot be called. This is fine.
        if (!Application.isPlaying)
        {
            _garnishesInGlass.Clear();
            foreach(Transform child in transform){
                Garnish garnish = child.GetComponent<Garnish>();
                if(garnish != null){
                    _garnishesInGlass.Add(garnish.garnishType);
                }
            }
        }
    }
}