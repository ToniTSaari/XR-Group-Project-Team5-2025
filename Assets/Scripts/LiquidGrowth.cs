using UnityEngine;

public class LiquidGrowth : MonoBehaviour
{
    public LiquidAdder liquidAdder;

    public float growthSpeed = 1f;
    public float maxFill = 1f;
    public float baseSpillAngle = 45f; // Base angle for spilling when container is full
    public float emptySpillAngle = 85f; // Angle needed to spill when almost empty
    public float spillSpeed = 0.5f;
    public AnimationCurve spillCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float minimumFillToSpill = 0.05f; // Minimum amount of liquid needed to spill
    public float fillAmountPerParticle = 0.05f; // How much each particle contributes to filling
   
    private float currentFill;
    private Material material;
    private MeshRenderer meshRenderer;
    private Color liquidColor = Color.clear; // Start with no color (transparent)
    private float coloredLiquidAmount = 0f; // Track how much colored liquid we have
    
    void Start()
    {
        currentFill = 0f;
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        
        // Initialize the color properties in the shader
        material.SetColor("_SideColor", liquidColor);
        
        // Optionally set the top color slightly lighter
        Color topColor = Color.Lerp(liquidColor, Color.white, 0.2f);
        material.SetColor("_TopColor", topColor);
    }
    
    void Update()
    {
        // Calculate dynamic spill angle based on fill level
        float currentSpillThreshold = Mathf.Lerp(emptySpillAngle, baseSpillAngle, currentFill);
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);
        
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
               
                // Update the colored liquid amount proportionally
                if (currentFill > 0)
                {
                    coloredLiquidAmount = (currentFill - spillAmount) / currentFill * coloredLiquidAmount;
                }
                
                currentFill -= spillAmount;
                currentFill = Mathf.Max(currentFill, 0f);
            }
        }
        
        // Update shader
        material.SetFloat("_Fill", currentFill);
        
        // Apply the color to the shader's SideColor property (which is visible in the liquid)
        material.SetColor("_SideColor", liquidColor);
        
        // Optionally set the top color slightly lighter
        Color topColor = Color.Lerp(liquidColor, Color.white, 0.2f);
        material.SetColor("_TopColor", topColor);
    }

    // Public method for external calls (used by ParticleRaycaster), also returns the GameObject that was hit, for further processing
    public void OnParticleRaycastHit(GameObject glass, string ingredientName, Color particleColor)
    {
        AddLiquid(glass, ingredientName, particleColor);
    }
    
    // Method to initialize liquid color when pouring begins
    public void InitializeLiquidColor(Color bottleColor)
    {
        // Only set the initial color if the glass is empty or nearly empty
        if (currentFill < 0.01f)
        {
            liquidColor = bottleColor;
            material.SetColor("_SideColor", liquidColor);
            
            // Optionally set the top color slightly lighter
            Color topColor = Color.Lerp(liquidColor, Color.white, 0.2f);
            material.SetColor("_TopColor", topColor);
        }
    }
    
    // Centralized method to add liquid
    private void AddLiquid(GameObject glass, string ingredientName, Color particleColor)
    {
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);
        float currentSpillThreshold = Mathf.Lerp(emptySpillAngle, baseSpillAngle, currentFill);
        
        // Fill up only if not tilted too much
        if (tiltAngle < currentSpillThreshold && currentFill < maxFill)
        {
            // Calculate how much new liquid we're adding
            float newLiquidAmount = fillAmountPerParticle;
            
            // If we already have some liquid, mix the colors
            if (currentFill > 0)
            {
                // Calculate mix ratio based on existing volume vs new volume
                float existingAmount = coloredLiquidAmount;
                float totalAmount = existingAmount + newLiquidAmount;
                
                // Mix colors - weight by volume
                liquidColor = Color.Lerp(liquidColor, particleColor, newLiquidAmount / totalAmount);
                
                // Update the total colored liquid amount
                coloredLiquidAmount = totalAmount;
            }
            else
            {
                // First liquid, just use its color
                liquidColor = particleColor;
                coloredLiquidAmount = newLiquidAmount;
            }
            
            // Apply the new color to the material
            material.SetColor("_SideColor", liquidColor);
            
            // Optionally set the top color slightly lighter
            Color topColor = Color.Lerp(liquidColor, Color.white, 0.2f);
            material.SetColor("_TopColor", topColor);
            
            // Increase fill level based on particle collision
            currentFill += fillAmountPerParticle;
            currentFill = Mathf.Min(currentFill, maxFill);
            
            // gets the glass size from the hit GameObject and passes it to the liquidAdder to add liquid to the glass in the scoring game logic
            float glassSize = glass.GetComponent<GlassSize>().glassSize;
            liquidAdder.GetComponent<LiquidAdder>().pourIngredient(ingredientName, fillAmountPerParticle * glassSize);
        }
    }
}