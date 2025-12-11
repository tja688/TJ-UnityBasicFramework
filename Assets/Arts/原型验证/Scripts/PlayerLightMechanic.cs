using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLightMechanic : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("The Light2D component on the player. Will be auto-found if empty.")]
    [SerializeField] private Light2D playerLight;

    [Header("Replenishment Settings")]
    [Tooltip("Target intensity when fully replenished.")]
    public float targetIntensity = 2f;
    [Tooltip("Target falloff (outer radius) when fully replenished.")]
    public float targetFalloff = 5f;

    [Header("Decay settings")]
    public DecayAlgorithm decayType = DecayAlgorithm.Linear;
    [Tooltip("How fast the light decays per second.")]
    [Range(0.1f, 10f)]
    public float decaySpeed = 1.0f;

    [Header("Debug")]
    [SerializeField] private List<Transform> sceneLights = new List<Transform>();

    public enum DecayAlgorithm
    {
        Linear,
        Exponential,
        Cosine
    }

    private void Start()
    {
        if (playerLight == null)
        {
            playerLight = GetComponentInChildren<Light2D>();
            if (playerLight == null)
            {
                Debug.LogError("PlayerLightMechanic: No Light2D found on player!");
            }
        }

        // 2) Find all objects with tag "Light"
        var objects = GameObject.FindGameObjectsWithTag("Light");
        foreach (var obj in objects)
        {
            if (obj.transform != transform) // Don't add self if player is tagged Light
            {
                sceneLights.Add(obj.transform);
            }
        }
    }

    private void Update()
    {
        if (playerLight == null) return;

        ApplyDecay();
    }

    private void ApplyDecay()
    {
        float dt = Time.deltaTime * decaySpeed;

        switch (decayType)
        {
            case DecayAlgorithm.Linear:
                // Linear: Reduce by amount
                playerLight.intensity = Mathf.Max(0, playerLight.intensity - dt);

                // Falloff decay: Proportional to Intensity
                if (targetIntensity > 0)
                {
                    float ratio = playerLight.intensity / targetIntensity;
                    playerLight.pointLightOuterRadius = targetFalloff * ratio;
                }
                break;

            case DecayAlgorithm.Exponential:
                // Exponential: Lerp to 0
                // Use a fixed rate factor logic or just lerp
                playerLight.intensity = Mathf.Lerp(playerLight.intensity, 0, dt);

                if (targetIntensity > 0)
                {
                    // Sync radius
                    float ratio = playerLight.intensity / targetIntensity;
                    playerLight.pointLightOuterRadius = targetFalloff * ratio;
                }
                break;

            case DecayAlgorithm.Cosine:
                // Use a smooth step curve based on normalized intensity
                // First decay linearly to drive the process? Or use Time?
                // Let's just use linear reduction on value, but map radius with a curve
                playerLight.intensity = Mathf.Max(0, playerLight.intensity - dt);
                if (targetIntensity > 0)
                {
                    float normalized = playerLight.intensity / targetIntensity;
                    float curve = Mathf.SmoothStep(0, 1, normalized);
                    playerLight.pointLightOuterRadius = targetFalloff * curve;
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            // Try to find the specific script
            var source = other.GetComponent<ReplenishableLightSource>();
            if (source != null)
            {
                // 4) Logic handled via source interaction (The source gives its light)
                source.ConsumeLight(this);
            }
            else
            {
                // Generic Light object (Requirement 3 basics)
                // Just replenish to full
                Replenish(targetIntensity, targetFalloff);
            }
        }
    }

    // Called by Source or Self
    public void Replenish(float addedIntensity, float addedFalloff)
    {
        // 3) Trigger replenishment ... to a settable value
        // 4) Add source value to player
        // We add the amount, then clamp to Max (target)
        float newIntensity = playerLight.intensity + addedIntensity;
        // Don't clamp strictly if we want to allow 'Overcharge', but user said "to a settable value (Default 2)".
        // Usually implies a cap.
        playerLight.intensity = Mathf.Clamp(newIntensity, 0, targetIntensity);

        float newRadius = playerLight.pointLightOuterRadius + addedFalloff;
        playerLight.pointLightOuterRadius = Mathf.Clamp(newRadius, 0, targetFalloff);
    }
}
