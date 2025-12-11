using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ReplenishableLightSource : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("The Light2D on this object. Auto-finds on self or children.")]
    [SerializeField] private Light2D myLight;

    [Header("Regeneration")]
    public float recoverSpeed = 1f;
    public RecoveryType recoveryType = RecoveryType.Linear;

    public enum RecoveryType { Linear }

    // Snapshot values
    private float initialIntensity;
    private float initialRadius;
    private bool initialized = false;

    private void Start()
    {
        // 4) Require associating own point light (Default empty, find self/child)
        if (myLight == null)
        {
            myLight = GetComponent<Light2D>();
            if (myLight == null) myLight = GetComponentInChildren<Light2D>();
        }

        if (myLight != null)
        {
            // 4) Store snapshot
            initialIntensity = myLight.intensity;
            initialRadius = myLight.pointLightOuterRadius;
            initialized = true;
        }
        else
        {
            Debug.LogWarning($"ReplenishableLightSource on {name} could not find a Light2D component.");
        }
    }

    private void Update()
    {
        if (!initialized || myLight == null) return;

        // 4) Slowly recover to snapshot preset
        if (myLight.intensity < initialIntensity)
        {
            float dt = Time.deltaTime * recoverSpeed;
            if (recoveryType == RecoveryType.Linear)
            {
                myLight.intensity += dt;
                // Clamp
                if (myLight.intensity > initialIntensity) myLight.intensity = initialIntensity;
            }

            // Sync radius recovery
            if (initialIntensity > 0)
            {
                float ratio = myLight.intensity / initialIntensity;
                myLight.pointLightOuterRadius = initialRadius * ratio;
            }
        }
    }

    /// <summary>
    /// Called when player touches this light.
    /// Transfers light to player and dims self.
    /// </summary>
    /// <param name="player"></param>
    public void ConsumeLight(PlayerLightMechanic player)
    {
        if (!initialized || myLight == null) return;

        // Don't consume if empty
        if (myLight.intensity <= 0.05f) return;

        // 4) Add own intensity-decay value to player
        // We transfer current available light.
        float amountIntensity = myLight.intensity;
        float amountRadius = myLight.pointLightOuterRadius;

        player.Replenish(amountIntensity, amountRadius);

        // Dim self (Consumption)
        myLight.intensity = 0;
        myLight.pointLightOuterRadius = 0;
    }
}
