using UnityEngine;

public class BottleTiltAndPour : MonoBehaviour
{
    public ParticleSystem liquidParticles;
    public float tiltAngleThreshold = 30f; // When pouring starts
    public float pourRate = 10f; // Particle amount
    public GameObject liquidCollider;
    public GameObject liquidFill;
    public float fillSpeed = 0.1f;  // Will change fill system, placeholder
    public float maxFillHeight = 1.0f; // Placeholder fill system max height
    private float currentFill = 0.01f; //Current height.

    private bool isPouring = false;

    void Start()
    {
        // Make sure the particle system starts disabled
        liquidParticles.Stop();
        liquidParticles.Clear();
    }

    void Update()
    {
        // Tilt Detection
        float tiltAngle = Vector3.Angle(Vector3.up, transform.up);

        if (tiltAngle > tiltAngleThreshold)
        {
            if (!isPouring)
            {
                StartPouring();
            }
        }
        else
        {
            if (isPouring)
            {
                StopPouring();
            }
        }
         // Keep the liquid fill cylinder oriented.
        liquidFill.transform.up = Vector3.up;
    }

    void StartPouring()
    {
        isPouring = true;
        liquidParticles.Play();
        // Set the emission rate
        var emission = liquidParticles.emission;
        emission.rateOverTime = pourRate;
    }

    void StopPouring()
    {
        isPouring = false;
        // Stop emitting particles
        var emission = liquidParticles.emission;
        emission.rateOverTime = 0f;
        liquidParticles.Stop();
        liquidParticles.Clear();
    }
    
    void OnParticleCollision(GameObject other)
    {
      if (other == liquidCollider) {
        // Increment the fill level
        currentFill += fillSpeed * Time.deltaTime;
        currentFill = Mathf.Clamp(currentFill, 0.01f, maxFillHeight); // Keep it within bounds

        // Scale the cylinder
        Vector3 newScale = liquidFill.transform.localScale;
        newScale.y = currentFill;
        liquidFill.transform.localScale = newScale;
      }
    }
      private void OnParticleTrigger() {
        Debug.Log("Trigger activated");
    }
}