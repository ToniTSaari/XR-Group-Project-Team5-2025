using UnityEngine;
using UnityEngine.InputSystem;

public class LiquidGrowth : MonoBehaviour
{
    public float growthSpeed = 1f;
    public float maxFill = 1f;
    public float baseSpillAngle = 45f; // Base angle for spilling when container is full
    public float emptySpillAngle = 85f; // Angle needed to spill when almost empty
    public float spillSpeed = 0.5f;
    public AnimationCurve spillCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float minimumFillToSpill = 0.05f; // Minimum amount of liquid needed to spill
    
    private float currentFill;
    private Material material;
    private MeshRenderer meshRenderer;

    void Start()
    {
        currentFill = 0f;
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        meshRenderer.enabled = false;
    }

    void Update()
    {
        // Calculate dynamic spill angle based on fill level
        float currentSpillThreshold = Mathf.Lerp(emptySpillAngle, baseSpillAngle, currentFill);
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);

        if (Keyboard.current.spaceKey.isPressed)
        {
            if (!meshRenderer.enabled)
            {
                meshRenderer.enabled = true;
            }

            if (currentFill < maxFill)
            {
                // Fill up only if not tilted too much
                if (tiltAngle < currentSpillThreshold)
                {
                    currentFill += growthSpeed * Time.deltaTime;
                    currentFill = Mathf.Min(currentFill, maxFill);
                }
            }
        }

        // Spill logic with dynamic threshold
        if (currentFill > minimumFillToSpill)
        {
            if (tiltAngle > currentSpillThreshold)
            {
                // Calculate spill rate based on:
                // 1. How much the container is tilted beyond the threshold
                // 2. Current fill level (spills faster when fuller)
                float angleProgress = Mathf.InverseLerp(currentSpillThreshold, 90f, tiltAngle);
                float spillMultiplier = spillCurve.Evaluate(angleProgress);
                
                // Add fill-level influence on spill speed
                float fillInfluence = Mathf.Lerp(0.5f, 1.5f, currentFill);
                float spillAmount = spillSpeed * spillMultiplier * fillInfluence * Time.deltaTime;
                
                currentFill -= spillAmount;
                currentFill = Mathf.Max(currentFill, 0f);
            }
        }

        // Update shader
        material.SetFloat("_Fill", currentFill);
    }

    // Optional: Visualization of current spill threshold in editor
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            float threshold = Mathf.Lerp(emptySpillAngle, baseSpillAngle, currentFill);
            Gizmos.color = Color.yellow;
            Vector3 tiltDirection = Quaternion.AngleAxis(threshold, Vector3.forward) * Vector3.up;
            Gizmos.DrawRay(transform.position, tiltDirection);
        }
    }
}