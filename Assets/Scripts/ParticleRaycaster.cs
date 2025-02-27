using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleRaycaster : MonoBehaviour
{
    public float rayLength = 2f;
    public LayerMask targetLayers;
    [Tooltip("Rate at which to check for liquid container hits")]
    public float checkRate = 0.1f;
    [Tooltip("Amount to add per successful hit")]
    public float fillAmountPerHit = 0.01f;
    
    private float lastCheckTime;
    private ParticleSystem particleSystem;
    
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        lastCheckTime = 0f;
    }
    
    void Update()
    {
        // Only check at the specified rate
        if (Time.time - lastCheckTime >= checkRate)
        {
            lastCheckTime = Time.time;
            
            // Only perform raycast if particle system is emitting
            if (particleSystem.isEmitting && particleSystem.particleCount > 0)
            {
                CastRayFromEmitter();
            }
        }
    }
    
    void CastRayFromEmitter()
    {
        // Cast ray in the forward direction of the particle system
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, targetLayers))
        {
            // Check if hit object has LiquidGrowth component directly
            LiquidGrowth liquidGrowth = hit.collider.GetComponent<LiquidGrowth>();
            
            // If not found directly, check if it's tagged as a liquid container
            if (liquidGrowth == null && hit.collider.CompareTag("Liquid"))
            {
                liquidGrowth = hit.collider.GetComponentInChildren<LiquidGrowth>();
            }
            
            // Notify the liquid growth component
            if (liquidGrowth != null)
            {
                liquidGrowth.OnParticleRaycastHit();
            }
        }
    }
    
    // Visualize the raycast in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * rayLength);
    }
}