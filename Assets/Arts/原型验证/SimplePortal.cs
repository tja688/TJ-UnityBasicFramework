using System.Collections;
using UnityEngine;

public class SimplePortal : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The destination portal to teleport the player to.")]
    public SimplePortal targetPortal;
    [Tooltip("Cooldown duration in seconds after teleportation.")]
    public float cooldownDuration = 1f;

    private bool isCoolingDown = false;
    private Collider2D myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        if (myCollider == null)
        {
            Debug.LogError("SimplePortal requires a Collider2D component attached to the same GameObject.");
        }
        else
        {
            // Ensure it is a trigger
            myCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only trigger if not cooling down and is Player
        if (isCoolingDown) return;

        if (collision.CompareTag("Player"))
        {
            InitiateTeleport(collision.transform);
        }
    }

    /// <summary>
    /// Teleports the player to the target portal and starts cooldown on both.
    /// </summary>
    /// <param name="playerTransform"></param>
    public void InitiateTeleport(Transform playerTransform)
    {
        if (targetPortal == null)
        {
            Debug.LogWarning($"Target portal is not assigned on {gameObject.name}.");
            return;
        }

        // Move player to target position
        // You might want to preserve offset or just snap to center. 
        // "传送到关联的对象传送门处" -> Transport to the linked object portal location.
        playerTransform.position = targetPortal.transform.position;

        // Start cooldown on both source and target
        this.StartCooldown();
        targetPortal.StartCooldown();
    }

    /// <summary>
    /// Starts the cooldown process.
    /// </summary>
    public void StartCooldown()
    {
        // Restart coroutine if already running? Or just ignore?
        // Usually if already cooling down, we might extend? 
        // But here we just ensure it is cooling down.
        // If it was already cooling down, we probably shouldn't mess with it unless we want to reset timer.
        // For simplicity and robustness, let's stop existing and start new to ensure full 1s from this event.
        StopAllCoroutines();
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownDuration);

        // Cooldown finished
        isCoolingDown = false;

        // Perform one-time instant area detection
        CheckExistingOverlap();
    }

    private void CheckExistingOverlap()
    {
        if (myCollider == null) return;

        // We use the collider's overlap check to see if the player is still inside
        Collider2D[] results = new Collider2D[8];
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter(); // Iterate and check tags manually

        int count = myCollider.Overlap(filter, results);

        for (int i = 0; i < count; i++)
        {
            if (results[i] != null && results[i].CompareTag("Player"))
            {
                // Player detected immediately after cooldown
                InitiateTeleport(results[i].transform);
                return; // Teleport once
            }
        }
    }
}
