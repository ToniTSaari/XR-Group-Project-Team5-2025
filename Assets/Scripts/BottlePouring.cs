using UnityEngine;

public class BottlePouring: MonoBehaviour
{
    public Transform bottleMouth;
    public ParticleSystem liquidParticles;
    public float pourThresholdAngle = 45f;
    public float pourForceMultiplier = 5f;
    public float particleLifetime = 1f;

    public AudioSource pouringStartSound;
    public AudioSource pouringStopSound;

    public float maxEmissionRate = 200f; // Maximum particles per second
    public float minEmissionRate = 50f;  // Minimum particles per second

    public AnimationCurve emissionCurve; // Add this for the AnimationCurve

    private bool isPouring = false;
    private ParticleSystem.EmissionModule emissionModule;

    private void Start()
    {
        if (liquidParticles!= null)
        {
            liquidParticles.Stop();
            emissionModule = liquidParticles.emission;
            emissionModule.enabled = false;
        }
    }

    private void Update()
    {
        // Use dot product instead of Vector3.Angle
        float dotProduct = Vector3.Dot(transform.up, Vector3.up);
        float pourAngle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg; // Convert to degrees

        // Calculate a normalized tilt value between 0 and 1
        float normalizedTilt = Mathf.InverseLerp(pourThresholdAngle, 90f, pourAngle);

        // Use the AnimationCurve to get the curved tilt
        float curvedTilt = emissionCurve.Evaluate(normalizedTilt);

        // Calculate emission rate based on the curved tilt
        float emissionRate = Mathf.Lerp(minEmissionRate, maxEmissionRate, curvedTilt);

        // Use dot product for threshold comparison
        if (dotProduct < Mathf.Cos(pourThresholdAngle * Mathf.Deg2Rad)) // Convert threshold to radians
        {
            StartPouring(emissionRate);
        }
        else
        {
            StopPouring();
        }
    }

    private void StartPouring(float emissionRate)
    {
        if (!isPouring)
        {
            isPouring = true;

            if (liquidParticles!= null &&!liquidParticles.isPlaying)
            {
                liquidParticles.Play();
                pouringStartSound?.Play();
            }
        }

        emissionModule.enabled = true;
        emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(emissionRate);

        var main = liquidParticles.main;
        main.startLifetime = particleLifetime;

        var forceOverLifetime = liquidParticles.forceOverLifetime;
        forceOverLifetime.enabled = true;
        forceOverLifetime.y = new ParticleSystem.MinMaxCurve(-pourForceMultiplier);

        liquidParticles.transform.position = bottleMouth.position;
    }

    private void StopPouring()
    {
        if (isPouring)
        {
            isPouring = false;

            if (liquidParticles!= null)
            {
                if (liquidParticles.isPlaying)
                {
                    liquidParticles.Stop();
                    pouringStartSound?.Stop();
                    pouringStopSound?.Play();
                }

                emissionModule.enabled = false;
                liquidParticles.Clear();
                liquidParticles.Play();
                liquidParticles.Stop();
            }
        }
    }
}