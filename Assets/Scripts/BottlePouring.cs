using UnityEngine;

public class BottlePouring : MonoBehaviour
{
    public string ingredientName;
    public Color liquidColor = Color.white; // Add this to set the color in Inspector

    public Transform pourPoint; // Assign PourPoint in Inspector
    public ParticleSystem liquidParticles;
    public float pourThresholdAngle = 45f;
    public float pourForceMultiplier = 5f;
    public float particleLifetime = 1f;

    public AudioSource pouringStartSound;
    public AudioSource pouringStopSound;

    public float maxEmissionRate = 200f;
    public float minEmissionRate = 50f;

    public AnimationCurve emissionCurve;

    public float raycastDistance = 10f;
    public LayerMask glassLayer;
    public bool showRayInSceneView = true; // Keep this for debugging


    private bool isPouring = false;
    private ParticleSystem.EmissionModule emissionModule;


    private void Start()
    {
        if (liquidParticles != null)
        {
            liquidParticles.Stop();
            emissionModule = liquidParticles.emission;
            emissionModule.enabled = false;
        }
    }

    private void Update()
    {
        if (pourPoint == null)
        {
            Debug.LogError("Pour point not assigned! Assign PourPoint in the Inspector.");
            return;
        }

        float dotProduct = Vector3.Dot(transform.up, Vector3.up);
        float pourAngle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

        float normalizedTilt = Mathf.InverseLerp(pourThresholdAngle, 90f, pourAngle);
        float curvedTilt = emissionCurve.Evaluate(normalizedTilt);
        float emissionRate = Mathf.Lerp(minEmissionRate, maxEmissionRate, curvedTilt);

        if (dotProduct < Mathf.Cos(pourThresholdAngle * Mathf.Deg2Rad))
        {
            StartPouring(emissionRate);

            // Raycast handling (when pouring)
            RaycastHit hit;
            if (Physics.Raycast(pourPoint.position, Vector3.down, out hit, raycastDistance, glassLayer))
            {
                if (showRayInSceneView)
                {
                    Debug.DrawLine(pourPoint.position, hit.point, Color.red);
                }
                // Send ingredient name AND color to LiquidGrowth.cs
                LiquidGrowth liquidGrowth = hit.collider.GetComponent<LiquidGrowth>();
                if (liquidGrowth != null)
                {
                    // First, initialize the color as soon as pouring starts
                    liquidGrowth.InitializeLiquidColor(liquidColor);
                    
                    // Then handle the actual liquid addition with particle hit
                    liquidGrowth.OnParticleRaycastHit(hit.collider.gameObject, ingredientName, liquidColor);
                }
            }
            else
            {
                if (showRayInSceneView)
                {
                    Debug.DrawLine(pourPoint.position, pourPoint.position + Vector3.down * raycastDistance, Color.green);
                }
            }
        }
        else
        {
            StopPouring();
            if (showRayInSceneView)
            {
                Debug.DrawLine(pourPoint.position, pourPoint.position + Vector3.down * 0.5f, Color.gray);
            }
        }
    }


    private void StartPouring(float emissionRate)
    {
        if (!isPouring)
        {
            isPouring = true;

            // Start particle emission
            if (liquidParticles != null)
            {
                if (!liquidParticles.isPlaying)
                {
                    liquidParticles.Play();
                }

                // Set particle color to match liquid color
                var mainModule = liquidParticles.main;
                mainModule.startColor = liquidColor;

                pouringStartSound?.Play();  // Play start sound if assigned
            }
        }

        emissionModule.enabled = true;
        emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(emissionRate);

        var main = liquidParticles.main;
        main.startLifetime = particleLifetime;

        var forceOverLifetime = liquidParticles.forceOverLifetime;
        forceOverLifetime.enabled = true;
        forceOverLifetime.y = new ParticleSystem.MinMaxCurve(-pourForceMultiplier);

        liquidParticles.transform.position = pourPoint.position;
    }

    private void StopPouring()
    {
        if (isPouring)
        {
            isPouring = false;

            if (liquidParticles != null)
            {
                if (liquidParticles.isPlaying)
                {
                    liquidParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    pouringStartSound?.Stop();
                    pouringStopSound?.Play();
                }
            }
        }
    }
}